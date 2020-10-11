﻿using Xunit;
using Bunit;
using BlazorServerApp.Pages;
using System.Net.Http;
using RockPaperScissors.Tests.Data;
using Microsoft.Extensions.DependencyInjection;
using AngleSharp.Dom;

namespace RockPaperScissors.Tests.BUnitTests
{
    public class BUnitTests : TestContext
    {
        [Fact]
        public void InputBoxDoesNotInitializeAsAlert()
        {
            Services.AddSingleton<HttpClient, HttpClientFake>();
            var comp = RenderComponent<RPS>();

            Assert.Contains(@"class=""alert alert-info""", 
                comp.Find(RpsPage.PlayerInput).ToMarkup());
        }

        [Fact]
        public void SubmitButtonDoesNotInitializeAsAlert()
        {
            Services.AddSingleton<HttpClient, HttpClientFake>();
            var comp = RenderComponent<RPS>();

            Assert.Contains(@"class=""btn btn-primary""",
                comp.Find(RpsPage.SubmitButton).ToMarkup());
        }

        [Fact]
        public void BadInputMarksInputBoxAsAlert()
        {
            Services.AddSingleton<HttpClient, HttpClientFake>();
            var comp = RenderComponent<RPS>();
            comp.Find(RpsPage.PlayerInput).Change("bad_value");
            comp.Find(RpsPage.SubmitForm).Submit();

            Assert.Contains(@"class=""alert alert-danger""",
                comp.Find(RpsPage.PlayerInput).ToMarkup());
        }

        [Fact]
        public void BadInputMarksSubmitButtonAsAlert()
        {
            Services.AddSingleton<HttpClient, HttpClientFake>();
            var comp = RenderComponent<RPS>();
            comp.Find(RpsPage.PlayerInput).Change("bad_value");
            comp.Find(RpsPage.SubmitForm).Submit();

            Assert.Contains(@"class=""btn btn-danger""",
                comp.Find(RpsPage.SubmitButton).ToMarkup());
        }
    }
}
