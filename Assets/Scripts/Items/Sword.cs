using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : PlayerItem
{
    public Animator ani;
    [SerializeField] private float cooldownTime = 5f;
    private bool isUsed;
    public override void Trigger()
    {
        base.Trigger();
        if (!isUsed)
        {
            ani.SetTrigger("wp_swing");
            StartCoroutine(StartCooldown());
        }
    }
    

    private IEnumerator StartCooldown()
    {
        isUsed = true;
        yield return new WaitForSeconds(cooldownTime);
        isUsed = false;
    }
}
