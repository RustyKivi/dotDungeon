using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Steamworks;
using Unity.Collections;

public class PlayerInfo : MonoBehaviour
{
    /*
        #Steam
        All steam info som kan va bra att veta, hjälper också med att idifera vem som är vem.

        #Data
        Idifera vilken class/role spelaren är. Hälper också till att säga vilken level spelaren är.

        #Lobby
        Detta kommer bara andvändas under lobby tid, hjälper till att vissa vem som är vem och säger om spelaren är redo.
    */

    [Header("Steam")]
    public string steamName;
    [ReadOnly]
    public ulong steamId;


    [Header("Data")]
    public int level = 1;
    public string role;
    [Space(5)]
    public int level_phy = 1;
    public int level_mgc = 1;
    public int level_def = 1;

    [Header("Lobby")]
    [SerializeField] private TMP_Text playerName;
    [SerializeField] private TMP_Text playerLevel;
    [SerializeField] private TMP_Text playerClass;
    public GameObject readyImage;
    public bool isReady;

    private void Start()
    {
        readyImage.SetActive(false);
        playerName.text = steamName;
        playerClass.text = role;
        playerLevel.text = "( " + level.ToString() + " )";
    }
}