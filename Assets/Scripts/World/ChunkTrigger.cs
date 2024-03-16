using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ChunkTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player" && GameManager.instance.isHost && other.GetComponent<NetworkObject>().IsOwnedByServer == true)
        {
            ChunkLoader.instance.AskForLoad();
            Destroy(this.gameObject);
        }
    }
}
