using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alcohol : PlayerItem
{
    [SerializeField] private float cooldownTime = 5f;
    private bool isUsed;
    private NetworkPlayer player;
    public override void Trigger()
    {
        base.Trigger();
        if (!isUsed)
        {
            player = GetComponentInParent<NetworkPlayer>();
            if (player != null)
            {
                player.Heal(Damage);
                StartCoroutine(StartCooldown());
            }
        }
        else
        {
            Debug.LogError("NetworkPlayer component not found in parent object.");
        }
    }

    private IEnumerator StartCooldown()
    {
        isUsed = true;
        yield return new WaitForSeconds(cooldownTime);
        isUsed = false;
    }
}
