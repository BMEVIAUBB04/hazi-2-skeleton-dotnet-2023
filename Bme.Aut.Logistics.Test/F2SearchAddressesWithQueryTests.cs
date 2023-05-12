using Bme.Aut.Logistics.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Bme.Aut.Logistics.Test;

[TestClass]
public class F2SearchAddressesWithQueryTests
{
    private static readonly Address[] TestAddresses = new[]
    {
        new Address(47.29, 19.04, "HU", "Budapest", "1111", "Magyar tudósok körútja", "1"),
        new Address(47.29, 19.04, "HU", "Budapest", "1111", "Magyar tudósok körútja", "2"),
        new Address(47.29, 19.04, "HU", "Budapest", "1111", "Magyar tudósok körútja", "3"),
        new Address(47.29, 19.04, "HU", "Budapest", "1111", "Magyar tudósok körútja", "4"),
        new Address(47.29, 19.04, "HU", "Budapest", "1111", "Magyar tudósok körútja", "5"),
        new Address(47.29, 19.04, "HU", "Budapest", "1111", "Magyar tudósok körútja", "6"),
        new Address(47.29, 19.04, "HU", "Budapest", "1111", "Magyar tudósok körútja", "7"),
        new Address(47.29, 19.04, "HU", "Budapest", "1111", "Magyar tudósok körútja", "8"),
        new Address(47.30, 19.06, "HU", "Eger", "5559", "Some street", "2/2/2/3"),
        new Address(47.45, 18.93, "HU", "Budakeszi", "2041", "Sport utca", "2"),
        new Address(47.38, 18.92, "HU", "Érd", "2030", "Budai út", "13"),
        new Address(47.63, -122.13, "USA", "Redmond", "98052", "Microsoft Way", "1")
    };

    [TestMethod]
    public async Task F2SearchWithSizeTest()
    {
        using (var testScope = TestWebAppFactory.Create())
        {
            testScope.AddSeedEntities(TestAddresses);

            var searchAddress = new Address() { City = "Budapest" };
            var expected = new Address[] { TestAddresses[0], TestAddresses[1], TestAddresses[2], TestAddresses[3], TestAddresses[4] };

            var client = testScope.CreateClient();
            var response = await client.PostAsJsonAsync("/addresses/search?size=5", searchAddress);

            response.EnsureSuccessStatusCode();
            var actual = await response.Content.ReadAsAsync<Address[]>();

            Assert.IsNotNull(actual);
            CollectionAssert.AreEquivalent(expected, actual);
        }
    }

    [TestMethod]
    public async Task F2SearchWithSizeAndPageTest()
    {
        using (var testScope = TestWebAppFactory.Create())
        {
            testScope.AddSeedEntities(TestAddresses);

            var searchAddress = new Address() { City = "Budapest" };
            var expected = new Address[] { TestAddresses[3], TestAddresses[4], TestAddresses[5] };

            var client = testScope.CreateClient();
            var response = await client.PostAsJsonAsync("/addresses/search?size=3&page=1", searchAddress);

            response.EnsureSuccessStatusCode();
            var actual = await response.Content.ReadAsAsync<Address[]>();

            Assert.IsNotNull(actual);
            CollectionAssert.AreEquivalent(expected, actual);
        }
    }

    [TestMethod]
    public async Task F2SearchWithSizeAndLastPageTest()
    {
        using (var testScope = TestWebAppFactory.Create())
        {
            testScope.AddSeedEntities(TestAddresses);

            var searchAddress = new Address() { Country = "hu" };
            var expected = new Address[] { TestAddresses[9], TestAddresses[10] };

            var client = testScope.CreateClient();
            var response = await client.PostAsJsonAsync("/addresses/search?size=3&page=3", searchAddress);

            response.EnsureSuccessStatusCode();
            var actual = await response.Content.ReadAsAsync<Address[]>();

            Assert.IsNotNull(actual);
            CollectionAssert.AreEquivalent(expected, actual);
        }
    }

