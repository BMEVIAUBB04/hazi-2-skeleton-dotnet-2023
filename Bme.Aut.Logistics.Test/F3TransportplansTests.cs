using Bme.Aut.Logistics.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bme.Aut.Logistics.Test;

[TestClass]
public class F3TransportplansTests
{

    [TestMethod]
    public async Task F3GetAllTransportPlansTest()
    {
        using (var testScope = TestWebAppFactory.Create())
        {
            testScope.AddSeedEntities(TestTransportPlans.ToArray());

            var client = testScope.CreateClient();
            var response = await client.GetAsync("/transportplans?dateTime=2020-01-01T01-01-01");

            response.EnsureSuccessStatusCode();
            var actual = await response.Content.ReadAsAsync<TransportPlan[]>();

            Assert.IsNotNull(actual);
            CollectionAssert.AreEquivalent(TestTransportPlans, actual);
        }
    }

    [TestMethod]
    public async Task F3GetOneTransportPlansTest()
    {
        using (var testScope = TestWebAppFactory.Create())
        {
            testScope.AddSeedEntities(TestTransportPlans.ToArray());

            var expected = new[] { TestTransportPlans[1] };

            var client = testScope.CreateClient();
            var response = await client.GetAsync("/transportplans?dateTime=2020-05-10T01-01-01");

            response.EnsureSuccessStatusCode();
            var actual = await response.Content.ReadAsAsync<TransportPlan[]>();

            Assert.IsNotNull(actual);
            CollectionAssert.AreEquivalent(expected, actual);
        }
    }

    [TestMethod]
    public async Task F3NoDate()
    {
        using (var testScope = TestWebAppFactory.Create())
        {
            var client = testScope.CreateClient();
            var response = await client.GetAsync("/transportplans");

            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.BadRequest);
        }
    }

    [TestMethod]
    public async Task F3BadDateFormat()
    {
        using (var testScope = TestWebAppFactory.Create())
        {
            var client = testScope.CreateClient();
            var response = await client.GetAsync($"/transportplans?dateTime={DateTime.Now}");

            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.BadRequest);
        }
    }


    private static readonly List<TransportPlan> TestTransportPlans = new List<TransportPlan>
    {
        new TransportPlan()
        {
            Sections = new List<Section>
            {
                new Section
                {
                    FromMilestone = new Milestone()
                    {
                        PlannedTime = new DateTime(2020, 5, 1, 13, 0, 0, DateTimeKind.Local),
                        Address = new Address(50, 19, "HU", "Budapest", "1111", "Vaci utca", "77"),
                    },
                    ToMilestone = new Milestone()
                    {
                        PlannedTime = new DateTime(2020, 5, 9, 13, 0, 0, DateTimeKind.Local),
                        Address = new Address(50, 19, "HU", "Budapest", "1111", "Vaci utca", "77"),
                    }
                }
             }
        },
        new TransportPlan()
        {
            Sections = new List<Section>
            {
                new Section
                {
                    FromMilestone = new Milestone()
                    {
                        PlannedTime = new DateTime(2020, 5, 1, 13, 0, 0, DateTimeKind.Local),
                        Address = new Address(50, 19, "HU", "Budapest", "1111", "Vaci utca", "77"),
                    },
                    ToMilestone = new Milestone()
                    {
                        PlannedTime = new DateTime(2020, 5, 8, 13, 0, 0, DateTimeKind.Local),
                        Address = new Address(50, 19, "HU", "Budapest", "1111", "Vaci utca", "77"),
                    }
                },
                new Section
                {
                    FromMilestone = new Milestone()
                    {
                        PlannedTime = new DateTime(2020, 5, 10, 13, 0, 0, DateTimeKind.Local),
                        Address = new Address(50, 19, "HU", "Budapest", "1111", "Vaci utca", "77"),
                    },
                    ToMilestone = new Milestone()
                    {
                        PlannedTime = new DateTime(2020, 5, 10, 13, 0, 0, DateTimeKind.Local),
                        Address = new Address(50, 19, "HU", "Budapest", "1111", "Vaci utca", "77"),
                    }
                }
             }
        },
        new TransportPlan()
        {
            Sections = new List<Section>
            {
                new Section
                {
                    FromMilestone = new Milestone()
                    {
                        PlannedTime = new DateTime(2020, 2, 3, 13, 0, 0, DateTimeKind.Local),
                        Address = new Address(50, 19, "HU", "Budapest", "1111", "Vaci utca", "77"),
                    },
                    ToMilestone = new Milestone()
                    {
                        PlannedTime = new DateTime(2020, 2, 8, 13, 0, 0, DateTimeKind.Local),
                        Address = new Address(50, 19, "HU", "Budapest", "1111", "Vaci utca", "77"),
                    }
                },
                new Section
                {
                    FromMilestone = new Milestone()
                    {
                        PlannedTime = new DateTime(2020, 2, 10, 13, 0, 0, DateTimeKind.Local),
                        Address = new Address(50, 19, "HU", "Budapest", "1111", "Vaci utca", "77"),
                    },
                    ToMilestone = new Milestone()
                    {
                        PlannedTime = new DateTime(2020, 2, 14, 13, 0, 0, DateTimeKind.Local),
                        Address = new Address(50, 19, "HU", "Budapest", "1111", "Vaci utca", "77"),
                    }
                }
             }
        }
    };
}
