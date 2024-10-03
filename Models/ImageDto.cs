namespace BlazorTicketmasterApp.Models
{
    public class ImageDto
    {
        // Add properties for images if available in the full response.
        public string url { get; set; }
        public string ratio { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public bool fallback { get; set; }

    }
}