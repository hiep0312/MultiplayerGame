using System;
using System.Collections.Generic;
using Riptide;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Dictionary<ushort, Player> list = new Dictionary<ushort, Player>();

    public ushort Id { get; private set; }
    public string Username { get; private set; }
    
    public bool IsAlive { get; set; }
    public PlayerMovement Movement => movement;

    [SerializeField] private PlayerMovement movement;

    private void Start()
    {
        IsAlive = true;
    }

    private void OnDestroy()
    {
        list.Remove(Id);
    }

    public static void Spawn(ushort id, string username)
    {
        // send other data player to player id
        foreach (Player otherPlayer in list.Values)
        {
            otherPlayer.SendSpawned(id);
        }
        
        
        // spawn player
        Player player = Instantiate(GameLogic.Instance.PlayerPrefab, Vector3.up, Quaternion.identity).GetComponent<Player>();
        player.name = $"Player {id} ({(String.IsNullOrEmpty(username) ? "Guest" : username)})";
        player.Id = id;
        player.Username = string.IsNullOrEmpty(username) ? $"Guest {id}" : username;

        player.SendSpawned();
        list.Add(id, player);
    }

    public static bool CheckPlayerRemain()
    {
        foreach (ushort key in list.Keys)
        {
            if (list[key].IsAlive) return true;
        }

        return false;
    }

    #region Message Handler

    [MessageHandler((ushort)ClientToServerId.Name)]
    private static void GetUsernameFromClient(ushort clientId, Message message)
    {
        Spawn(clientId, message.GetString());
    }

    [MessageHandler((ushort)ClientToServerId.Input)]
    private static void Input(ushort fromClientId, Message message)
    {
        if (list.TryGetValue(fromClientId, out Player player))
            player.Movement.SetInput(message.GetBool());
    }
    #endregion

    
    
    #region Messages

    /// <summary>
    /// Send event to all client is connecting
    /// </summary>
    public void SendSpawned()
    {
        NetworkManager.Instance.Server.SendToAll(AddSpawnData(Message.Create(MessageSendMode.Reliable, ServerToClientId.PlayerSpawned)));
    }
    
    
    /// <summary>
    /// Send to client Id
    /// </summary>
    /// <param name="toClientId">Id of player will receive message</param>
    public void SendSpawned(ushort toClientId)
    {
        NetworkManager.Instance.Server.Send(AddSpawnData(Message.Create(MessageSendMode.Reliable, ServerToClientId.PlayerSpawned)), toClientId);
    }
    

    /// <summary>
    /// Add id, username, position to Message
    /// </summary>
    /// <param name="message">object need add data</param>
    /// <returns></returns>
    private Message AddSpawnData(Message message)
    {
        message.AddUShort(Id);
        message.AddString(Username);
        message.AddVector3(transform.position);
        return message;
    }
    
    #endregion
}