using System;
using System.Net.WebSockets;

public class User
{
    public int Id { get; set; }
    public WebSocket Connection { get; set; }
    public String Login { get; set; }
}


