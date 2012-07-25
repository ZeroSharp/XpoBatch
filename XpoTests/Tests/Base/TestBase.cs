using System;
using System.Data.SqlClient;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using NUnit.Framework;
using XpoBatch;

namespace XpoTests
{
    public enum TestConnection
    {
        SqlExpress,
        Sql,
        InMemory
    }

    public class TestBase
    {
        public TestConnection TestConnection
        {
            get
            {
                // Change this to test against a different data connection
                return TestConnection.InMemory;
            }
        }       

        [SetUp]
        public void Setup()
        {
            string connectionString;
            switch (TestConnection)
            {
                case TestConnection.SqlExpress:
                    connectionString = @"Data Source=(local)\SQLEXPRESS;Initial Catalog=XpoTests;Integrated Security=SSPI;Pooling=False";
                    DefaultDataLayer = new SimpleDataLayer(new MSSqlConnectionProvider(new SqlConnection(connectionString), AutoCreateOption.DatabaseAndSchema));                   
                    break;
                case TestConnection.Sql:
                    connectionString = @"Data Source=(local);Initial Catalog=XpoTests;Integrated Security=SSPI;Pooling=False";
                    DefaultDataLayer = new SimpleDataLayer(new MSSqlConnectionProvider(new SqlConnection(connectionString), AutoCreateOption.DatabaseAndSchema));                                       
                    break;
                case TestConnection.InMemory:
                    DefaultDataLayer = new SimpleDataLayer(new InMemoryDataStore(AutoCreateOption.DatabaseAndSchema));
                    break;
            }
            
            InsertReferenceDataIfNecessary(100);           
            InsertRecords<MySimpleObject>(1000);
        }

        [TearDown]
        public void TearDown()
        {
            DeleteRecords<MySimpleObject>();
        }        

        public SimpleDataLayer DefaultDataLayer { get; set; }

        private void InsertReferenceDataIfNecessary(int count)
        {
            using (UnitOfWork uow = new UnitOfWork(DefaultDataLayer))
            {
                if (uow.GetObjectCount<MyReferenceObject>() < 100)
                {
                    for (int i = 0; i < count; i++)
                    {
                        MyReferenceObject myReferenceObject = new MyReferenceObject(uow);
                        myReferenceObject.RandomizeFields(null);
                        myReferenceObject.Save();
                    }
                    uow.CommitChanges();
                }
            }
        }

        public void InsertRecords<T>(int count) where T : MyObjectBase
        {
            using (UnitOfWork uow = new UnitOfWork(DefaultDataLayer))
            {
                ReferenceDataProvider referenceDataProvider = new ReferenceDataProvider(uow);
                for (int i = 0; i < count; i++)
                {
                    T myObject = (T)Activator.CreateInstance(typeof(T), new object[1] { uow });
                    myObject.RandomizeFields(referenceDataProvider);
                    myObject.Save();
                }
                uow.CommitChanges();
            }
        }

        public void DeleteRecords<T>()
        {
            using (UnitOfWork uow = new UnitOfWork(DefaultDataLayer))
            {
                XPCollection<T> myObjects = new XPCollection<T>(uow);
                uow.Delete(myObjects);
                uow.CommitChanges();
            }
        }
    }
}
