using FreesqlDemo.API.Controllers;
using FreesqlDemo.API.DbModels;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace FreesqlDemo.API.Test
{
    public class HomeControllerTest
    {
        //private readonly HomeController _homeController;
        public HttpClient Client { get; }
        public HomeControllerTest(ITestOutputHelper outputHelper)
        {
            //var dbContext = new Mock<AppDbContext>();
            //_homeController = new HomeController(dbContext.Object);
            var server = new TestServer(Program.CreateHostBuilder(new string[] { }).Build().Services);
            Client = server.CreateClient();
        }

        [Fact]
        public async void Get()
        {
            var result = await Client.GetAsync("/api/Home");
            var list = await result.Content.ReadAsStringAsync();
            JObject objectRes = JsonConvert.DeserializeObject<JObject>(list);
            foreach (var obj in objectRes)
            {
                Trace.WriteLine(obj.Key);
                Trace.WriteLine(obj.Value);
            }
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        private int Add(int a, int b)
        {
            return a + b;
        }
    }
}