using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HospitalApiTests
{
    [TestFixture]
    public class SymptomsTest : BaseTest
    {
        [Test]
        public async Task TestPostSymptom()
        {

            var symptom = new SymptomDTO
            {
                Name = "TestPostSymptom"
            };


            var result = await client.PostAsJsonAsync(symptomsRoute, symptom);
            TestContext.WriteLine(await result.Content.ReadAsStringAsync());
            var resultDTO = await result.Content.ReadAsAsync<SymptomDTO>();
            
            //Proper Status Code
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

            //Returns object
            Assert.IsNotNull(resultDTO);

            //Returns ID and Name
            Assert.IsNotNull(resultDTO.Id);
            Assert.IsNotNull(resultDTO.Name);

        }
        [Test]
        public async Task TestGetSymptoms()
        {


            var result = await client.GetAsync(symptomsRoute);
            var resultContent = await result.Content.ReadAsAsync<List<SymptomDTO>>();

            //Proper Status Code
            Assert.AreEqual(HttpStatusCode.OK,result.StatusCode);

            //Returns object
            Assert.IsNotNull(result);

            var symptom = resultContent.FirstOrDefault();
            Assert.NotNull(symptom?.Id);
            Assert.NotNull(symptom?.Name);
            


        }

        [Test]
        public async Task TestPostMalformedSymptom()
        {

            var testList = new List<SymptomDTO>
            {
                //Empty symptom
                new SymptomDTO
                {

                },
                //Contains Id
                new SymptomDTO
                {
                    Id=1,
                    Name="asd",
                },
               
            };
            foreach(var symptom in testList) { 


            var result = await client.PostAsJsonAsync(symptomsRoute, symptom);
            var resultDTO = await result.Content.ReadAsAsync<SymptomDTO>();

            //Proper Status Code
            Assert.AreEqual(HttpStatusCode.BadRequest,result.StatusCode);
            

            //Does not return result
            Assert.IsNull(resultDTO);

            //Does contain feedback
            var feedback = await result.Content.ReadAsAsync<FeedbackDTO>();
            Assert.IsNotNull(feedback?.Result);
            }

        }


        

    }
    public class SymptomDTO
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Id { get; set; }
        public String Name { get; set; }
    }
    public class DiseaseDTO
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Id { get; set; }
        public String Name { get; set; }
        public List<SymptomDTO> Symptoms { get; set; }
    }
}