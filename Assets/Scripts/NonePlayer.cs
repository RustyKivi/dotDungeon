using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class NonePlayer : MonoBehaviour
{
    public int Health;
    public float WalkSpeed;
    [Header("GUI")]
    public TMP_Text Nametag;
    public Slider HealthBar;

    private NavMeshAgent agent;

    private void Start() {
        agent = GetComponent<NavMeshAgent>();
        if(Nametag != null){Nametag.text = this.name;}
        if(HealthBar != null){HealthBar.maxValue = Health; HealthBar.value = Health;}
    }

    public virtual void Attack()
    {

    }
    public void Damage(int amout)
    {
        if(amout >= Health)
        {
            Destroy(gameObject);
        }else{
            Health -= amout;
            if(HealthBar != null)return;
            HealthBar.value -= amout;
        }
    }
}
