using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public DungeonManager dungeonManager;
    public List<GameObject> NonPlayers = new List<GameObject>();
    public int activeNonPlayers = 0;
    [SerializeField] private Transform spawnPoint;

    public void Initialize(DungeonManager manager)
    {
        dungeonManager = manager;
    }

    public void SpawnNPCs()
    {
        foreach(GameObject nonPlayerPrefab in NonPlayers)
        {
            GameObject npc = Instantiate(nonPlayerPrefab, spawnPoint.position, spawnPoint.rotation);
            activeNonPlayers++;
            npc.GetComponent<NonePlayer>().room = this;
        }
    }

    public void CheckCompletion()
    {
        if (activeNonPlayers == 0)
        {
            Debug.Log("Room complete!");
            dungeonManager.RoomCompleted(this);
        }else{return;}
    }
}
