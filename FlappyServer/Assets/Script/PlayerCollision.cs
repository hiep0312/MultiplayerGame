using System;
using System.Collections;
using System.Collections.Generic;
using Riptide;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private Player _player;

    private void OnValidate()
    {
        _player = GetComponent<Player>();
    }

    private void Start()
    {
        _player = GetComponent<Player>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            _player.IsAlive = false;
            SendCollideWithWall();
            if (!Player.CheckPlayerRemain())
            {
                GameLogic.IsPlaying = false;
            }
        }
    }

    #region Messages

    private void SendCollideWithWall()
    {
        Message message = Message.Create(MessageSendMode.Reliable, ServerToClientId.CollideWithWall);
        message.AddUShort(_player.Id);
        message.AddBool(_player.IsAlive);
        NetworkManager.Instance.Server.SendToAll(message);
    }

    #endregion
}
