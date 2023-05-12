using Bme.Aut.Logistics.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bme.Aut.Logistics.Test;

[TestClass]
public class F1AddressesTests
{
    private static readonly Address[] TestAddresses = new[]
    {
        new Address(47.29, 19.04, "HU", "Budapest", "1111", "street1", "2"),
        new Address(47.30, 19.06, "HU", "Eger", "5559", "Some street", "2/2/2/3"),
        new Address(47.31, 19.05, "HU", "BUDAPEST", "1111", "street1", "2")
    };

    [TestMethod]
    public async Task F1PostNewWithInvalidContent()
    {
        using (var testScope = TestWebAppFactory.Create())
        {
            var client = testScope.CreateClient();
            var response = await client.PostAsJsonAsync("/addresses", new Milestone());

            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }
    }

    [TestMethod]
    public async Task F1PostNewWithMissingProperties()
    {
        using (var testScope = TestWebAppFactory.Create())
        {
            var client = testScope.CreateClient();
            var response = await client.PostAsJsonAsync("/addresses", new Address() { City = "Budapest" });

            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }
    }

    class IdResponse
    {
        public int Id { get; set; }
    }

    [TestMethod]
    public async Task F1PostNewWithSuccess()
    {
        using (var testScope = TestWebAppFactory.Create())
        {
            var toInsert = new Address(47.29, 19.04, "HU", "Budapest", "1111", "street1", "2");

            var client = testScope.CreateClient();
            var response = await client.PostAsJsonAsync("/addresses", toInsert);

            response.EnsureSuccessStatusCode();
            var postResponse = await response.Content.ReadAsAsync<IdResponse>();

            Assert.IsNotNull(postResponse);
            var insertedRecord = testScope.GetDbTableContent<Address>().SingleOrDefault(x => x.Id == postResponse.Id);
            Assert.IsNotNull(insertedRecord);
            Assert.AreEqual(toInsert.GeoLatitude, insertedRecord.GeoLatitude);
            Assert.AreEqual(toInsert.City, insertedRecord.City);
            Assert.AreEqual(toInsert.Number, insertedRecord.Number);
        }
    }

    [TestMethod]
    public async Task F1AddressesListAll()
    {
        using (var testScope = TestWebAppFactory.Create())
        {
            testScope.AddSeedEntities(TestAddresses);

            var client = testScope.CreateClient();
            var response = await client.GetAsync("/addresses");

            response.EnsureSuccessStatusCode();
            var actual = await response.Content.ReadAsAsync<Address[]>();

            Assert.IsNotNull(actual);
            CollectionAssert.AreEquivalent(TestAddresses, actual);
        }
    }

    [TestMethod]
    public async Task F1AddressesListAllWhenNone()
    {
        using (var testScope = TestWebAppFactory.Create())
        {
            var client = testScope.CreateClient();
            var response = await client.GetAsync("/addresses");

            response.EnsureSuccessStatusCode();
            var actual = await response.Content.ReadAsAsync<Address[]>();

            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.Length);
        }
    }

    [TestMethod]
    public async Task F1GetOneSuccess()
    {
        using (var testScope = TestWebAppFactory.Create())
        {
            testScope.AddSeedEntities(TestAddresses);
            var client = testScope.CreateClient();

            foreach (var expected in testScope.GetDbTableContent<Address>())
            {
                var response = await client.GetAsync($"/addresses/{expected.Id}");

                response.EnsureSuccessStatusCode();
                var actual = await response.Content.ReadAsAsync<Address>();

                Assert.IsNotNull(actual);
                Assert.AreEqual(expected, actual);
            }
        }
    }

    [TestMethod]
    public async Task F1GetOneDoesNotExist()
    {
        using (var testScope = TestWebAppFactory.Create())
        {
            testScope.AddSeedEntities(TestAddresses);
            var client = testScope.CreateClient();

            var response = await client.GetAsync($"/addresses/{new Random().Next(100, 999999999)}");

            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }
    }

    [TestMethod]
    public async Task F1DeleteOneWithSuccess()
    {
        using (var testScope = TestWebAppFactory.Create())
        {
            testScope.AddSeedEntities(TestAddresses);
            var client = testScope.CreateClient();

            var recordToDelete = testScope.GetDbTableContent<Address>().Last();

            var response = await client.DeleteAsync($"/addresses/{recordToDelete.Id}");

            response.EnsureSuccessStatusCode();
            Assert.IsNull(testScope.GetDbTableContent<Address>().FirstOrDefault(x => x.Id == recordToDelete.Id));
        }
    }

    [TestMethod]
    public async Task F1DeleteDoesNotExist()
    {
        using (var testScope = TestWebAppFactory.Create())
        {
            testScope.AddSeedEntities(TestAddresses);
            var client = testScope.CreateClient();

            var response = await client.DeleteAsync($"/addresses/{new Random().Next(100, 999999999)}");

            response.EnsureSuccessStatusCode();
        }
    }

    [TestMethod]
    public async Task F1PutDoesNotExist()
    {
        using (var testScope = TestWebAppFactory.Create())
        {
            testScope.AddSeedEntities(TestAddresses);
            var client = testScope.CreateClient();

            var response = await client.PutAsJsonAsync("/addresses/1234567", new Address(47.31, 19.05, "HU", "BUDAPEST", "1111", "street1", "2") { Id = 1234567 });

            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }
    }

    [TestMethod]
    public async Task F1PutWithMissingProperties()
    {
        using (var testScope = TestWebAppFactory.Create())
        {
            testScope.AddSeedEntities(TestAddresses);
            var client = testScope.CreateClient();
            var recordToUpdate = testScope.GetDbTableContent<Address>().Last();

            var response = await client.PutAsJsonAsync($"/addresses/{recordToUpdate.Id}", new Address() { City = "Budapest" });

            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }
    }

    [TestMethod]
    public async Task F1PutWithSuccess()
    {
        using (var testScope = TestWebAppFactory.Create())
        {
            testScope.AddSeedEntities(TestAddresses);

            var client = testScope.CreateClient();
            var recordToUpdate = testScope.GetDbTableContent<Address>().Last();
            recordToUpdate.City = "Some new city";
            recordToUpdate.Street = "Some new street";

            var response = await client.PutAsJsonAsync($"/addresses/{recordToUpdate.Id}", recordToUpdate);

            response.EnsureSuccessStatusCode();
            var putResponse = await response.Content.ReadAsAsync<Address>();

            Assert.IsNotNull(putResponse);
            Assert.AreEqual(recordToUpdate.Id, putResponse.Id);
            Assert.AreEqual(recordToUpdate.City, putResponse.City);
            Assert.AreEqual(recordToUpdate.Street, putResponse.Street);

            var updatedRecord = testScope.GetDbTableContent<Address>().SingleOrDefault(x => x.Id == recordToUpdate.Id);
            Assert.IsNotNull(updatedRecord);
            Assert.AreEqual(recordToUpdate.Id, updatedRecord.Id);
            Assert.AreEqual(recordToUpdate.City, updatedRecord.City);
            Assert.AreEqual(recordToUpdate.Street, updatedRecord.Street);
        }
    }
}
