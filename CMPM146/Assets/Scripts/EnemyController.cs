using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("Stats")]
    public float damage;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField]private Transform target;
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        chase();
    }

    void chase()
    {
        agent.SetDestination(target.position);
    }
}
