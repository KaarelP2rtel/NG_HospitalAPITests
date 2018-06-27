using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            #region Get the count of all symptoms
            var request = await client.GetAsync(symptomsRoute);
            var count = (await request.Content.ReadAsAsync<List<SymptomDTO>>()).Count;
            #endregion

            #region Get Count from api and compare results
            request = await client.GetAsync(symptomsCountRoute);
            var apiCount = (await request.Content.ReadAsAsync<CountDTO>()).Count;

            Assert.AreEqual(count, apiCount);
            Assert.IsTrue(request.IsSuccessStatusCode);
            #endregion

        }

        [Test]
        public async Task TestGreatestDiseases()
        {
            #region Find diseases and calculate the top 3

            var result = await client.GetAsync(diseasesRoute);
            var diseases = await result.Content.ReadAsAsync<List<DiseaseDTO>>();

            var calclulatedGreatestDiseases = diseases.OrderBy(d => d.Symptoms.Count).ThenBy(d => d.Name).Take(3).ToList();
            #endregion

            #region Find Top 3 and compare to calculated

            result = await client.GetAsync(greatestDiseasesRoute);
            var apiGreatestDiseases = await result.Content.ReadAsAsync<List<DiseaseDTO>>();

            Assert.AreEqual(apiGreatestDiseases.Count, 3);
            Assert.AreEqual(apiGreatestDiseases.Count, calclulatedGreatestDiseases.Count);

            for (int i = 0; i < apiGreatestDiseases.Count; i++)
            {
                var apiDisease = apiGreatestDiseases.ElementAt(i);
                var calculatedDisease = calclulatedGreatestDiseases.ElementAt(i);

                Assert.AreEqual(apiDisease.Id, calculatedDisease.Id);
                Assert.AreEqual(apiDisease.Name, calculatedDisease.Name);
                Assert.IsTrue(apiDisease.HasAllFields());

            }
            #endregion
        }

        [Test]
        public async Task TestGreatestSymptoms()
        {

            #region Find symptoms and calculate the top 3

            var result = await client.GetAsync(symptomsWithDiseasesRoute);
            var diseases = await result.Content.ReadAsAsync<List<SymptomDTO>>();

            var calclulatedGreatestSymptoms = diseases.OrderByDescending(s => s.Diseases?.Count ?? 0).ThenBy(s => s.Name).Take(3).ToList();
            #endregion

            #region Find Top 3 and compare to calculated

            result = await client.GetAsync(greatestSymptomsRoute);
            var apiGreatestDiseases = await result.Content.ReadAsAsync<List<SymptomDTO>>();

            Assert.AreEqual(apiGreatestDiseases.Count, 3);
            Assert.AreEqual(apiGreatestDiseases.Count, calclulatedGreatestSymptoms.Count);

            for (int i = 0; i < apiGreatestDiseases.Count; i++)
            {
                var apiSymptom = apiGreatestDiseases.ElementAt(i);
                var calculatedSymptom = calclulatedGreatestSymptoms.ElementAt(i);

                Assert.AreEqual(apiSymptom.Id, calculatedSymptom.Id);
                Assert.AreEqual(apiSymptom.Name, calculatedSymptom.Name);

                Assert.IsTrue(apiSymptom.HasAllFields());

                
            }
            #endregion
        }

        [Test]
        public async Task TestFindSingleDisease()
        {
            #region Create and add some new unique symptoms.
            var r = new Random();
            int count = r.Next(7);
            var symptoms = new List<SymptomDTO>();
            for (int i = 0; i < count; i++)
            {

                symptoms.Add(await NewUniqueSymptom());
            }
            #endregion

            #region Create and add new unique disease with symptoms
            var disease = await NewUniqueDisease(symptoms);


            #endregion

            #region Find Disease with symptoms and compare

            var result = await client.PostAsJsonAsync(findDiseasesRoute, symptoms);
            var foundDiseases = await result.Content.ReadAsAsync<List<DiseaseDTO>>();

            Assert.AreEqual(foundDiseases.Count, 1);

            var foundDisease = foundDiseases.FirstOrDefault();

            Assert.IsTrue(foundDisease.HasAllFields());
            Assert.IsTrue(foundDisease.Symptoms.TrueForAll(s => s.HasAllFields()));

            Assert.AreEqual(disease.Id, foundDisease.Id);
            Assert.AreEqual(disease.Name, foundDisease.Name);

            #endregion

        }
        [Test]
        public async Task TestFindWithoutId()
        {

            var list = new List<SymptomDTO>
            {
                new SymptomDTO
                {
                    Name="asd"
                },
                new SymptomDTO
                {
                    Id=1,
                }
            };
            var result = await client.PostAsJsonAsync(findDiseasesRoute, list);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.BadRequest);

        }

        [Test]
        public async Task TestFindManyDiseases()
        {
            #region Create and add 3 new unique symptoms.
            var symptom1 = await NewUniqueSymptom();
            var symptom2 = await NewUniqueSymptom();
            var symptom3 = await NewUniqueSymptom();
            #endregion

            #region 2 Lists of symptoms

            var symptomsSet1 = new List<SymptomDTO>
            {
                symptom1,
                symptom2,
                symptom3
            };
            var symptomsSet2 = new List<SymptomDTO>
            {
                symptom1,
                symptom2

            };


            #endregion

            #region Create and add two new diseases with symptoms
            var disease1 = await NewUniqueDisease(symptomsSet1);
            var disease2 = await NewUniqueDisease(symptomsSet2);
            #endregion

            #region Post testset and compare results

            var testSet1 = new List<SymptomDTO>
            {
                symptom2
            };

            var result = await client.PostAsJsonAsync(findDiseasesRoute, testSet1);
            var diseases = await result.Content.ReadAsAsync<List<DiseaseDTO>>();
            var firstDisease = diseases.First();
            var secondDisease = diseases.ElementAt(1);

            Assert.AreEqual(diseases.Count, 2);

            Assert.IsTrue(firstDisease.HasAllFields());
            Assert.IsTrue(firstDisease.Symptoms.TrueForAll(s => s.HasAllFields()));
            Assert.AreEqual(firstDisease.Id, disease2.Id);

            Assert.IsTrue(secondDisease.HasAllFields());
            Assert.IsTrue(secondDisease.Symptoms.TrueForAll(s => s.HasAllFields()));
            Assert.AreEqual(secondDisease.Id, disease1.Id);

            #endregion

            #region Post testset2 and compare results

            var testSet2 = new List<SymptomDTO>
            {
                symptom2,
                symptom3
            };

            result = await client.PostAsJsonAsync(findDiseasesRoute, testSet2);
            diseases = await result.Content.ReadAsAsync<List<DiseaseDTO>>();
            firstDisease = diseases.First();


            Assert.AreEqual(1,diseases.Count);

            Assert.IsTrue(firstDisease.HasAllFields());
            Assert.IsTrue(firstDisease.Symptoms.TrueForAll(s => s.HasAllFields()));
            Assert.AreEqual(firstDisease.Id, disease1.Id);

            #endregion


        }

    }
    public class CountDTO
    {
        public int Count { get; set; }
    }
}
