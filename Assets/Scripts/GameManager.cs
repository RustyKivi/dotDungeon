using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Steam")]
    public ulong MySteamid = 0;
    public string MySteamname = "anonymous";

    [Header("World Config")]
    public string randomData = "";

    [Header("World Objects")]
    public GameObject lobbyDoor;

    [Header("Player Objects")]
    public CameraController playerCamera;
    public GameObject myObject;

    public void StartGame()
    {
        playerCamera.camInit(true);
    }

    public void Init()
    {
        playerCamera.target = myObject.transform;
    }
}
