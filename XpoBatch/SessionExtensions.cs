using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DevExpress.Data.Filtering;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Generators;
using DevExpress.Xpo.Metadata;
using DevExpress.Xpo.Metadata.Helpers;

namespace XpoBatch
{
    public class PropertyValueStore : List<KeyValuePair<XPMemberInfo, Object>>
    {
    }

    public static class SessionExtensions
    {
        public static int GetObjectCount<T>(this Session session, CriteriaOperator criteria = null)
        {
            return (int)session.Evaluate<T>(CriteriaOperator.Parse("Count()"), criteria);
        }

        public static PropertyValueStore CreatePropertyValueStore(XPClassInfo classInfo, MemberInitExpression memberInitExpression)
        {
            PropertyValueStore propertyValueStore = new PropertyValueStore();

            /// Parse each expression binding within the anonymous class.  
            /// Each binding represents a property assignment within the IXPObject.
            /// Add a KeyValuePair for the corresponding MemberInfo and (invoked) value.
            foreach (var binding in memberInitExpression.Bindings)
            {
                var assignment = binding as MemberAssignment;
                if (binding == null)
                {
                    throw new NotImplementedException("All bindings inside the MemberInitExpression are expected to be of type MemberAssignment.");
                }

                // Get the memberInfo corresponding to the property name.
                string memberName = binding.Member.Name;
                XPMemberInfo memberInfo = classInfo.GetMember(memberName);
                if (memberInfo == null)
                    throw new ArgumentOutOfRangeException(memberName, String.Format("The member {0} of the {1} class could not be found.", memberName, classInfo.FullName));

                if (!memberInfo.IsPersistent)
                    throw new ArgumentException(memberName, String.Format("The member {0} of the {1} class is not persistent.", memberName, classInfo.FullName));

                // Compile and invoke the assignment expression to obtain the contant value to add as a parameter.
                var constant = Expression.Lambda(assignment.Expression, null).Compile().DynamicInvoke();

                // Add the 
                propertyValueStore.Add(new KeyValuePair<XPMemberInfo, Object>(memberInfo, constant));
            }
            return propertyValueStore;
        }

        public static ModificationResult Delete<T>(this Session session, CriteriaOperator criteria) where T : IXPObject
        {
            if (ReferenceEquals(criteria, null))
                criteria = CriteriaOperator.Parse("True");
            XPClassInfo classInfo = session.GetClassInfo(typeof(T));
            /// if you are using DevExpress 11.2 or earlier
            /// var batchWideData = new BatchWideDataHolder(session);
            var batchWideData = new BatchWideDataHolder4Modification(session);
            int recordsAffected = (int)session.Evaluate<T>(CriteriaOperator.Parse("Count()"), criteria);
            List<ModificationStatement> collection = DeleteQueryGenerator.GenerateDelete(classInfo, criteria, batchWideData);
            foreach (ModificationStatement item in collection)
            {
                item.RecordsAffected = recordsAffected;
            }
            ModificationStatement[] collectionToArray = collection.ToArray<ModificationStatement>();
            ModificationResult result = session.DataLayer.ModifyData(collectionToArray);
            return result;
        }

        public static ModificationResult Update<T>(this Session session, Expression<Func<T>> evaluator, CriteriaOperator criteria) where T : IXPObject
        {
            if (ReferenceEquals(criteria, null))
                criteria = CriteriaOperator.Parse("True");

            XPClassInfo classInfo = session.GetClassInfo(typeof(T));
            /// if you are using DevExpress 11.2 or earlier
            /// var batchWideData = new BatchWideDataHolder(session);
            var batchWideData = new BatchWideDataHolder4Modification(session);
            int recordsAffected = (int)session.Evaluate<T>(CriteriaOperator.Parse("Count()"), criteria);

            /// Parse the Expression.
            /// Expect to find a single MemberInitExpression.
            PropertyValueStore propertyValueStore = null;
            int memberInitCount = 1;
            evaluator.Visit<MemberInitExpression>(expression =>
                {
                    if (memberInitCount > 1)
                    {
                        throw new NotImplementedException("Only a single MemberInitExpression is allowed for the evaluator parameter.");
                    }
                    memberInitCount++;
                    propertyValueStore = CreatePropertyValueStore(classInfo, expression);
                    return expression;
                });

            MemberInfoCollection properties = new MemberInfoCollection(classInfo, propertyValueStore.Select(x => x.Key).ToArray());

            List<ModificationStatement> collection = UpdateQueryGenerator.GenerateUpdate(classInfo, properties, criteria, batchWideData);
            foreach (UpdateStatement updateStatement in collection.OfType<UpdateStatement>())
            {
                for (int i = 0; i < updateStatement.Parameters.Count; i++)
                {
                    // support for converter
                    var valueConverter = propertyValueStore[i].Key.Converter;
                    Object value = (valueConverter != null) ? valueConverter.ConvertToStorageType(propertyValueStore[i].Value) : propertyValueStore[i].Value;
                    if (value is IXPObject)
                        updateStatement.Parameters[i].Value = ((IXPObject)(value)).ClassInfo.GetId(value);
                    else
                        updateStatement.Parameters[i].Value = value;
                }
                updateStatement.RecordsAffected = recordsAffected;
            }
            return session.DataLayer.ModifyData(collection.ToArray<ModificationStatement>());
        }
    }
}
