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
    public class DiseasesTest : BaseTest
    {
        [Test]
        public async Task TestPostDisease()
        {

            var symptoms = await (await client.GetAsync(symptomsRoute)).Content.ReadAsAsync<List<SymptomDTO>>();
            Assume.That((symptoms?.Count ?? 0) >= 3);


            var disease = new DiseaseDTO
            {
                Name = "TestPostDisease",
                Symptoms = new List<SymptomDTO>()

            };
            
            for (int i = 0; i < 3; i++)
            {
                disease.Symptoms.Add(symptoms.ElementAt(i));

            }


            var result = await client.PostAsJsonAsync(diseasesRoute, disease);
            var resultDTO = await result.Content.ReadAsAsync<DiseaseDTO>();

            //Proper Status Code
            Assert.AreEqual(HttpStatusCode.OK,result.StatusCode);

            
            //Returns all fields
            Assert.NotNull(resultDTO.Id);
            Assert.NotNull(resultDTO.Name);
            Assert.NotNull(resultDTO.Symptoms);

            //Returns with symptoms
            var symptom = resultDTO.Symptoms.FirstOrDefault();
            Assert.NotNull(symptom?.Id);
            Assert.NotNull(symptom?.Name);



        }
        [Test]
        public async Task TestGetDiseases()
        {


            var result = await client.GetAsync(diseasesRoute);
            var resultContent = await result.Content.ReadAsAsync<List<DiseaseDTO>>();

            //Proper Status Code
            Assert.AreEqual(HttpStatusCode.OK,result.StatusCode);

            //Returns Something
            Assert.IsNotNull(result);

            //Returns all fields
            var disease = resultContent.FirstOrDefault();
            Assert.IsTrue(disease.HasAllFields());
            Assert.IsTrue(disease.Symptoms.TrueForAll(s => s.HasAllFields()));


        }

        [Test]
        public async Task TestPostMalformedDisease()
        {
            #region Malformed Diseases
            var testList = new List<DiseaseDTO>
            {
                //Empty Disease
                new DiseaseDTO
                {

                },
                //Disease with ID
                new DiseaseDTO
                {
                    Id=11,
                    Name="asd"
                },
                //No symptoms
                new DiseaseDTO
                {
                    Name="asd"
                },
                //No symptoms
                new DiseaseDTO
                {
                    Id=11,
                    Name="asd",
                    Symptoms=new List<SymptomDTO>()

                },
                //Malformed symptom
                new DiseaseDTO
                {

                    Name="asd",
                    Symptoms=new List<SymptomDTO>
                    {
                        new SymptomDTO
                        {
                            Name="asd"
                        }
                    }

                },


            }; 
            #endregion

            foreach (var disease in testList)
            {

                var result = await client.PostAsJsonAsync(diseasesRoute, disease);
                



                //Proper Status Code
                Assert.AreEqual(HttpStatusCode.BadRequest,result.StatusCode,$"This was accepted: {disease}");
                

             
            }
        }

    }


}



