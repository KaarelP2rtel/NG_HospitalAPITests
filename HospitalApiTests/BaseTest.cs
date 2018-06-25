using NUnit.Framework;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Formatting;
using Newtonsoft.Json;

namespace HospitalApiTests
{
    public class BaseTest
    {
        #region Variables
        public static string baseUri = "https://localhost:44379/api";
        public static string diseasesRoute = "/diseases";
        public static string greatestDiseasesRoute = $"{diseasesRoute}/greatest";
        public static string symptomsRoute = "/symptoms";
        public static string greatestSymptomsRoute = $"{symptomsRoute}/greatest";
        public static string symptomsCountRoute = $"{symptomsRoute}/count";
        public static string findDiseasesRoute = $"{diseasesRoute}/find";
        public static string uploadRoute = "upload";
        public HttpClient client;

        #endregion


        [OneTimeSetUp]
        public async Task InitalSetup()
        {
            client = new HttpClient {
                BaseAddress = new Uri(baseUri),                
            };

            var result = await client.GetAsync("");
            var feedbackDTO = result.Content.ReadAsAsync<FeedbackDTO>();

            Assume.That(result.IsSuccessStatusCode, "Api is not up");



        }

    }
    public class FeedbackDTO
    {
        public String Result { get; set; }
    }
    
    
}