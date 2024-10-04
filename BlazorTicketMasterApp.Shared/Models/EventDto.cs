using System.Text.Json.Serialization;

namespace BlazorTicketmasterApp.Shared.Models
{
    public class EventDto
    {
        public required string? Name { get; set; }
        [JsonPropertyName("venues")]
        public string? Venue { get; set; }

        //get atrribute for getting nested properties form json

        public string?   LocalDate { get; set; }
        [JsonPropertyName("images")]
        public List<ImageDto>? Images { get; set; }// List of images for the event

        //Foreign key to associate the event with an attraction. This is used to create a relationship between the Event and Attraction entities. Just for demonstration purposes with Entity Framework Core.
        public string? AttractionId { get; set; } //// ID of the related Attraction
        public AttractionDto? Attraction { get; set; } // Navigation property to get the Attraction associated with the event
    }
}