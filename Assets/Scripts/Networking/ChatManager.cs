using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatManager : MonoBehaviour
{
    public static ChatManager instance;

    [SerializeField] private int maxMessages = 20;

    [Header("GUI Element")]
    [SerializeField] public GameObject chatPanel;
    [SerializeField] private GameObject textObject;
    [SerializeField] public TMP_InputField chatInput;

    private List<Message> messageList = new List<Message>();

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    private void Update()
    {
        if(chatInput.text != "")
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (chatInput.text == " ")
                {
                    chatInput.text = "";
                    chatInput.DeactivateInputField();
                    return;
                }
                NetworkTransmission.instance.IWishToSendAChatServerRPC(chatInput.text, GameManager.instance.myClientId);
                chatInput.text = "";
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                chatInput.ActivateInputField();
                chatInput.text = " ";
            }
        }
    }

    public class Message
    {
        public string text;
        public TMP_Text textObject;
    }

    public void SendMessageToChat(string _text, ulong _fromwho, bool _server)
    {
        if(messageList.Count >= maxMessages)
        {
            Destroy(messageList[0].textObject.gameObject);
            messageList.Remove(messageList[0]);
        }
        Message newMessage = new Message();
        string _name = "Server";

        if (!_server)
        {
            if (GameManager.instance.playerInfo.ContainsKey(_fromwho))
            {
                _name = GameManager.instance.playerInfo[_fromwho].GetComponent<PlayerInfo>().steamName;
            }
        }

        newMessage.text = _name + ": " + _text;

        GameObject newText = Instantiate(textObject, chatPanel.transform);
        newMessage.textObject = newText.GetComponent<TMP_Text>();
        newMessage.textObject.text = newMessage.text;

        messageList.Add(newMessage);
    }

    public void ClearChat()
    {
        messageList.Clear();
        GameObject[] chat = GameObject.FindGameObjectsWithTag("ChatMessage");
        foreach(GameObject chit in chat)
        {
            Destroy(chit);
        }
        Debug.Log("clearing chat");
    }
}
