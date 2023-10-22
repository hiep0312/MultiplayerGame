using System;
using System.Collections.Generic;
using Riptide;
using UnityEngine;
using Random = System.Random;

public class Player : MonoBehaviour
{
    // List player in server
    public static Dictionary<ushort, Player> list = new Dictionary<ushort, Player>();

    public ushort Id { get; private set; }
    public bool IsLocal { get; private set; }

    private string username;
    
    private void OnDestroy()
    {
        list.Remove(Id);
    }

    public static void Spawn(ushort id, string username, Vector3 position)
    {
        Player player;
        if (id == NetworkManager.Instance.Client.Id)
        {
            player = Instantiate(GameLogic.Instance.LocalPlayerPrefab, position, Quaternion.identity).GetComponent<Player>();
            player.IsLocal = true;
        }
        else
        {
            player = Instantiate(GameLogic.Instance.PlayerPrefab, position, Quaternion.identity).GetComponent<Player>();
            player.IsLocal = false;
        }
        
        player.name = $"Player {id} (username)";
        player.Id = id;
        player.username = username;
        
        list.Add(id, player);
    }

    private void Move(Vector3 newPos)
    {
        transform.position = newPos;
    }

    #region Messages

    [MessageHandler((ushort)ServerToClientId.PlayerSpawned)]
    private static void SpawnPlayer(Message message)
    {
        Spawn(message.GetUShort(), message.GetString(), message.GetVector3());
    }
    
    [MessageHandler((ushort)ServerToClientId.PlayerMovement)]
    private static void PlayerMovement(Message message)
    {
        if (list.TryGetValue(message.GetUShort(), out Player player))
            player.Move(message.GetVector3());
    }

    #endregion
}