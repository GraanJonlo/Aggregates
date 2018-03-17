using System;
using DapperDb;
using Domain;
using Domain.Entities;
using Domain.Values;
using Xunit;

namespace DapperTests
{
    public class Integration
    {
        //[Fact]
        public void CreateAndUpdate()
        {
            var id = Guid.NewGuid();
            var address = new Address("Faux House", "Imaginary Street", "Scum on the Wold", "Widgetshire", "AB12 3CD");

            using (IUnitOfWork uow = new UnitOfWork(""))
            {
                var l = Landlord.Create(id, new Name("Bob", "Rocket"), "bob.rocket@email.com", address);
                uow.LandlordRepository.Save(l, 0);
                uow.Commit();
            }

            using (IUnitOfWork uow = new UnitOfWork(""))
            {
                var l = uow.LandlordRepository.GetById(id);
                l.ChangeName(new Name("Peter", "Crabkin"));
                uow.LandlordRepository.Save(l, 1);
                uow.Commit();
            }
        }
    }
}