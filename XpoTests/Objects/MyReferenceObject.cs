using System;
using System.ComponentModel;
using DevExpress.Xpo;

namespace XpoTests
{
    public class MyReferenceObject : MyObjectBase
    {
        public MyReferenceObject(Session session)
            : base(session)
        {
        }

        public override void RandomizeFields(ReferenceDataProvider referenceDataProvider)
        {
            StringProperty = Randomizer.RandomString(10);
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
    }
}
