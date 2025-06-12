using DevExpress.Data.Filtering;
using DevExpress.Xpo;
using NUnit.Framework;
using XpoBatch;

namespace XpoTests
{
    [TestFixture]
    public class Tests_Delete : TestBase
    {
        [Test]
        public void Test_DeleteAll()
        {
            using (UnitOfWork uow = new UnitOfWork(DefaultDataLayer))
            {
                Assert.AreEqual(1000, uow.GetObjectCount<MySimpleObject>());
                uow.Delete<MySimpleObject>(null);
                Assert.AreEqual(0, uow.GetObjectCount<MySimpleObject>());
            }            
        }

        [Test]
        public void Test_DeleteSome()
        {
            CriteriaOperator criteria = CriteriaOperator.Parse("StartsWith([StringProperty], 'A')");

            using (UnitOfWork uow = new UnitOfWork(DefaultDataLayer))
            {
                int recordsAffected = uow.GetObjectCount<MySimpleObject>(criteria);
                Assert.Greater(recordsAffected, 0);
                uow.Delete<MySimpleObject>(criteria);
                Assert.AreEqual(1000 - recordsAffected, uow.GetObjectCount<MySimpleObject>());
            }
        }

        [Test]
        public void Test_DeleteSome_With_PersistentAlias_Criteria()
        {
            CriteriaOperator criteria = CriteriaOperator.Parse("StartsWith([PersistentAliasProperty], 'A')");

            using (UnitOfWork uow = new UnitOfWork(DefaultDataLayer))
            {
                int recordsAffected = uow.GetObjectCount<MySimpleObject>(criteria);
                Assert.Greater(recordsAffected, 0);
                uow.Delete<MySimpleObject>(criteria);
                Assert.AreEqual(1000 - recordsAffected, uow.GetObjectCount<MySimpleObject>());
            }
        }


        [Test]
        public void Test_DeleteNone()
        {
            CriteriaOperator criteria = CriteriaOperator.Parse("1=0");
            using (UnitOfWork uow = new UnitOfWork(DefaultDataLayer))
            {
                uow.Delete<MySimpleObject>(criteria);
                Assert.AreEqual(1000, uow.GetObjectCount<MySimpleObject>());
            }
        }
    }
}
