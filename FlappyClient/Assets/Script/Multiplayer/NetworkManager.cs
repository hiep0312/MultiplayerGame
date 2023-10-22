using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Riptide;
using Riptide.Utils;

public enum ServerToClientId : ushort
{
    PlayerSpawned = 0,
    PlayerMovement = 1,
}

public enum ClientToServerId : ushort
{
    Name = 0,
    Input = 1,
}


public class NetworkManager : Singleton<NetworkManager>
{
    public Client Client;
    
    [SerializeField] private string ip;
    [SerializeField] private ushort port;
    public void Start()
    {
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);

        Client = new Client();
        Client.Connected += DidConnect;
        Client.ConnectionFailed += FailedToConnect;
        Client.ClientDisconnected += PlayerLeft; // other player left server
        Client.Disconnected += DidDisconnect; // this player left server 
    }

    public void Connect()
    {
        Client.Connect($"{ip}:{port}");
    }

    private void FixedUpdate()
    {
        Client.Update();
    }

    private void OnApplicationQuit()
    {
        Client.Disconnect();
    }

    private void DidDisconnect(object sender, EventArgs e)
    {
        UIManager.Instance.BackToMain();
        foreach (Player player in Player.list.Values)
            Destroy(player.gameObject);
    }
    private void PlayerLeft(object sender, ClientDisconnectedEventArgs e)
    {
        if (Player.list.TryGetValue(e.Id, out Player player))
        {
            Destroy(player.gameObject);
        }
    }

    private void FailedToConnect(object sender, ConnectionFailedEventArgs e)
    {
        UIManager.Instance.BackToMain();
    }

    private void DidConnect(object sender, EventArgs e)
    {
        UIManager.Instance.SendName();
    }
}
