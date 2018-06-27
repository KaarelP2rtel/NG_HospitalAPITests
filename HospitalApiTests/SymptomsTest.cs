using NUnit.Framework;
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
            var resultDTO = await result.Content.ReadAsAsync<SymptomDTO>();

            //Proper Status Code
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

            //Returns object
            Assert.IsNotNull(resultDTO);

            Assert.IsTrue(resultDTO.HasAllFields());

        }
        [Test]
        public async Task TestGetSymptoms()
        {


            var result = await client.GetAsync(symptomsRoute);
            var resultContent = await result.Content.ReadAsAsync<List<SymptomDTO>>();

            //Proper Status Code
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

            //Returns object
            Assert.IsNotNull(result);

            var symptom = resultContent.FirstOrDefault();
            Assert.IsTrue(symptom.HasAllFields());



        }

        [Test]
        public async Task TestGetSymptomsWithDiseases()
        {


            var result = await client.GetAsync(symptomsWithDiseasesRoute);
            var resultContent = await result.Content.ReadAsAsync<List<SymptomDTO>>();

            //Proper Status Code
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

            //Returns object
            Assert.IsNotNull(result);

            foreach (var symptom in resultContent)
            {
                Assert.IsTrue(symptom.HasAllFields());
                if ((symptom.Diseases?.Count ?? 0) > 0)
                {
                    Assert.IsTrue(symptom.Diseases.TrueForAll(d => d.HasAllFields()));
                }
            }




        }

        [Test]
        public async Task TestPostMalformedSymptom()
        {

            #region Malformed symptoms
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
            #endregion

            #region Test all malformed symptoms
            foreach (var symptom in testList)
            {


                var result = await client.PostAsJsonAsync(symptomsRoute, symptom);

                //Proper Status Code
                Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode, $"This was accepted: {symptom}");



            }
            #endregion

        }




    }
}