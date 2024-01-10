using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Transform roomHook;
    [Header("Config")]
    [ReadOnly]
    public int enimesLeft;
    public Transform[] spawners;
    public bool isOpen = false;
    public bool isCompleted = false;

    private void Start() {
        if(spawners != null)
        {
            enimesLeft = spawners.Length;
        }else{return;}
    }
}
