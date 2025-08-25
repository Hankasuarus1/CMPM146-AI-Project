using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Stats")]
    public float health;
    public float damage;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeDamage(float x)
    {
        health -= x;
        if (health <= 0)
            die();
    }

    void die()
    {
        Destroy(gameObject);
    }
}
