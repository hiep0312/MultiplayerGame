using System;
using System.Collections;
using System.Collections.Generic;
using Riptide;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool input;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            input = true;
        }
    }

    private void FixedUpdate()
    {
        SendInput();

        input = false;
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
