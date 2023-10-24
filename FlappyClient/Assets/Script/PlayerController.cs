using System;
using System.Collections;
using System.Collections.Generic;
using Riptide;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool input;

    private void Start()
    {
        this.RegisterListener(EventID.Input, Jump);
    }

    private void OnDestroy()
    {
        this.RemoveListener(EventID.Input, Jump);
    }

    public void Jump(object obj)
    {
        input = true;
    }

    private void Update()
    {
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
