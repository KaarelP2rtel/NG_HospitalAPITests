using NUnit.Framework;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Formatting;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace HospitalApiTests
{
    public class BaseTest
    {
        #region Variables
        public static string baseUri = "https://localhost:44383/api/";
        public static string diseasesRoute = "diseases";
        public static string greatestDiseasesRoute = $"{diseasesRoute}/greatest";
        public static string symptomsRoute = "symptoms";
        public static string greatestSymptomsRoute = $"{symptomsRoute}/greatest";
        public static string symptomsCountRoute = $"{symptomsRoute}/count";
        public static string findDiseasesRoute = $"{diseasesRoute}/find";
        public static string uploadRoute = "upload";
        public static string symptomsWithDiseasesRoute = $"{symptomsRoute}/withDiseases";
        public static string clearRoute = "clear";
        public HttpClient client;

        #endregion


        [OneTimeSetUp]
        public async Task InitalSetup()
        {
            client = new HttpClient
            {
                BaseAddress = new Uri(baseUri),
            };

            var result = await client.GetAsync("");
            Assume.That(result.IsSuccessStatusCode, "Api is not up");




        }

        public async Task<SymptomDTO> NewUniqueSymptom()
        {
            Random r = new Random();
            return await(await client.PostAsJsonAsync(symptomsRoute, new SymptomDTO { Name = $"UniqueSymptom{r.Next(int.MaxValue)}" }))
               .Content
               .ReadAsAsync<SymptomDTO>();
        }

        public async Task<DiseaseDTO> NewUniqueDisease(List<SymptomDTO> symptoms)
        {
            Random r = new Random();
            return await(await client.PostAsJsonAsync(diseasesRoute, new DiseaseDTO { Name = $"UniqueDisease{r.Next(int.MaxValue)}", Symptoms=symptoms }))
               .Content
               .ReadAsAsync<DiseaseDTO>();
        }




    }
}