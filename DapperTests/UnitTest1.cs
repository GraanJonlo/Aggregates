using System;
using DapperDb;
using Domain.Entities;
using Xunit;

namespace DapperTests
{
    public class UnitTest1
    {
        [Fact(Skip = "Actual thing")]
        //[Fact]
        public void Test1()
        {
            using (var uow = new UnitOfWork(""))
            {
                var l = new Landlord(Guid.NewGuid(), "Bob");
                uow.LandlordRepository.Save(l, -1);
                uow.Commit();
            }
            
        }

        [Fact(Skip = "Actual thing")]
        // [Fact]
        public void Test2()
        {
            using (var uow = new UnitOfWork(""))
            {
                var l = uow.LandlordRepository.GetById(Guid.Parse("E9B0E19F-FF76-4F78-B437-2756545BCB8D"));
                l.ChangeName("Harry");
                uow.LandlordRepository.Save(l, 0);
                uow.Commit();
            }
        }
    }
}
