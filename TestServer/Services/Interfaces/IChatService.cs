using System;

public interface IChatService
{
    Task WebSocketRequest(string login, HttpContext context);
    Task<IBaseResponse<List<User>>> GetUsers();
}


