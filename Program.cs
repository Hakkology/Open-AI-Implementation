using Newtonsoft.Json;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;

internal class Program {
    private static void Main(string[] args) {
        Console.WriteLine("How can i help ?");
        var question = Console.ReadLine();

        var answer = CallOpenAI(250, question, "text-davinci-002", 0.7, 1, 0, 0);
        Console.WriteLine(answer);
    }

    private static string? CallOpenAI(int tokens, string input, string engine, double temperature, int topP, int frequencyPenalty, int presencePenalty) {

        var openAIKey = "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";

        var apiCall = "https://api.openai.com/v1/engines/" + engine + "/completions";

        try {

            using (var httpClient = new HttpClient()) {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), apiCall)){

                    request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + openAIKey);
                    request.Content = new StringContent("{\n  \"prompt\": \"" + input + "\",\n  \"temperature\": " +
                                                        temperature.ToString(CultureInfo.InvariantCulture) + ",\n  \"max_tokens\": " + tokens + ",\n  \"top_p\": " + topP +
                                                        ",\n  \"frequency_penalty\": " + frequencyPenalty + ",\n  \"presence_penalty\": " + presencePenalty + "\n}");

                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                    var response = httpClient.SendAsync(request).Result;
                    var json = response.Content.ReadAsStringAsync().Result;

                    dynamic dynObj = JsonConvert.DeserializeObject(json);

                    if (dynObj != null) {
                        return dynObj.choices[0].text.ToString();
                    }
                }
            }
        }
        catch (Exception ex) {

            Console.Write(ex.Message);
        }

        return null;
    }
}