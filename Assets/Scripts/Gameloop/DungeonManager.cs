using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager instance;
    public List<Room> DungeonRooms = new List<Room>();
    private int activeRoomIndex = 0;

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

    private void Start()
    {
        StartDungeon();
    }

    private void StartDungeon()
    {
        if (DungeonRooms.Count > 0)
        {
            StartRoom(DungeonRooms[activeRoomIndex]);
        }
        else
        {
            Debug.Log("No rooms in the dungeon!");
        }
    }

    private void StartRoom(Room room)
    {
        room.SpawnNPCs();
    }

    public void RoomCompleted(Room completedRoom)
    {
        completedRoom.CheckCompletion(); // Check if the completed room is truly complete
        activeRoomIndex++;
        if (activeRoomIndex < DungeonRooms.Count)
        {
            Room nextRoom = DungeonRooms[activeRoomIndex];
            StartRoom(nextRoom);
        }
        else
        {
            Debug.Log("All rooms are complete!");
        }
    }
}