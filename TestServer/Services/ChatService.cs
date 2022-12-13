﻿using System;
using System.Net.WebSockets;

public class ChatService : IChatService
{


    // Список всех клиентов
    private static readonly List<WebSocket> Clients = new List<WebSocket>();

    // Блокировка для обеспечения потокабезопасности
    private static readonly ReaderWriterLockSlim Locker = new ReaderWriterLockSlim();

    private static List<User> Users = new List<User>();


    public async Task WebSocketRequest(string login, HttpContext context)
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            Console.WriteLine(context.Request.ToString());
            Console.WriteLine(login);
            var connections = new List<WebSocket>();
            var user = new User();
            var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            user.Login = login;
            user.Id = Users.Count() + 1;
            user.Connection = webSocket;
            // Добавляем нового пользователя ко всем остальным 
            // connections.Append(await HttpContext.WebSockets.AcceptWebSocketAsync());

            await WebSocketRequest(webSocket, user);
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }


    public async Task<IBaseResponse<List<User>>> GetUsers()
    {
        var baseResponse = new BaseResponse<List<User>>();
        baseResponse.Description = "Get users";
        baseResponse.StatusCode = StatusCode.OK;

        baseResponse.Data = Users;
        return baseResponse;
    }


    private async Task WebSocketRequest(WebSocket socket, User user = null)
    {
        // Получаем сокет клиента из контекста запроса
        //  var socket = context;

        // Добавляем его в список клиентов
        if (user != null && Users.Where(item => item.Login == user.Login).Count() == 0 && user.Login != "" && user.Login != null)
        {
            Locker.EnterWriteLock();
            try
            {
                Users.Add(user);
            }
            finally
            {
                Locker.ExitWriteLock();
            }
        }
        /* Locker.EnterWriteLock();
         try
         {
             Clients.Add(socket);
         }
         finally
         {
             Locker.ExitWriteLock();
         }*/
        Console.WriteLine(Users.Count());
        // Слушаем его
        while (true)
        {
            var buffer = new ArraySegment<byte>(new byte[1024]);

            // Ожидаем данные от него
            var result = await socket.ReceiveAsync(buffer, CancellationToken.None);


            //Передаём сообщение всем клиентам
            for (int i = 0; i < Users.Count(); i++)
            {

                User client = Users[i];

                try
                {
                    if (client.Connection.State == WebSocketState.Open)
                    {
                        await client.Connection.SendAsync(buffer.Slice(0, result.Count), WebSocketMessageType.Binary, true, CancellationToken.None);
                    }
                    else
                    {
                        Console.WriteLine("Close");
                        Console.WriteLine(client.Connection.State);
                        Locker.EnterWriteLock();
                        try
                        {

                            Users.RemoveAt(i);
                            i--;
                            foreach(var u in Users)
                            {
                                Console.WriteLine(u.Login);
                            }

                        }
                        finally
                        {
                            Locker.ExitWriteLock();
                        }
                    }
                }

                catch (ObjectDisposedException)
                {
                    Locker.EnterWriteLock();
                    try
                    {
                        Users.RemoveAt(i);

                        i--;
                    }
                    finally
                    {
                        Locker.ExitWriteLock();
                    }
                }
            }

        }
    }

    private static async Task Echo(WebSocket webSocket)
    {
        var buffer = new byte[1024 * 4];
        var receiveResult = await webSocket.ReceiveAsync(
            new ArraySegment<byte>(buffer), CancellationToken.None);

        while (!receiveResult.CloseStatus.HasValue)
        {
            Console.WriteLine(webSocket.State.ToString());
            await webSocket.SendAsync(
                new ArraySegment<byte>(buffer, 0, receiveResult.Count),
                receiveResult.MessageType,
                receiveResult.EndOfMessage,
                CancellationToken.None);
            buffer = new byte[1024 * 4];
            receiveResult = await webSocket.ReceiveAsync(
                new ArraySegment<byte>(buffer), CancellationToken.None);
            Console.WriteLine(System.Text.Encoding.Default.GetString(buffer.Where(bit => bit != 0).ToArray()));
        }
        Console.WriteLine("Close socket");
        await webSocket.CloseAsync(
            receiveResult.CloseStatus.Value,
            receiveResult.CloseStatusDescription,
            CancellationToken.None);
    }
}

