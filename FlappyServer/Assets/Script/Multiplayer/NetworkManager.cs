using System;
using System.Collections;
using System.Collections.Generic;
using Riptide;
using Riptide.Utils;
using UnityEngine;

public enum ServerToClientId : ushort
{
    PlayerSpawned = 0,
    PlayerMovement = 1,
    WallMovement = 2,
    WallDestroyed = 3,
    WallSpawned = 4,
    GroundMove = 5,
    CollideWithWall = 6,
}

public enum ClientToServerId : ushort
{
    Name = 0,
    Input = 1,
}

public class NetworkManager : Singleton<NetworkManager>
{
    public Server Server { get; private set; }

    [SerializeField] private ushort port;
    [SerializeField] private ushort maxClientCount;

    private void Start()
    {
        Application.targetFrameRate = 90;
        
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);

        Server = new Server("Flappy Server");
        Server.Start(port, maxClientCount);

        Server.ClientConnected += OnClientConnect;
        Server.ClientDisconnected += OnClientDisconnect;
    }

    private void OnClientDisconnect(object sender, ServerDisconnectedEventArgs e)
    {
        if (Player.list.TryGetValue(e.Client.Id, out Player player))
        {
            Destroy(player.gameObject);
            if (Player.list.Count == 0)
            {
                Spawner.Instance.DeleteAll();
            }
        }
    }

    private void OnClientConnect(object sender, ServerConnectedEventArgs e)
    {
        
    }

    private void FixedUpdate()
    {
        Server.Update();
    }

    private void OnApplicationQuit()
    {
        Server.Stop();
    }
    
}
