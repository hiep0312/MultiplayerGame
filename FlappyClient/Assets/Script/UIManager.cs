using Riptide;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [Header("Connect")]
    [SerializeField] private GameObject connectUI;
    [SerializeField] private InputField usernameField;
    
    public void Connect()
    {
        usernameField.interactable = false;
        connectUI.SetActive(false);
        
        NetworkManager.Instance.Connect();
    }
    
    public void BackToMain()
    {
        usernameField.interactable = true;
        connectUI.SetActive(true);
    }

    public void SendName()
    {
        Message message = Message.Create(MessageSendMode.Reliable, ClientToServerId.Name);
        message.AddString(usernameField.text);
        NetworkManager.Instance.Client.Send(message);
    }
}