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
    [Space]
    public Transform Target;

    private NavMeshAgent agent;

    private void Start() {
        agent = GetComponent<NavMeshAgent>();
        if(Nametag != null){Nametag.text = this.name;}
        if(HealthBar != null){HealthBar.maxValue = Health; HealthBar.value = Health;}
        agent.speed = WalkSpeed;
    }
    private void Update() {
        agent.destination = Target.position;
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
