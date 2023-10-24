using System;
using Riptide;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Player player;
    
    public float jumpSpeed;

    private bool input;

    private void OnValidate()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
        
        if (player == null)
            player = GetComponent<Player>();
    }

    private void Update()
    {
        if (input)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
            input = false;
        }
    }

    private void FixedUpdate()
    {
        SendMovement();
    }

    public void SetInput(bool input)
    {
        this.input = input;
    }

    private void SendMovement()
    {
        Message message = Message.Create(MessageSendMode.Reliable, ServerToClientId.PlayerMovement);
        message.AddUShort(player.Id);
        message.AddVector3(transform.position);
        NetworkManager.Instance.Server.SendToAll(message);
    }
}