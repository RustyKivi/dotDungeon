using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NonePlayer : MonoBehaviour
{
    public int Health;
    public float WalkSpeed;

    private NavMeshAgent agent;

    private void Start() {
        agent = GetComponent<NavMeshAgent>();
    }
}
