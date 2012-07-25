using System;
using System.ComponentModel;
using DevExpress.Xpo;

namespace XpoTests
{
    public class MySimpleObject : MyObjectBase
    {
        public MySimpleObject(Session session) : base(session)
        {
        }

        public override void RandomizeFields(ReferenceDataProvider referenceDataProvider)
        {
            StringProperty = Randomizer.RandomString(10);
            EnumProperty = Randomizer.RandomEnum<MyEnum>();
            IntegerProperty = Randomizer.RandomInteger(10);
            ReferenceProperty = referenceDataProvider.MyReferenceObjects[Randomizer.RandomInteger(referenceDataProvider.MyReferenceObjects.Count - 1)];
        }

        public override void AfterConstruction()
        {
            _Id = XpoDefault.NewGuid();
            base.AfterConstruction();
        }

        [Key(AutoGenerate = false)]
        [Persistent("ID")]
        private Guid _Id;
        [Browsable(false)]
        [PersistentAlias("_Id")]
        public Guid Id
        {
            get
            {
                return _Id;
            }
        }

        private MyEnum _EnumProperty;
        public MyEnum EnumProperty
        {
            get
            {
                return _EnumProperty;
            }
            set
            {
                SetPropertyValue("EnumProperty", ref _EnumProperty, value);
            }
        }

        private string _StringProperty;
        public string StringProperty
        {
            get
            {
                return _StringProperty;
            }
            set
            {
                SetPropertyValue("StringProperty", ref _StringProperty, value);
            }
        }

        private int _IntegerProperty;
        public int IntegerProperty
        {
            get
            {
                return _IntegerProperty;
            }
            set
            {
                SetPropertyValue("IntegerProperty", ref _IntegerProperty, value);
            }
        }

        private MyReferenceObject _ReferenceProperty;
        public MyReferenceObject ReferenceProperty
        {
            get { return _ReferenceProperty; }
            set
            {
                _ReferenceProperty = value;
            }
        }       

        private static FieldsClass _Fields;
        public new static FieldsClass Fields
        {
            get
            {
                if (ReferenceEquals(_Fields, null))
                    _Fields = new FieldsClass();
                return _Fields;
            }
        }
        //Created/Updated: babbage\ra on BABBAGE at 7/25/2012 12:02 PM
        public new class FieldsClass : MyObjectBase.FieldsClass
        {
            public FieldsClass()
                : base()
            {
            }
            public FieldsClass(string propertyName)
                : base(propertyName)
            {
            }
            public DevExpress.Data.Filtering.OperandProperty _Id
            {
                get
                {
                    return new DevExpress.Data.Filtering.OperandProperty(GetNestedName("_Id"));
                }
            }
            public DevExpress.Data.Filtering.OperandProperty Id
            {
                get
                {
                    return new DevExpress.Data.Filtering.OperandProperty(GetNestedName("Id"));
                }
            }
            public DevExpress.Data.Filtering.OperandProperty EnumProperty
            {
                get
                {
                    return new DevExpress.Data.Filtering.OperandProperty(GetNestedName("EnumProperty"));
                }
            }
            public DevExpress.Data.Filtering.OperandProperty StringProperty
            {
                get
                {
                    return new DevExpress.Data.Filtering.OperandProperty(GetNestedName("StringProperty"));
                }
            }
            public DevExpress.Data.Filtering.OperandProperty IntegerProperty
            {
                get
                {
                    return new DevExpress.Data.Filtering.OperandProperty(GetNestedName("IntegerProperty"));
                }
            }
            public XpoTests.MySimpleObject.FieldsClass ReferenceProperty
            {
                get
                {
                    return new XpoTests.MySimpleObject.FieldsClass(GetNestedName("ReferenceProperty"));
                }
            }
        }
    }
}
