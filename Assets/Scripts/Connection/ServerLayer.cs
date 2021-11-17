using Microsoft.AspNetCore.SignalR.Client;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections;
using System.Collections.Generic;


namespace ServerLayer
{
    public class GameConnectionClient
    {
        public readonly Dictionary<String, Delegate> Callbacks = new Dictionary<String, Delegate>();
        
        public GameConnectionClient Moved(Func<Double, Double, Int32> callback)
        {
            if (Callbacks.ContainsKey("Moved")) throw new Exception("Moved method is already binded!");
            Callbacks.Add("Moved", callback);
            return this;
        }

        public GameConnectionClient MovementError(Func<String, Int32> callback)
        {
            if (Callbacks.ContainsKey("MovementError")) throw new Exception("MovementError method is already binded!");
            Callbacks.Add("MovementError", callback);
            return this;
        }

        public GameConnectionClient PlayerJoined(Func<Int32> callback)
        {
            if (Callbacks.ContainsKey("PlayerJoined")) throw new Exception("PlayerJoined method is already binded!");
            Callbacks.Add("PlayerJoined", callback);
            return this;
        }

    }

    public class GameConnection
    {
        public GameConnectionClient Client { get; private set; }

        public Server Server { get; private set; }

        private HubConnection _connection;

        public GameConnection(String baseUrl)
        {
            _connection = new HubConnectionBuilder()
                .WithUrl(baseUrl + "/game", (options) =>
                {
                
                })
                .Build();

            _connection.On<Double, Double>("Moved", (x, y) =>
            {
                Client.Callbacks.TryGetValue("Moved", out var callback);
                callback.DynamicInvoke(x, y);
            });

            _connection.On<String>("MovementError", (errorString) =>
            {
                Client.Callbacks.TryGetValue("MovementError", out var callback);
                callback.DynamicInvoke(errorString);
            });

            Client = new GameConnectionClient();
            Server = new Server();
            Server.Connection = _connection;
        }    

        public async void Start()
        {
            CheckClientBindings();
            await _connection.StartAsync();
        }

        public async void Stop()
        {
            await _connection.StopAsync();
        }

        private void CheckClientBindings()
        {
            if (!Client.Callbacks.ContainsKey("Moved")) throw new Exception("Moved Is Not Bind For Client!");
            if (!Client.Callbacks.ContainsKey("MovementError")) throw new Exception("MovementError Is Not Bind For Client!");
            if (!Client.Callbacks.ContainsKey("PlayerJoined")) throw new Exception("PlayerJoined Is Not Bind For Client!");
        }
    }

    public class Server
    {
        public HubConnection Connection { get; set; }

        public void MoveForward()
        {
            Connection.InvokeAsync("MoveForward");
        }

        public void MoveLeft()
        {
            Connection.InvokeAsync("MoveLeft");
        }

        public void MoveRight()
        {
            Connection.InvokeAsync("MoveRight");
        }

        public void MoveBackward()
        {
            Connection.InvokeAsync("MoveBackward");
        }
    }
}


