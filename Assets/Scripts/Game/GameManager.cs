using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Space]
    public Dungeon[] dungeonList;
    public Dungeon selectedDungeon;
    public int[] currentSeed;

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
    

    private bool isGameActive = false;
    private int activeMinutes = 0;

    private void Awake()
    {
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
            NetworkManager.Singleton.SceneManager.LoadScene("GamePlay", LoadSceneMode.Single);
            MakeSeed();
        }
    }
    public void MakeSeed()
    {
        if (selectedDungeon != null)
        {
            int amount = selectedDungeon.maxLoadedRooms;

            if (amount <= 0)
            {
                Debug.LogError("Amount should be greater than zero.");
                return;
            }
            int[] roomIndexes = new int[amount];
            for (int i = 0; i < amount; i++)
            {
                int randomIndex = UnityEngine.Random.Range(0, selectedDungeon.roomPrefabs.Length);

                roomIndexes[i] = randomIndex;
            }
            Debug.Log("Random Room Indices: " + string.Join(", ", roomIndexes));
            NetworkTransmission.instance.StartGameServerRPC(roomIndexes);
        }
    }
}