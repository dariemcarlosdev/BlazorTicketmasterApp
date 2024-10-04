namespace BlazorTicketmasterApp.Shared.Models
{
    public class ExternalLinksDto
    {
        public List<Link>? youtube { get; set; }
        public List<Link>? spotify { get; set; }
        public List<Link>? xtwitter { get; set; }
        public List<Link>? homepage { get; set; }
    }


    public class Link
    {
        public string url { get; set; }
    }
}