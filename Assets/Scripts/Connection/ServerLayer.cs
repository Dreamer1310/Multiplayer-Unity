using Microsoft.AspNetCore.SignalR.Client;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

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
        public GameConnectionClient PlayerJoined(Func<PlayerDto, Int32> callback)
        {
            if (Callbacks.ContainsKey("PlayerJoined")) throw new Exception("PlayerJoined method is already binded!");
            Callbacks.Add("PlayerJoined", callback);
            return this;
        }
        public GameConnectionClient PlayerDisconnected(Func<Int64, Int32> callback)
        {
            if (Callbacks.ContainsKey("PlayerDisconnected")) throw new Exception("PlayerDisconnected method is already binded!");
            Callbacks.Add("PlayerDisconnected", callback);
            return this;
        }
        public GameConnectionClient State(Func<StateDto, Int32> callback)
        {
            if (Callbacks.ContainsKey("State")) throw new Exception("State method is already binded!");
            Callbacks.Add("State", callback);
            return this;
        }
        public GameConnectionClient PlayerData(Func<PlayerDto, Int32> callback)
        {
            if (Callbacks.ContainsKey("PlayerData")) throw new Exception("PlayerData method is already binded!");
            Callbacks.Add("PlayerData", callback);
            return this;
        }

    }

    public class GameConnection
    {
        public GameConnectionClient Client { get; private set; }

        public Server Server { get; private set; }

        private HubConnection _connection;

        public GameConnection(String baseUrl, Cookie cookie)
        {
            _connection = new HubConnectionBuilder()
                .WithUrl(baseUrl + "/game", (options) =>
                {
                    options.Cookies.Add(cookie);
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

            _connection.On<PlayerDto>("PlayerJoined", (player) =>
            {
                Client.Callbacks.TryGetValue("PlayerJoined", out var callback);
                callback.DynamicInvoke(player);
            });

            _connection.On<Int64>("PlayerDisconnected", (playerId) =>
            {
                Client.Callbacks.TryGetValue("PlayerDisconnected", out var callback);
                callback.DynamicInvoke(playerId);
            });

            _connection.On<StateDto>("State", (state) =>
            {
                Client.Callbacks.TryGetValue("State", out var callback);
                callback.DynamicInvoke(state);
            });

            _connection.On<PlayerDto>("PlayerData", (player) =>
            {
                Client.Callbacks.TryGetValue("PlayerData", out var callback);
                callback.DynamicInvoke(player);
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
            if (!Client.Callbacks.ContainsKey("PlayerDisconnected")) throw new Exception("PlayerDisconnected Is Not Bind For Client!");
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

        public void Disconnect()
        {
            Connection.InvokeAsync("Disconnect");
        }
    }

    public class PlayerDto
    {
        public Int64 ID { get; set; }
        public float X { get; set; }
        public float Z { get; set; }
    }

    public class StateDto
    {
        public List<PlayerDto> Players { get; set; }
    }
}


