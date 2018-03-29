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
            const string connectionString = "";
            var id = Guid.NewGuid();
            var address = new Address("Faux House", "Imaginary Street", "Scum on the Wold", "Widgetshire", "AB12 3CD");

            using (IUnitOfWork u = new UnitOfWork(new SqlConnection(connectionString)))
            {
                var eventStore = new SqlEventStore(new DictionaryBackedEventPublisher(), u);
                var landlordRepository = new LandlordRepository(eventStore);
                var l = Landlord.Create(id, new Name("Bob", "Rocket"), "bob.rocket@email.com", address);
                landlordRepository.Save(l, 0);
                u.Commit();
            }

            using (IUnitOfWork u = new UnitOfWork(new SqlConnection(connectionString)))
            {
                var eventStore = new SqlEventStore(new DictionaryBackedEventPublisher(), u);
                var landlordRepository = new LandlordRepository(eventStore);
                var l = landlordRepository.Get(id);
                l.ChangeName(new Name("Peter", "Crabkin"));
                landlordRepository.Save(l, 1);
                u.Commit();
            }
        }
    }
}