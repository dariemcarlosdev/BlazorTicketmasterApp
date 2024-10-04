namespace BlazorTicketmasterApp.Shared.Models
{
    public class AttractionDto
    {

        public ExternalLinksDto? externalLinks;
        public string? name { get; set; }
        public string? type { get; set; }
        public string? id { get; set; }
        public List<ImageDto>? images { get; set; }
        //Navigation property to get the events associated with the attraction
        public List<EventDto>? events { get; set; } = new();
    }

}