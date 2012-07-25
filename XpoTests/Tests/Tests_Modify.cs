using System;
using DevExpress.Data.Filtering;
using DevExpress.Xpo;
using NUnit.Framework;
using XpoBatch;

namespace XpoTests
{
    [TestFixture]
    public class Tests_Modify_Enum : TestBase
    {
        [Test]
        public void Test_ModifyAll_Enum()
        {
            using (UnitOfWork uow = new UnitOfWork(DefaultDataLayer))
            {
                Assert.Less(uow.GetObjectCount<MySimpleObject>(MySimpleObject.Fields.EnumProperty == new OperandValue(MyEnum.Blue)), 1000);
                uow.Update<MySimpleObject>(() => new MySimpleObject(uow) { EnumProperty = MyEnum.Blue }, null);
                Assert.AreEqual(1000, uow.GetObjectCount<MySimpleObject>(MySimpleObject.Fields.EnumProperty == new OperandValue(MyEnum.Blue)));
            }
        }

        [Test]
        public void Test_ModifySome_Enum()
        {
            CriteriaOperator criteria = CriteriaOperator.Parse("StartsWith([StringProperty], 'A')");

            using (UnitOfWork uow = new UnitOfWork(DefaultDataLayer))
            {
                int affectedRecords = uow.GetObjectCount<MySimpleObject>(MySimpleObject.Fields.EnumProperty == new OperandValue(MyEnum.Blue) & !criteria)
                                    + uow.GetObjectCount<MySimpleObject>(criteria);
                uow.Update<MySimpleObject>(() => new MySimpleObject(uow) { EnumProperty = MyEnum.Blue }, criteria);
                Assert.AreEqual(affectedRecords, uow.GetObjectCount<MySimpleObject>(MySimpleObject.Fields.EnumProperty == new OperandValue(MyEnum.Blue)));
            }
        }

        [Test]
        public void Test_ModifyNone_Enum()
        {
            CriteriaOperator criteria = CriteriaOperator.Parse("1=0");

            using (UnitOfWork uow = new UnitOfWork(DefaultDataLayer))
            {
                int affectedRecords = uow.GetObjectCount<MySimpleObject>(MySimpleObject.Fields.EnumProperty == new OperandValue(MyEnum.Blue));
                uow.Update<MySimpleObject>(() => new MySimpleObject(uow) { EnumProperty = MyEnum.Blue }, criteria);
                Assert.AreEqual(affectedRecords, uow.GetObjectCount<MySimpleObject>(MySimpleObject.Fields.EnumProperty == new OperandValue(MyEnum.Blue)));
            }
        }
    }

    [TestFixture]
    public class Tests_Modify_Integer : TestBase
    {
        [Test]
        public void Test_ModifyAll_Integer()
        {
            using (UnitOfWork uow = new UnitOfWork(DefaultDataLayer))
            {
                Assert.Less(uow.GetObjectCount<MySimpleObject>(MySimpleObject.Fields.IntegerProperty == 23), 1000);
                uow.Update<MySimpleObject>(() => new MySimpleObject(uow) { IntegerProperty = 23 }, null);
                Assert.AreEqual(1000, uow.GetObjectCount<MySimpleObject>(MySimpleObject.Fields.IntegerProperty == 23));
            }
        }

        [Test]
        public void Test_ModifySome_Integer()
        {
            CriteriaOperator criteria = CriteriaOperator.Parse("StartsWith([StringProperty], 'A')");

            using (UnitOfWork uow = new UnitOfWork(DefaultDataLayer))
            {
                int affectedRecords = uow.GetObjectCount<MySimpleObject>(MySimpleObject.Fields.IntegerProperty == 23 & !criteria)
                                + uow.GetObjectCount<MySimpleObject>(criteria);
                uow.Update<MySimpleObject>(() => new MySimpleObject(uow) { IntegerProperty = 23 }, criteria);
                Assert.AreEqual(affectedRecords, uow.GetObjectCount<MySimpleObject>(MySimpleObject.Fields.IntegerProperty == 23));
            }
        }

        [Test]
        public void Test_ModifyNone_Integer()
        {
            CriteriaOperator criteria = CriteriaOperator.Parse("1=0");

            using (UnitOfWork uow = new UnitOfWork(DefaultDataLayer))
            {
                int affectedRecords = uow.GetObjectCount<MySimpleObject>(MySimpleObject.Fields.IntegerProperty == 23);
                uow.Update<MySimpleObject>(() => new MySimpleObject(uow) { IntegerProperty = 23 }, criteria);
                Assert.AreEqual(affectedRecords, uow.GetObjectCount<MySimpleObject>(MySimpleObject.Fields.IntegerProperty == 23));
            }
        }

    }

