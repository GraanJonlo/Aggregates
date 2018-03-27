using System;
using System.Data.SqlClient;
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
            var connectionString = "";
            var id = Guid.NewGuid();
            var address = new Address("Faux House", "Imaginary Street", "Scum on the Wold", "Widgetshire", "AB12 3CD");

            using (IUnitOfWork u = new UnitOfWork(new DictionaryBackedEventPublisher(), new SqlConnection(connectionString)))
            {
                var l = Landlord.Create(id, new Name("Bob", "Rocket"), "bob.rocket@email.com", address);
                u.LandlordRepository.Save(l, 0);
                u.Commit();
            }

            using (IUnitOfWork u = new UnitOfWork(new DictionaryBackedEventPublisher(), new SqlConnection(connectionString)))
            {
                var l = u.LandlordRepository.GetById(id);
                l.ChangeName(new Name("Peter", "Crabkin"));
                u.LandlordRepository.Save(l, 1);
                u.Commit();
            }
        }
    }
}