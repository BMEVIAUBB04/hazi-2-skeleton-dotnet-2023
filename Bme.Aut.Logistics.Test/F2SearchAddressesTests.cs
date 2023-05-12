using Bme.Aut.Logistics.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Bme.Aut.Logistics.Test;

[TestClass]
public class F2SearchAddressesTests
{
    private static readonly Address[] TestAddresses = new[]
    {
        new Address(47.29, 19.04, "HU", "Budapest", "1111", "Magyar tudósok körútja", "2"),
        new Address(47.30, 19.06, "HU", "Eger", "5559", "Some street", "2/2/2/3"),
        new Address(47.45, 18.93, "HU", "Budaörs", "2041", "Sport utca", "2"),
        new Address(47.38, 18.92, "HU", "Érd", "2030", "Budai út", "13"),
        new Address(47.63, -122.13, "USA", "Redmond", "98052", "Microsoft Way", "1")
    };

    [TestMethod]
    public async Task F2SearchNoBodyTest()
    {
        using (var testScope = TestWebAppFactory.Create())
        {
            testScope.AddSeedEntities(TestAddresses);
            var client = testScope.CreateClient();

            var content = new StringContent("", Encoding.UTF8, "application/json"); ;
            var response = await client.PostAsync("/addresses/search", content);

            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.BadRequest);
        }
    }


    [TestMethod]
    public async Task F2SearchSingleCityTest()
    {
        using (var testScope = TestWebAppFactory.Create())
        {
            testScope.AddSeedEntities(TestAddresses);

            var searchAddress = new Address() { City = "Budapest" };
            var expected = new Address[] { TestAddresses[0] };

            var client = testScope.CreateClient();
            var response = await client.PostAsJsonAsync("/addresses/search", searchAddress);

            response.EnsureSuccessStatusCode();
            var actual = await response.Content.ReadAsAsync<Address[]>();

            Assert.IsNotNull(actual);
            CollectionAssert.AreEquivalent(expected, actual);
        }
    }

    [TestMethod]
    public async Task F2SearchMultiCityTest()
    {
        using (var testScope = TestWebAppFactory.Create())
        {
            testScope.AddSeedEntities(TestAddresses);

            var searchAddress = new Address() { City = "Buda" };
            var expected = new[] { TestAddresses[0], TestAddresses[2] };

            var client = testScope.CreateClient();
            var response = await client.PostAsJsonAsync("/addresses/search", searchAddress);

            response.EnsureSuccessStatusCode();
            var actual = await response.Content.ReadAsAsync<Address[]>();

            Assert.IsNotNull(actual);
            CollectionAssert.AreEquivalent(expected, actual);
        }
    }

    [TestMethod]
    public async Task F2SearchCountryTest()
    {
        using (var testScope = TestWebAppFactory.Create())
        {
            testScope.AddSeedEntities(TestAddresses);

            var searchAddress = new Address() { Country = "HU", };
            var expected = new[] { TestAddresses[0], TestAddresses[1], TestAddresses[2], TestAddresses[3] };

            var client = testScope.CreateClient();
            var response = await client.PostAsJsonAsync("/addresses/search", searchAddress);

            response.EnsureSuccessStatusCode();
            var actual = await response.Content.ReadAsAsync<Address[]>();

            Assert.IsNotNull(actual);
            CollectionAssert.AreEquivalent(expected, actual);
        }
    }

    [TestMethod]
    public async Task F2SearchStreetTest()
    {
        using (var testScope = TestWebAppFactory.Create())
        {
            testScope.AddSeedEntities(TestAddresses);

            var searchAddress = new Address() { Street = "s" };
            var expected = new[] { TestAddresses[1], TestAddresses[2] };

            var client = testScope.CreateClient();
            var response = await client.PostAsJsonAsync("/addresses/search", searchAddress);

            response.EnsureSuccessStatusCode();
            var actual = await response.Content.ReadAsAsync<Address[]>();

            Assert.IsNotNull(actual);
            CollectionAssert.AreEquivalent(expected, actual);
        }
    }

    [TestMethod]
    public async Task F2SearchAllTest()
    {
        using (var testScope = TestWebAppFactory.Create())
        {
            testScope.AddSeedEntities(TestAddresses);

            var searchAddress = new Address() { City = "bud", Country = "h", Street = "mag" };
            var expected = new[] { TestAddresses[0] };

            var client = testScope.CreateClient();
            var response = await client.PostAsJsonAsync("/addresses/search", searchAddress);

            response.EnsureSuccessStatusCode();
            var actual = await response.Content.ReadAsAsync<Address[]>();

            Assert.IsNotNull(actual);
            CollectionAssert.AreEquivalent(expected, actual);
        }
    }

    [TestMethod]
    public async Task F2SearchNoneTest()
    {
        using (var testScope = TestWebAppFactory.Create())
        {
            testScope.AddSeedEntities(TestAddresses);

            var searchAddress = new Address() { City = "Debrecen" };
            var expected = new Address[] { };

            var client = testScope.CreateClient();
            var response = await client.PostAsJsonAsync("/addresses/search", searchAddress);

            response.EnsureSuccessStatusCode();
            var actual = await response.Content.ReadAsAsync<Address[]>();

            Assert.IsNotNull(actual);
            CollectionAssert.AreEquivalent(expected, actual);
        }
    }

    [TestMethod]
    public async Task F2SearchSearchByUnsupportedPropertiesTest()
    {
        using (var testScope = TestWebAppFactory.Create())
        {
            testScope.AddSeedEntities(TestAddresses);

            var searchAddress = new Address() { GeoLongitude = 47.29, GeoLatitude = 19.04 };

            var client = testScope.CreateClient();
            var response = await client.PostAsJsonAsync("/addresses/search", searchAddress);

            response.EnsureSuccessStatusCode();
            var actual = await response.Content.ReadAsAsync<Address[]>();

            Assert.IsNotNull(actual);
            CollectionAssert.AreEquivalent(TestAddresses, actual);
        }
    }
}
