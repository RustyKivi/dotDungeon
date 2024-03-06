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
    public int AttackDamage;
    [Header("GUI")]
    public TMP_Text Nametag;
    public Slider HealthBar;
    [Space]
    public Transform Target;
    public Room room;

    private NavMeshAgent agent;

    private void Start() {
        agent = GetComponent<NavMeshAgent>();
        if(Nametag != null){Nametag.text = this.name;}
        if(HealthBar != null){HealthBar.maxValue = Health; HealthBar.value = Health;}
        agent.speed = WalkSpeed;
        Target = GameObject.FindWithTag("Player").transform;
    }
    private void Update() {
        if(Target == null)return;
        agent.destination = Target.position;
    }

    public virtual void Attack()
    {

    }
    public void Damage(int amount)
    {
        if (amount >= Health)
        {
            if (room != null)
            {
                room.activeNonPlayers--;
                DungeonManager.instance.RoomCompleted(room);
                Destroy(gameObject);
            }
        }
        else
        {
            Health -= amount;
            if (HealthBar != null)
            {
                HealthBar.value -= amount;
            }
        }
    }
}
