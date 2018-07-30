using System;
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
using Newtonsoft.Json;

namespace CaseKataTest
{
    [TestFixture]
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
        public async Task GivenCaseRequest_WhenCaseDoesNotExist_ThenReturnNotFound()
        {
            var response = await _client.GetAsync("http://localhost:5000/api/casefile/30");
            
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test]
        public async Task GivenCaseRequest_WhenCaseDoesExist_ThenReturnCase()
        {
            var casefile = new CaseFile();
            casefile.Description = "Cain was jealous";
            casefile.Title = "Case Number One";
            casefile.DocketId = 1;
            casefile.Id = "1";

            await AddCaseFile(casefile);

            var casefiles = await RetrieveCaseFiles(casefile.DocketId);

            Assert.AreEqual(1, casefiles.Count);
            
            Assert.AreEqual(casefile.Description, casefiles[0].Description);
            Assert.AreEqual(casefile.Title, casefiles[0].Title);
            Assert.AreEqual(casefile.Id, casefiles[0].Id);
        }

        [Test]
        public async Task GivenACaseDoesNotExist_WhenANewOneIsSubmitted_ItIsGeneratedWithAnOpenDate()
        {
            var startTime = DateTime.Now;
            var casefile = new CaseFile();
            casefile.Description = "Cookie Monster stole from the cookie jar";
            casefile.Title = "Case Number Two";
            casefile.DocketId = 2;
            casefile.Id = "2";

            await AddCaseFile(casefile);

            var casefiles = await RetrieveCaseFiles(casefile.DocketId);
            var endTime = DateTime.Now;
            Assert.AreEqual(1, casefiles.Count);
            Assert.IsTrue(casefiles[0].OpenDate > startTime && casefiles[0].OpenDate < endTime);
        }
        
        private async Task AddCaseFile(CaseFile casefile)
        {
            var jsonCaseFile = JsonConvert.SerializeObject(casefile);
            var contentExpected = new StringContent(jsonCaseFile, Encoding.UTF8, "application/json");

            var postResponse = await _client.PostAsync("http://localhost:5000/api/casefile/", contentExpected);
            Assert.IsTrue(postResponse.IsSuccessStatusCode);
        }
        
        private async Task<IList<CaseFile>> RetrieveCaseFiles(int docketId)
        {
            var response = await _client.GetAsync("http://localhost:5000/api/casefile/" + docketId);
            Assert.IsTrue(response.IsSuccessStatusCode, "response code: " + response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            var casefiles = JsonConvert.DeserializeObject<IList<CaseFile>>(content);
            return casefiles;
        }
        
        [TearDown]
        public void Dispose()
        {
            _client.Dispose();
            _server.Dispose();
        }
    }
}
