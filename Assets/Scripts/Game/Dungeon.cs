using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class Dungeon : MonoBehaviour
{
    public int difficulty;
    [Space]
    [Range(1,10)]
    public int maxLoadedRooms;
    public GameObject spawnRoom;
    public GameObject[] roomPrefabs;

    public virtual void Load()
    {
        Debug.Log("Loading dungeon...");
    }
}
