using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using Netcode.Transports.Facepunch;
using Steamworks;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Space]
    public Dungeon[] dungeonList;
    public Dungeon selectedDungeon;
    public int[] currentSeed;
    public string[] roleOptions;

    public Dictionary<ulong, GameObject> playerInfo = new Dictionary<ulong, GameObject>();
    [Header("Networking")]
    public bool connected;
    public bool inGame;
    public bool isHost;
    public ulong myClientId;

    [Header("MyData")]
    public int myLevel = 1;
    public string myClass;
    [Space(5)]
    public int myMagicLevel = 1;
    public int myPhysicLevel = 1;
    public int myDefendLevel = 1;

    [Header("GUI")]
    public TMP_Text client_name_text;
    public TMP_Text client_level_text;
    public TMP_Text client_health_text;
    public UnityEngine.UI.Slider client_health_slider;
    [Space]
    public TMP_Dropdown dropdown;
    public GameObject loadScreen;

    private bool isGameActive = false;
    private int activeMinutes = 0;

    private void Awake()
    {
        dropdown.AddOptions(new List<string>(roleOptions));
        dropdown.value = Array.IndexOf(roleOptions, myClass);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Duplicate instance of GameConsole. Destroying the new one.");
            Destroy(gameObject);
        }
    }
    private void Start() {
        dropdown.onValueChanged.AddListener(delegate {
            ChangeClass(dropdown);
        });
        UpdateClientGUI();
    }

    private void Update()
    {
        if (isGameActive)
        {
            activeMinutes = Mathf.FloorToInt(Time.time / 60f);
        }
    }

    public void SwitchGameMode(int index)
    {
        selectedDungeon = dungeonList[index];
        GameConsole.instance.Output("Current Game Mode: " + selectedDungeon.GetType().Name.ToString());
        Debug.Log("Selected d: " + selectedDungeon.GetType().Name.ToString());
        if(LobbyManager.instance.modeDisplay == null)return;
        LobbyManager.instance.modeDisplay.text = "< " + selectedDungeon.GetType().Name.ToString() + " >";
    }

    public void StartCurrentGameMode()
    {
        if (selectedDungeon != null)
        {
            NetworkTransmission.instance.StartGameServerRPC();
            NetworkManager.Singleton.SceneManager.LoadScene("GamePlay", LoadSceneMode.Single);
        }
    }
    public void UpdateClientGUI()
    {
        client_name_text.text = SteamClient.Name;
        client_level_text.text = "Level: " + myLevel.ToString();
        client_health_text.text = "100/100";
        client_health_slider.maxValue = 100;
        client_health_slider.value = 100;
    }
    public void ChangeClass(TMP_Dropdown change)
    {
        myClass = roleOptions[change.value];
        if(connected == true)
        {
            NetworkTransmission.instance.UpdatePlayerServerRPC(SteamClient.SteamId,myClass);
        }
    }
}