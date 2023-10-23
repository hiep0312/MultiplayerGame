using System;
using System.Collections.Generic;
using Riptide;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public static Dictionary<ushort, Wall> list = new Dictionary<ushort, Wall>();
    
    public ushort ID { get; set; }

    public static void Spawn(ushort id, Vector3 pos)
    {
        Wall wall = Instantiate(GameLogic.Instance.WallPrefab, pos, Quaternion.identity).GetComponent<Wall>();
        wall.ID = id;
        wall.name = $"Wall id: {id}";
        
        list.Add(id, wall);
    }

    public void Move(Vector3 pos)
    {
        transform.position = pos;
    }

    public void WallDestroy()
    {
        list.Remove(ID);
        Destroy(gameObject);
    }
    
    #region Messages Handler
    
    [MessageHandler((ushort)ServerToClientId.WallSpawned)]
    private static void OnWallSpawn(Message message)
    {
        ushort id = message.GetUShort();
        Vector3 pos = message.GetVector3();
        Debug.Log("Spawner " + id + " " + pos);
        Spawn(id, pos);
    }
    

    [MessageHandler((ushort)ServerToClientId.WallMovement)]
    private static void OnMove(Message message)
    {
        if (list.TryGetValue(message.GetUShort(), out Wall wall))
        {
            wall.Move(message.GetVector3());
        }
    }

    [MessageHandler((ushort)ServerToClientId.WallDestroyed)]
    private static void DestroyWall(Message message)
    {
        if (list.TryGetValue(message.GetUShort(), out Wall wall))
        {
            wall.WallDestroy();
        }
    }

    #endregion
}