    [TestMethod]
    public async Task F2SearchHeaderTest()
    {
        using (var testScope = TestWebAppFactory.Create())
        {
            testScope.AddSeedEntities(TestAddresses);

            var searchAddress = new Address() { City = "Budapest" };
            var expected = new Address[] { TestAddresses[0], TestAddresses[1], TestAddresses[2], TestAddresses[3], TestAddresses[4] };

            var client = testScope.CreateClient();
            var response = await client.PostAsJsonAsync("/addresses/search?size=5", searchAddress);

            response.EnsureSuccessStatusCode();
            var actual = await response.Content.ReadAsAsync<Address[]>();

            Assert.IsNotNull(actual);
            CollectionAssert.AreEquivalent(expected, actual);
            Assert.AreEqual(TestAddresses.Count(ta => ta.City == "Budapest").ToString(), response.Headers.GetValues("X-Total-Count").FirstOrDefault());
        }
    }

    [TestMethod]
    public async Task F2SearchWithSizeAndPageSortByCityTest()
    {
        using (var testScope = TestWebAppFactory.Create())
        {
            testScope.AddSeedEntities(TestAddresses);
            
            var searchAddress = new Address() { Country = "h" };
            var expected = new Address[] { TestAddresses[9] };
            
            var client = testScope.CreateClient();
            var response = await client.PostAsJsonAsync("/addresses/search?size=1&page=0&sort=city", searchAddress);

            response.EnsureSuccessStatusCode();
            var actual = await response.Content.ReadAsAsync<Address[]>();

            Assert.IsNotNull(actual);
            CollectionAssert.AreEquivalent(expected, actual);
        }
    }

    [TestMethod]
    public async Task F2SearchWithSizeAndPageSortByCityDescendingTest()
    {
        using (var testScope = TestWebAppFactory.Create())
        {
            testScope.AddSeedEntities(TestAddresses);
            var searchAddress = new Address() { Country = "h" };
            var expected = new Address[] { TestAddresses[9] };

            var client = testScope.CreateClient();
            var response = await client.PostAsJsonAsync("/addresses/search?size=5&page=2&sort=city,desc", searchAddress);

            response.EnsureSuccessStatusCode();
            var actual = await response.Content.ReadAsAsync<Address[]>();

            Assert.IsNotNull(actual);
            CollectionAssert.AreEquivalent(expected, actual);
        }
    }

    [TestMethod]
    public async Task F2SearchWithSizeAndPageSortByNumberTest()
    {
        using (var testScope = TestWebAppFactory.Create())
        {
            testScope.AddSeedEntities(TestAddresses);
            var searchAddress = new Address() { Country = "h" };
            var expected = new Address[] { TestAddresses[7] };

            var client = testScope.CreateClient();
            var response = await client.PostAsJsonAsync("/addresses/search?size=5&page=2&sort=number,asc", searchAddress);

            response.EnsureSuccessStatusCode();
            var actual = await response.Content.ReadAsAsync<Address[]>();

            Assert.IsNotNull(actual);
            CollectionAssert.AreEquivalent(expected, actual);
        }
    }

    [TestMethod]
    public async Task F2SearchWithSizeAndPageSortByNumberDescendingTest()
    {
        using (var testScope = TestWebAppFactory.Create())
        {
            testScope.AddSeedEntities(TestAddresses);
            var searchAddress = new Address() { Country = "h" };
            var expected = new Address[] { TestAddresses[0] };

            var client = testScope.CreateClient();
            var response = await client.PostAsJsonAsync("/addresses/search?size=5&page=2&sort=number,desc", searchAddress);

            response.EnsureSuccessStatusCode();
            var actual = await response.Content.ReadAsAsync<Address[]>();

            Assert.IsNotNull(actual);
            CollectionAssert.AreEquivalent(expected, actual);
        }
    }
}

