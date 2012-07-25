using System.Collections.Generic;
using System.Linq;
using DevExpress.Xpo;

namespace XpoTests
{
    public class ReferenceDataProvider
    {
        public ReferenceDataProvider(Session session)
        {
            _Session = session;
        }

        private Session _Session;
        public Session Session
        {
            get { return _Session; }
        }

        private List<MyReferenceObject> _MyReferenceObjects;
        public List<MyReferenceObject> MyReferenceObjects
        {
            get
            {
                // Create on demand...
                if (_MyReferenceObjects == null)
                {
                    var myReferenceObjects = new XPCollection<MyReferenceObject>(Session);
                    _MyReferenceObjects = myReferenceObjects.ToList();
                }
                return _MyReferenceObjects;
            }
        }
    }
}
