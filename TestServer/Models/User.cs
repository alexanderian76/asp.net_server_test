using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.WebSockets;

public class User
{
    public int Id { get; set; }
    public required string Login { get; set; }
    public required string TotpKey { get; set; }

    [NotMapped]
    public WebSocket? Connection { get; set; }

}


