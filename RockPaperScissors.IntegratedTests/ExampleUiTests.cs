using Microsoft.Playwright.MSTest;

namespace RockPaperScissors.IntegratedTests;

[TestClass]
public class ExampleUiTests : PageTest
{
    [TestMethod]
    public async Task NavigateToPageSubmitPlayerChoiceVerifyComputerPicksValidChoice()
    {
        PageUnderTest put = new(Page);
        await put.NavigateToGame();
        await put.SubmitChoice("rock");
        await put.VerifyValidChoice();
    }

    [TestMethod]
    public async Task NavigateToPageSubmitPlayerChoiceVerifyComputerPicksValidChoiceWithAltAssert()
    {
        PageUnderTest put = new(Page);
        await put.NavigateToGame();
        await put.SubmitChoice("paper");
        await Expect(put.ComputerChoice).ToBeVisibleAsync();
        await Expect(put.ComputerChoice).ToContainTextAsync("Computer Picks:", 
            new LocatorAssertionsToContainTextOptions { IgnoreCase = true });
    }
}