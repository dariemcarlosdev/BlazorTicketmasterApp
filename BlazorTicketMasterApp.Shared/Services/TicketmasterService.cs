using BlazorTicketmasterApp.Shared.Models;
using BlazorTicketmasterApp.Shared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;


public class TicketmasterService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly ILogger<TicketmasterService> _logger;

    public TicketmasterService(HttpClient httpClient, IConfiguration configuration, ILogger<TicketmasterService> logger)
    {
        _httpClient = httpClient;
        _apiKey = configuration["Ticketmaster:ApiKey"];
        _logger = logger;

        // Log the HttpClient configuration
        _logger.LogInformation("HttpClient BaseAddress: {BaseAddress}", _httpClient.BaseAddress);
        _logger.LogInformation("HttpClient Timeout: {Timeout}", _httpClient.Timeout);
    }

    // Search attractions by keyword
    public async Task<List<AttractionDto>> SearchAttractionsAsync(string searchTerm)
    {

        try
        {
            var apiResponse = await _httpClient.GetAsync($"discovery/v2/attractions.json?apikey={_apiKey}&keyword={searchTerm}");

            if (apiResponse.IsSuccessStatusCode)
            {
                var json = await apiResponse.Content.ReadAsStringAsync();//json is a string that contains the response from the API call to search for attractions.
                var jsonObject = JObject.Parse(json);//jsonObject is a JObject that contains the parsed JSON response and allows easy access to its properties.

                var dataApiResponse = new TicketmasterResponse<List<AttractionDto>>()
                {
                    responseData = new List<AttractionDto>()
                };

                dataApiResponse.responseData = jsonObject["_embedded"]?["attractions"]?.ToObject<List<AttractionDto>>();

                //if (jsonObject.TryGetValue("_embedded", out JToken embeddedToken))
                //{
                //    var jsonPropertyValue = embeddedToken?.ToString() ?? string.Empty;

                //    if (!string.IsNullOrEmpty(jsonPropertyValue))
                //    {
                //        var data = System.Text.Json.JsonSerializer.Deserialize<TicketmasterResponse<List<Attraction>>>(jsonPropertyValue);
                //        return data?.responseData ?? new List<Attraction>();
                //    }
                //}
                if (dataApiResponse.responseData != null && dataApiResponse.responseData != null)
                {
                    return dataApiResponse.responseData;
                }
            }
            else
            {
                _logger.LogError($"Failed to fetch attractions with search term {searchTerm}. Status Code: {apiResponse.StatusCode}");
            }
        }
        // Handle different exceptions and log detailed errors

        catch (HttpRequestException httpRequestException)
        {
            _logger.LogError(httpRequestException, "HTTP request error occurred.");
        }
        catch (TaskCanceledException taskCanceledException)
        {
            _logger.LogError(taskCanceledException, "Request timed out.");
        }
        catch (System.Text.Json.JsonException jsonException)
        {
            _logger.LogError(jsonException, "Error deserializing the response.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred.");
        }

        return new List<AttractionDto>();  // Return empty list in case of any failure
    }

    // Get events by attraction ID
    public async Task<List<EventDto>> GetEventsByAttractionIdAsync(string attractionId)
    {
        try
        {
            var apiResponse = await _httpClient.GetAsync($"discovery/v2/events.json?apikey={_apiKey}&attractionId={attractionId}");

            if (apiResponse.IsSuccessStatusCode)
            {
                var json = await apiResponse.Content.ReadAsStringAsync(); //json is a string that contains the response from the API call to get events.
                _logger.LogInformation($"Response JSON: {json}");  // Log the raw response

                var jsonObject = JObject.Parse(json); //jsonObject is a JObject that contains the parsed JSON response and allows easy access to its properties.

                // Access the list of events
                var eventsArray = jsonObject["_embedded"]?["events"] as JArray;

                if (eventsArray != null)
                {
                    var eventsList = new TicketmasterResponse<List<EventDto>>()
                    {
                        responseData = new List<EventDto>()
                    };

                    foreach (var eventObj in eventsArray) // Take only the first 2 events
                    {
                        var eventItem = new EventDto
                        {
                            Name = eventObj["name"]?.ToString().ToUpper(),
                            //LocalDate = eventObj["dates"]?["start"]?["localDate"]?.ToString(),
                            //LocalTime = eventObj["dates"]?["start"]?["localTime"]?.ToString(), convert to date time and format it
                            LocalDate = DateTime.Parse(eventObj["dates"]?["start"]?["localDate"]?.ToString()).ToString("dddd, MMMM dd, yyyy").ToUpper(),  
                            Images = eventObj["images"]?.ToObject<List<ImageDto>>(),
                            Venue = eventObj["_embedded"]?["venues"]?[0]?["name"]?.ToString().ToUpper() + "," + eventObj["_embedded"]?["venues"]?[0]?["city"]?["name"]?.ToString().ToUpper() + "," + eventObj["_embedded"]?["venues"]?[0]?["state"]?["stateCode"]?.ToString().ToUpper()

                        };

                        eventsList.responseData.Add(eventItem);
                    }
                    //order by event date
                    eventsList.responseData = eventsList.responseData.OrderBy(e => DateTime.Parse(e.LocalDate)).ToList();
                    return eventsList.responseData;
                }
                else
                {
                    _logger.LogError($"No events found for attraction with ID {attractionId}.");
                }
            }
            else
            {
                _logger.LogError($"Failed to fetch attraction with ID {attractionId}. Status Code: {apiResponse.StatusCode}");
            }
        }
        // Handle different exceptions and log detailed errors

        catch (HttpRequestException httpRequestException)
        {
            _logger.LogError(httpRequestException, "HTTP request error occurred.");
        }
        catch (TaskCanceledException taskCanceledException)
        {
            _logger.LogError(taskCanceledException, "Request timed out.");
        }
        catch (System.Text.Json.JsonException jsonException)
        {
            _logger.LogError(jsonException, "Error deserializing the response.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred.");
        }

        return new List<EventDto>();  // Return empty list in case of failure
    }

    // Get attraction details by attraction ID
    public async Task<AttractionDto> GetAttractionByIdAsync(string attractionId)
    {
        try
        {
            var apiResponse = await _httpClient.GetAsync($"discovery/v2/attractions/{attractionId}.json?apikey={_apiKey}");

            if (apiResponse.IsSuccessStatusCode)
            {
                var json = await apiResponse.Content.ReadAsStringAsync(); //json is a string that contains the response from the API call to get attraction details.
                _logger.LogInformation($"Response JSON: {json}");  // Log the raw response

                JObject jsonObject = JObject.Parse(json); // Parse the JSON response using Newtonsoft.Json to extract the required data and avoid case sensitivity issues and ease of access to nested objects and properties.

                var dataAPiResponse = new TicketmasterResponse<AttractionDto>
                {
                    responseData = new AttractionDto()
                };

                dataAPiResponse.responseData.id = jsonObject["id"]?.ToObject<string>();
                dataAPiResponse.responseData.externalLinks = jsonObject["externalLinks"]?.ToObject<ExternalLinksDto>();
                dataAPiResponse.responseData.name = jsonObject["name"]?.ToObject<string>();
                dataAPiResponse.responseData.images = jsonObject["images"]?.ToObject<List<ImageDto>>();
                dataAPiResponse.responseData.events = await GetEventsByAttractionIdAsync(attractionId);


                //Here I am not using System.Text.Json.JsonSerializer since it does not support deserializing nested objects and I have already used Newtonsoft.Json for deserializing the response into JObject.

                //var options = new JsonSerializerOptions
                //{
                //    PropertyNameCaseInsensitive = true, //false is the default value and can be omitted, if true it will ignore case
                //    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault //this will ignore properties with default values when serializing
                //};

                // var data = System.Text.Json.JsonSerializer.Deserialize<TicketmasterAttractionResponse<Attraction>>(json,options);

                if (dataAPiResponse != null && dataAPiResponse.responseData != null)
                {
                    return dataAPiResponse.responseData;
                }
            }
            else
            {
                _logger.LogError($"Failed to fetch attraction with ID {attractionId}. Status Code: {apiResponse.StatusCode}");
            }
        }
        // Handle different exceptions and log detailed errors

        catch (HttpRequestException httpRequestException)
        {
            _logger.LogError(httpRequestException, "HTTP request error occurred.");
        }
        catch (TaskCanceledException taskCanceledException)
        {
            _logger.LogError(taskCanceledException, "Request timed out.");
        }
        catch (System.Text.Json.JsonException jsonException)
        {
            _logger.LogError(jsonException, "Error deserializing the response.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred.");
        }


        return new AttractionDto();  // Return an empty Attraction in case of any failure
    }
}
