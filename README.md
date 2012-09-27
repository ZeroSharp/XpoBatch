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

Requires [DevExpress XPO](http://devexpress.com/Products/NET/ORM/). This currently uses version 12.1.7, but it works with 11.2 and possibly earlier versions with minor changes that are documented in the code.

Includes NUnit 2.6 NuGet package.