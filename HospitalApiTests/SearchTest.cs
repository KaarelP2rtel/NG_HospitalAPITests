using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HospitalApiTests
{
    [TestFixture]
    public class SearchTest : BaseTest
    {
        [Test]
        public async Task TestSymptomsCount()
        {

            var request = await client.GetAsync(symptomsCountRoute);
            Assert.IsTrue(request.IsSuccessStatusCode);

            var count = (await request.Content.ReadAsAsync<CountDTO>()).Count;
            await client.PostAsJsonAsync<SymptomDTO>(symptomsRoute, new SymptomDTO { Name = "NewSymptomForCount" });

            request = await client.GetAsync(symptomsCountRoute);
            Assert.IsTrue(request.IsSuccessStatusCode);
            var newCount = (await request.Content.ReadAsAsync<CountDTO>()).Count;
            Assert.Greater(newCount, count);

        }
    }
    public class CountDTO
    {
        public int Count { get; set; }
    }
}
