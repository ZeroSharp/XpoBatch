## Fast batch modifications and deletions for DevExpress XPO ##

A set of extension methods which makes it easy to execute batch wide deletions and modifications with strong typing and using criteria. Note the operation bypasses all the business logic and locking. See the related blog posts for more information. Includes unit tests.

* [Fast batch deletions with DevExpress XPO](http://blog.zerosharp.com/fast-batch-deletions-with-devexpress-xpo/)
* [Fast batch modifications with DevExpress XPO](http://blog.zerosharp.com/fast-batch-modifications-with-devexpress-xpo/)

### Fast deletions ###

```csharp
using (UnitOfWork uow = new UnitOfWork())
{
    uow.Delete<MyObject>(CriteriaOperator.Parse("City != 'Chicago'"));
}
```

### Fast modifications ###

```csharp
using (UnitOfWork uow = new UnitOfWork())
{
	uow.Update<MyObject>(
        () => new MyObject(uow) 
                  { 
                     State = "CA", 
                     CostCenter = 123 
                  }, 
        CriteriaOperator.Parse("City == 'San Francisco'"));
}
```

### Dependencies ###

Requires [DevExpress XPO](http://devexpress.com/Products/NET/ORM/). This currently uses version 24.2.6, but it works with versions as old as 11.2.

Includes NUnit 2.7.1 NuGet package.