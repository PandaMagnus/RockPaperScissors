using Microsoft.Playwright.MSTest;
using System.Text.Json.Serialization;

namespace RockPaperScissors.IntegratedTests;

[TestClass]
public class ExampleApiTest : PlaywrightTest
{
    [TestMethod]
    public async Task ValidatePaperIsValid()
    {
        IAPIRequestContext api = await Playwright.APIRequest.NewContextAsync();
        IAPIResponse result = await api.PostAsync("https://rockpaperscissors.azurewebsites.net/api/rockpaperscissors/validate/paper");
        await Expect(result).ToBeOKAsync();
        Response? deserializedResponse = System.Text.Json.JsonSerializer.Deserialize<Response>(await result.BodyAsync());
        Assert.IsNotNull(deserializedResponse);
        Assert.IsTrue(deserializedResponse.IsPlayerSelectionValid, "Paper did not report as valid in API response.");
    }
}


public class Response
{
    [JsonPropertyName("isPlayerSelectionValid")]
    public bool IsPlayerSelectionValid { get; set; }
    [JsonPropertyName("playerChoice")]
    public int PlayerChoice { get; set; }
    [JsonPropertyName("computerChoice")]
    public int ComputerChoice { get; set; }
    [JsonPropertyName("gameResult")]
    public string? GameResult { get; set; }
    [JsonPropertyName("setResult")]
    public string? SetResult { get; set; }
    [JsonPropertyName("errorMessage")]
    public string? ErrorMessage { get; set; }
}
