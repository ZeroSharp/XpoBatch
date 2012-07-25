using DevExpress.Xpo;

namespace XpoTests
{
    [NonPersistent]
    public abstract class MyObjectBase : XPBaseObject
    {
        public MyObjectBase(Session session)
            : base(session)
        {
        }

        public abstract void RandomizeFields(ReferenceDataProvider referenceDataProvider);
    }
}
