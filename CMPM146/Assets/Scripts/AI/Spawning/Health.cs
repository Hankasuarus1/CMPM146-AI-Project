using System;
using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class Health : MonoBehaviour
{
    [Header("Stats")]
    [Tooltip("Max HP for this enemy. Keep small while tuning.")]
    [SerializeField] private int maxHP = 10;
    public float dodgeDistance = 3f;
    public float dodgeSpeed = 10f;

    private int hp;
    private bool dead; // guard against double-death

    // Spawner subscribes to this so we don't have to manually hook anything up per enemy.
    public event Action<Health> OnDeath;

    private void Awake()
    {
        hp = Mathf.Max(1, maxHP);
        dead = false;
    }

    // I call this from damage sources (bullets, traps, etc.).
    public void TakeDamage(int dmg)
    {
        if (dead) return; // already handled
        hp -= Mathf.Max(1, dmg);
        if (hp <= 0) Die();
        DodgeRandom();
    }

    public void DodgeRandom() => StartCoroutine(DodgeRoutine(UnityEngine.Random.value < 0.5f ? -transform.right : transform.right));

    // Centralized death so the spawner always hears about it.
    public void Die()
    {
        if (dead) return;
        dead = true;
        try { OnDeath?.Invoke(this); } catch { /* keeping this safe */ }
        Destroy(gameObject);
    }
    
    IEnumerator DodgeRoutine(Vector3 dir)
    {
        Vector3 start = transform.position;
        Vector3 target = start + dir.normalized * dodgeDistance;

        while ((target - transform.position).sqrMagnitude > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, dodgeSpeed * Time.deltaTime);
            yield return null;
        }
    }

    // Handy for quick checks in other scripts if needed.
    public bool IsDead => dead;
}
