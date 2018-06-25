using NUnit.Framework;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Tests
{
    public class Tests
    {
        #region Variables
        private static string baseUri = "http://localhost:3000/api";
        private static string diseases = "/diseases";
        private static string greatestDiseases = $"{diseases}/greatest";
        private static string symptoms = "/symptoms";
        private static string greatestSymptoms = $"{symptoms}/greatest";
        private static string symptomsCount = $"{symptoms}/count";
        private static string findDiseases = $"{diseases}/find";
        private static string upload = "upload";
        private HttpClient client;

        #endregion


        [OneTimeSetUp]
        public async Task InitalSetup()
        {
            client = new HttpClient {
                BaseAddress = new Uri(baseUri),                
            };

            var result = await client.GetAsync("");

            Assert.IsTrue(result.IsSuccessStatusCode);
            Console.WriteLine(result.Content);
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
        [Test]
        public void Test2()
        {
            Assert.Pass();
        }

    }
}