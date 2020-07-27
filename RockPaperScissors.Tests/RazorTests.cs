using System;
using System.Collections.Generic;
using System.Text;
using BlazorServerApp.Pages;
using Xunit;

namespace RockPaperScissors.Tests
{
    public class RazorTests
    {
        [Fact]
        public void Testing()
        {
            var test = new BlazorServerApp.Pages.RPS();
            var test2 = test.GetType().GetMethods(
                System.Reflection.BindingFlags.NonPublic|
                System.Reflection.BindingFlags.Instance);
        }
    }
}
