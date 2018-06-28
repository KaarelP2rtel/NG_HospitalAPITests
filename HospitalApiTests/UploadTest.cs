using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HospitalApiTests
{
    [TestFixture]
    public class UploadTest : BaseTest
    {
        [Test]
        public async Task TestUpload()
        {
            var data = Encoding.UTF8.GetBytes(Resources.Datasets.database);
            var form = new MultipartFormDataContent();
            form.Add(new ByteArrayContent(data), "database", "database.csv");

            var result = await client.PostAsync(uploadRoute, form);

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            //TODO: assert that the correct items appear in database only once.
            //TODO: assert malformed files
        }
        [Test]
        public async Task TestClear()
        {


            
            var result = await client.GetAsync(clearRoute);

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

            //TODO: Assert that there is nothing in the database
            
        }

    }
}
