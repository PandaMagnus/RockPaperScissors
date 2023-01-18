using RockPaperScissors.Client.Pages;
using RockPaperScissors.Tests.Data;
using Xunit;

namespace RockPaperScissors.Tests
{
    public class RazorTests
    {
        [Fact]
        public async void PlayerSendsValidAndWinningChoiceGetsSuccessfulResultBack()
        {
            // Need way to mock out API. Probably override DI here?
            var view = new RpsGame();

            var httpClient = view.GetType().GetProperty(
                "Http",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance);
            httpClient.SetValue(view, new HttpClientFake());

            var playerSelection = view.GetType().GetField(
                "playerSelection",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance);
            var sendPlayerInputMethod = view.GetType().GetMethod(
                "SendUserChoice",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance);

            playerSelection.SetValue(view, "rock-win");
            await (Task)sendPlayerInputMethod.Invoke(view, null);

            Assert.Equal(
                "rock-win", 
                view.GetType().GetField(
                    "playerSelection", 
                    System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Instance).GetValue(view));
            Assert.Equal(
                "Scissors",
                view.GetType().GetField(
                    "computerPick",
                    System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Instance).GetValue(view));
            Assert.Equal(
                "Congratulations, you won!",
                view.GetType().GetField(
                    "result",
                    System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Instance).GetValue(view));
            Assert.False(
                bool.Parse(view.GetType().GetField(
                    "error",
                    System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Instance).GetValue(view).ToString()));
        }
    }
}

/*    private string playerSelection;
    private string computerPick;
    private string result;
    private bool error;
*/