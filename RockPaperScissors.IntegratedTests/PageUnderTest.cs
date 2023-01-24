namespace RockPaperScissors.IntegratedTests;

public class PageUnderTest
{
    public PageUnderTest(IPage pageUnderTest)
    {
        Page = pageUnderTest;
        ComputerChoice = Page.GetByTestId(ComputerChoiceLabelId);
    }

    // Public ILocator is an example.
    // I do not recommend mixing asserts in page model and test methods.
    public ILocator ComputerChoice { get; }

    private const string Url = "https://rockpaperscissors.azurewebsites.net/";
    private const string NavButtonId = "rps-nav";
    private const string PlayerChoiceInputId = "player-input";
    private const string SubmitChoiceButtonId = "submit-input-btn";
    private const string ComputerChoiceLabelId = "computer-choice";
    private const string GameOutcomeLabelId = "result";
    private readonly IPage Page;

    public async Task NavigateToGame()
    {
        await Page.GotoAsync(Url);
        await Page.GetByTestId(NavButtonId).ClickAsync();

        ILocator playerChoice = Page.GetByTestId(PlayerChoiceInputId);
        await playerChoice.IsVisibleAsync();
        await Assertions.Expect(playerChoice).ToBeVisibleAsync();
    }

    public async Task SubmitChoice(string choice)
    {
        await Page.GetByTestId(PlayerChoiceInputId).FillAsync(choice);
        await Page.GetByTestId(SubmitChoiceButtonId).ClickAsync();
    }

    public async Task VerifyValidChoice()
    {
        ILocator computerChoice = Page.GetByTestId(ComputerChoiceLabelId);
        await Assertions.Expect(computerChoice).ToBeVisibleAsync();
        await Assertions.Expect(computerChoice).ToContainTextAsync("Computer Picks",
            new LocatorAssertionsToContainTextOptions { IgnoreCase = true });
        await Assertions.Expect(Page.GetByTestId(GameOutcomeLabelId))
            .Not.ToContainTextAsync("Awaiting player input",
            new LocatorAssertionsToContainTextOptions { IgnoreCase = true });
    }
}
