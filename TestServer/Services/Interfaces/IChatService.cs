using System;

public interface IChatService
{
    Task WebSocketRequest(User user, HttpContext context);
    Task<IBaseResponse<List<User>>> GetUsers();
}


