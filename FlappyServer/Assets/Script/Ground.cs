using System;
using System.Collections;
using System.Collections.Generic;
using Riptide;
using UnityEngine;

public class Ground : MonoBehaviour
{
    public float speed;
    private Vector3 _startPos;

    private void Start()
    {
        _startPos = transform.position;
    }

    private void Update()
    {
        transform.position -= speed * Time.deltaTime * Vector3.right;
        if (transform.position.x < 0) transform.position = _startPos;
    }

    private void FixedUpdate()
    {
        SendGroundPosition();
    }

    private void SendGroundPosition()
    {
        Message message = Message.Create(MessageSendMode.Unreliable, ServerToClientId.GroundMove);
        message.AddVector3(transform.position);
        NetworkManager.Instance.Server.SendToAll(message);
    }
}
