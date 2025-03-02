public class MessageDTO
{
    public required string id { get; set; }
    public string? data { get; set; }
    public string? message { get; set; }
    public required string sender { get; set; }
    public required string type { get; set; }
    public bool isSent { get; set; } = false;
}