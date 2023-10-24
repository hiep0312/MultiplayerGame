using System;
using System.Collections;
using System.Collections.Generic;
using Riptide;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Player _player;
    
    private bool input;

    private void Start()
    {
        _player = GetComponent<Player>();
    }

    private void Update()
    {
        if (!_player.IsAlive) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            input = true;
            SendInput();
            input = false;
        }
    }
    

    #region Messages

    private void SendInput()
    {
        Message message = Message.Create(MessageSendMode.Unreliable, ClientToServerId.Input);

        message.AddBool(input);
        NetworkManager.Instance.Client.Send(message);
    }

    #endregion
    
    
}
