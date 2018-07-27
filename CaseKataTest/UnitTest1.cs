using System.Collections.Generic;
using System.Net;
using NUnit.Framework;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CaseKata;
using CaseKata.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using Newtonsoft.Json;

namespace CaseKataTest
{
    public class UnitTest1
    {
        private HttpClient _client;
        private TestServer _server;
        [SetUp]
        public void SetUp()
        {
            var builder = new WebHostBuilder().UseStartup<Startup>();
            _server = new TestServer(builder);
            _client = _server.CreateClient();
        }

        [Test]
        public async Task GivenCaseRequestWhenCaseDoesNotExistThenReturnNotFound()
        {
            var response = await _client.GetAsync("http://localhost:5000/api/casefile/30");
            
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test]
        public async Task GivenCaseRequestWhenCaseDoesExistThenReturnCase()
        {
            var casefile = new CaseFile();
            casefile.Description = "Cain was jealous";
            casefile.Title = "Case Number One";
            casefile.DocketId = 1;
            casefile.Id = "1";

            var jsonCaseFile = JsonConvert.SerializeObject(casefile);
            var contentExpected = new StringContent(jsonCaseFile, Encoding.UTF8, "application/json");

            var postResponse = await _client.PostAsync("http://localhost:5000/api/casefile/", contentExpected);
            Assert.IsTrue(postResponse.IsSuccessStatusCode);
            
            var response = await _client.GetAsync("http://localhost:5000/api/casefile/1");
            Assert.IsTrue(response.IsSuccessStatusCode, "response code: " + response.StatusCode);
            
            var content = await response.Content.ReadAsStringAsync();
            var casefiles = JsonConvert.DeserializeObject<IList<CaseFile>>(content);
            Assert.AreEqual(1, casefiles.Count);
            
            //compare cases
            Assert.AreEqual(casefile.Description, casefiles[0].Description);
            Assert.AreEqual(casefile.Title, casefiles[0].Title);
            Assert.AreEqual(casefile.Id, casefiles[0].Id);
        }
        
        [TearDown]
        public void Dispose()
        {
            _client.Dispose();
            _server.Dispose();
        }
    }
}