    [TestFixture]
    public class Tests_Modify_String : TestBase
    {
        [Test]
        public void Test_ModifyAll_String()
        {
            using (UnitOfWork uow = new UnitOfWork(DefaultDataLayer))
            {
                Assert.Less(uow.GetObjectCount<MySimpleObject>(MySimpleObject.Fields.StringProperty == "abcdefghjiklmnopqrstuvwxyz"), 1000);
                uow.Update<MySimpleObject>(() => new MySimpleObject(uow) { StringProperty = "abcdefghjiklmnopqrstuvwxyz" }, null);
                Assert.AreEqual(1000, uow.GetObjectCount<MySimpleObject>(MySimpleObject.Fields.StringProperty == "abcdefghjiklmnopqrstuvwxyz"));
            }
        }

        [Test]
        public void Test_ModifySome_String()
        {
            CriteriaOperator criteria = MySimpleObject.Fields.EnumProperty == new OperandValue(MyEnum.Blue);

            using (UnitOfWork uow = new UnitOfWork(DefaultDataLayer))
            {
                int affectedRecords = uow.GetObjectCount<MySimpleObject>(MySimpleObject.Fields.StringProperty == "abcdefghjiklmnopqrstuvwxyz" & !criteria)
                                    + uow.GetObjectCount<MySimpleObject>(criteria);
                uow.Update<MySimpleObject>(() => new MySimpleObject(uow) { StringProperty = "abcdefghjiklmnopqrstuvwxyz" }, criteria);
                Assert.AreEqual(affectedRecords, uow.GetObjectCount<MySimpleObject>(MySimpleObject.Fields.StringProperty == "abcdefghjiklmnopqrstuvwxyz"));
            }
        }

        [Test]
        public void Test_ModifyNone_String()
        {
            CriteriaOperator criteria = CriteriaOperator.Parse("1=0");

            using (UnitOfWork uow = new UnitOfWork(DefaultDataLayer))
            {
                int affectedRecords = uow.GetObjectCount<MySimpleObject>(MySimpleObject.Fields.StringProperty == "abcdefghjiklmnopqrstuvwxyz");
                uow.Update<MySimpleObject>(() => new MySimpleObject(uow) { StringProperty = "abcdefghjiklmnopqrstuvwxyz" }, criteria);
                Assert.AreEqual(affectedRecords, uow.GetObjectCount<MySimpleObject>(MySimpleObject.Fields.StringProperty == "abcdefghjiklmnopqrstuvwxyz"));
            }
        }
    }

    [TestFixture]
    public class Tests_Modify_ReferenceObject : TestBase
    {
        [Test]
        public void Test_ModifyAll_ReferenceObject()
        {
            using (UnitOfWork uow = new UnitOfWork(DefaultDataLayer))
            {
                var referenceObject = uow.FindObject<MyReferenceObject>(null);
                Assert.AreNotEqual(Guid.Empty, referenceObject);
                Guid referenceId = referenceObject.Id;
                Assert.Less(uow.GetObjectCount<MySimpleObject>(MySimpleObject.Fields.ReferenceProperty.Id == referenceId), 1000);
                uow.Update<MySimpleObject>(() => new MySimpleObject(uow) { ReferenceProperty = referenceObject }, null);
                Assert.AreEqual(1000, uow.GetObjectCount<MySimpleObject>(MySimpleObject.Fields.ReferenceProperty.Id == referenceId));
            }
        }

        [Test]
        public void Test_ModifySome_ReferenceObject()
        {
            CriteriaOperator criteria = CriteriaOperator.Parse("1=0");

            using (UnitOfWork uow = new UnitOfWork(DefaultDataLayer))
            {
                var referenceObject = uow.FindObject<MyReferenceObject>(null);
                Assert.AreNotEqual(Guid.Empty, referenceObject);
                Guid referenceId = referenceObject.Id;
                int affectedRecords = uow.GetObjectCount<MySimpleObject>(MySimpleObject.Fields.ReferenceProperty.Id == referenceId);
                uow.Update<MySimpleObject>(() => new MySimpleObject(uow) { ReferenceProperty = referenceObject }, criteria);
                Assert.AreEqual(affectedRecords, uow.GetObjectCount<MySimpleObject>(MySimpleObject.Fields.ReferenceProperty.Id == referenceId));
            }
        }

        [Test]
        public void Test_ModifyNone_ReferenceObject()
        {
            CriteriaOperator criteria = CriteriaOperator.Parse("StartsWith([StringProperty], 'A')");
            using (UnitOfWork uow = new UnitOfWork(DefaultDataLayer))
            {
                var referenceObject = uow.FindObject<MyReferenceObject>(null);
                Assert.AreNotEqual(Guid.Empty, referenceObject);
                Guid referenceId = referenceObject.Id;
                int affectedRecords = uow.GetObjectCount<MySimpleObject>(MySimpleObject.Fields.ReferenceProperty.Id == referenceId & !criteria)
                                    + uow.GetObjectCount<MySimpleObject>(criteria);
                uow.Update<MySimpleObject>(() => new MySimpleObject(uow) { ReferenceProperty = referenceObject }, criteria);
                Assert.AreEqual(affectedRecords, uow.GetObjectCount<MySimpleObject>(MySimpleObject.Fields.ReferenceProperty.Id == referenceId));
            }
        }
    }
}
