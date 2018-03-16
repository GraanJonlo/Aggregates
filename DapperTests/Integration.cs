using System;
using DapperDb;
using Domain.Entities;
using Xunit;

namespace DapperTests
{
    public class Integration
    {
        //[Fact]
        public void CreateAndUpdate()
        {
            var id = Guid.NewGuid();

            using (var uow = new UnitOfWork(""))
            {
                var l = new Landlord(id, "Bob");
                uow.LandlordRepository.Save(l, 0);
                uow.Commit();
            }

            using (var uow = new UnitOfWork(""))
            {
                var l = uow.LandlordRepository.GetById(id);
                l.ChangeName("Harry");
                uow.LandlordRepository.Save(l, 1);
                uow.Commit();
            }
        }
    }
}
