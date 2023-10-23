using System;
using System.Collections;
using System.Collections.Generic;
using Riptide;
using UnityEngine;

public class Ground : MonoBehaviour
{
    private static Ground Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    [MessageHandler((ushort)ServerToClientId.GroundMove)]
    private static void GroundMove(Message message)
    {
        Instance.SetPosition(message.GetVector3());
    }
}
