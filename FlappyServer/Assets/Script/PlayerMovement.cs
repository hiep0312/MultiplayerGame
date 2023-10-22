using Riptide;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private CharacterController controller;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpHeight;

    private float gravityAcceleration;
    private float jumpSpeed;

    private bool input;
    private float yVelocity;

    private void OnValidate()
    {
        if (controller == null)
            controller = GetComponent<CharacterController>();
        if (player == null)
            player = GetComponent<Player>();

        Initialize();
    }

    private void Start()
    {
        Initialize();
    }

    private void FixedUpdate()
    {
        Move(input);
    }

    private void Initialize()
    {
        gravityAcceleration = gravity * Time.fixedDeltaTime * Time.fixedDeltaTime;
        jumpSpeed = Mathf.Sqrt(jumpHeight * -2f * gravityAcceleration);
    }

    private void Move(bool jump)
    {
        Vector3 moveDirection = Vector3.zero;

        if (input)
        {
            input = false;
            yVelocity = 0f;
            if (jump)
                yVelocity = jumpSpeed;
        }
        
        yVelocity += gravityAcceleration;

        moveDirection.y = yVelocity;
        controller.Move(moveDirection);

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