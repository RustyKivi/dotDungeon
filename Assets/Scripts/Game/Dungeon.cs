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

    public virtual void Load(int[] seed)
    {
        Debug.Log("Loading dungeon...");
        LoadRoomClient(seed);
    }
    private void LoadRoomClient(int[] _roomIndices)
    {
        GameObject spawnedSpawnRoom = Instantiate(spawnRoom, transform.position, Quaternion.identity);
        Transform currentRoomHook = spawnedSpawnRoom.GetComponent<Room>().roomHook;
        if (_roomIndices == null || _roomIndices.Length == 0)
        {
            return;
        }
        else
        {
            foreach (int roomIndex in _roomIndices)
            {
                if (roomIndex >= 0 && roomIndex < roomPrefabs.Length)
                {
                    GameObject roomPrefab = roomPrefabs[roomIndex];
                    GameObject spawnedRoom = Instantiate(roomPrefab, currentRoomHook.position, Quaternion.identity);
                    Transform spawnedRoomHook = spawnedRoom.GetComponent<Room>().roomHook;

                    currentRoomHook = spawnedRoomHook;
                }
                else
                {
                    Debug.LogError("Error: Invalid room index in _roomIndices array.");
                }
            }
        }
    }

}
