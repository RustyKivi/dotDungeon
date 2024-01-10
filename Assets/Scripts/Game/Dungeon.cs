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
        if(NetworkManager.Singleton.IsHost)
        {
            LoadRoomHost(maxLoadedRooms);
        }
    }

    private void LoadRoomHost(int amount)
    {
        int[] spawnedRoomIndices = new int[amount];
        if (amount <= 0)
        {
            Debug.Log("Loading complete.");
            return;
        }

        GameObject spawnedSpawnRoom = Instantiate(spawnRoom, transform.position, Quaternion.identity);
        Transform currentRoomHook = spawnedSpawnRoom.GetComponent<Room>().roomHook;

        for (int i = 0; i < amount; i++)
        {
            GameObject randomRoomPrefab = roomPrefabs[Random.Range(0, roomPrefabs.Length)];
            int prefabIndex = System.Array.IndexOf(roomPrefabs, randomRoomPrefab);

            if (prefabIndex != -1)
            {
                spawnedRoomIndices[i] = prefabIndex;
            }
            else
            {
                Debug.LogError("Error: Spawned room prefab not found in roomPrefabs array.");
            }

            GameObject spawnedRoom = Instantiate(randomRoomPrefab, currentRoomHook.position, Quaternion.identity);
            Transform spawnedRoomHook = spawnedRoom.GetComponent<Room>().roomHook;

            currentRoomHook = spawnedRoomHook;
        }
        Debug.Log("Spawned Room Indices: " + string.Join(", ", spawnedRoomIndices));
    }
    public void LoadRoomClient(int[] _roomIndices)
    {
        if (GameManager.instance.isHost == true || _roomIndices == null || _roomIndices.Length == 0)
        {
            return;
        }
        else
        {
            GameObject spawnedSpawnRoom = Instantiate(spawnRoom, transform.position, Quaternion.identity);
            Transform currentRoomHook = spawnedSpawnRoom.GetComponent<Room>().roomHook;

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
