﻿using NUnit.Framework;
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


            var result = await client.PostAsJsonAsync<SymptomDTO>(symptomsRoute, symptom);
            var resultDTO = await result.Content.ReadAsAsync<SymptomDTO>();

            //Proper Status Code
            Assert.Equals(result.StatusCode, HttpStatusCode.Created);

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
            Assert.Equals(result.StatusCode, HttpStatusCode.Created);

            //Returns object
            Assert.IsNotNull(result);

            var symptom = resultContent.First();
            Assert.NotNull(symptom.Id);
            Assert.NotNull(symptom.Name);
            


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
                //Empty symptom
                new SymptomDTO
                {

                },
            };
            foreach(var symptom in testList) { 


            var result = await client.PostAsJsonAsync(symptomsRoute, symptom);
            var resultDTO = await result.Content.ReadAsAsync<SymptomDTO>();

            //Proper Status Code
            Assert.Equals(result.StatusCode, HttpStatusCode.BadRequest);

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
        public int? Id { get; set; }
        public String Name { get; set; }
    }
    public class DiseaseDTO
    {
        public int? Id { get; set; }
        public String Name { get; set; }
        public List<SymptomDTO> Symptoms { get; set; }
    }
}