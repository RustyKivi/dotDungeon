using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sten : MonoBehaviour
{
    public GameObject StartButton;
    private GameObject playerObject;

    private void Start()
    {
        ChatManager.instance.chatPanel.SetActive(false);
        ChatManager.instance.chatInput.gameObject.SetActive(false);
    }

    public void StartLobby(){
        this.gameObject.SetActive(false);
        Server.instance.StartHost(4);
        Destroy(playerObject);
        ChatManager.instance.chatPanel.SetActive(true);
        ChatManager.instance.chatInput.gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            playerObject = other.gameObject;
            StartButton.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            StartButton.SetActive(false);
        }
    }
}
