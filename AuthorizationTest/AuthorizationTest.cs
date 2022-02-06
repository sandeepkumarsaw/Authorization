using System;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Authorization.Repository;
using Authorization.Services;
using Authorization.Controllers;
using Microsoft.Extensions.Configuration;
using Authorization.Models;
using Moq;

namespace AuthorizationTest
{
    public class AuthorizationTest
    {
        private AuthenticationRepository repo;
        private AuthenticationService service;
        private AuthenticationController controller;
        private IConfiguration config = new ConfigurationBuilder().AddJsonFile($"appsettings.json", optional: false).Build();
        public AuthorizationTest()
        {
            //_config = config;
            repo = new AuthenticationRepository();
            service = new AuthenticationService(config, repo);
            controller = new AuthenticationController(config, service);
        }

        [Fact]
        public void Test_login()
        {
            string Username = "admin";
            string Password = "password";
            var data = controller.Login(new LoginInput() { Username=Username, Password=Password});
            Assert.NotNull(data);
        }

        [Fact]
        public void Test_generate_token()
        {
            string Username = "admin";
            string data = service.GenerateJsonWebToken(Username);
            Assert.NotNull(data);
        }

        [Fact]
        public void Test_getUserDetails()
        {
            string Username = "admin";
            string Password = "password";
            var data = repo.GetUserDetails(new LoginInput() { Username = Username, Password = Password });
            Assert.NotNull(data);
        }
    }
}
