using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkLoader : MonoBehaviour
{
    public static ChunkLoader instance;

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
    public int ChunksSpawned = 0;
    public Transform ChunkTransform;
    public List<GameObject> ChunkPrefabs = new List<GameObject>();

    public void AskForLoad()
    {
        if (ChunkPrefabs.Count == 0)
        {
            Debug.LogError("No ChunkPrefabs available.");
            return;
        }

        int randomIndex = Random.Range(0, ChunkPrefabs.Count);
        LoadChunk(randomIndex);
    }

    public void LoadChunk(int chunkId)
    {
        if (chunkId < 0 || chunkId >= ChunkPrefabs.Count)
        {
            Debug.LogError("Invalid chunkId provided.");
            return;
        }

        GameObject chunkPrefab = ChunkPrefabs[chunkId];
        if (chunkPrefab != null)
        {
            Vector3 spawnPosition = ChunkTransform.position;
            Instantiate(chunkPrefab, spawnPosition, Quaternion.identity);
            ChunkTransform.localPosition += new Vector3(18f,0,0);
            ChunksSpawned++;
        }
        else
        {
            Debug.LogError("Chunk prefab at index " + chunkId + " is null.");
        }
    }
}