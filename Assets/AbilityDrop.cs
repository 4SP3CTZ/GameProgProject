using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityDrop : MonoBehaviour
{
    public float damageRadius = 3f;
    public float lifeTime = 0.5f;

    private Stats playerStats;

    public void Init(GameObject player)
    {
        playerStats = player.GetComponent<Stats>();

     
        Collider[] hits = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (Collider hit in hits)
        {
            if (!hit.CompareTag("Enemy")) continue;

            Stats enemyStats = hit.GetComponent<Stats>();
            if (enemyStats != null)
            {
                enemyStats.TakeDamage(hit.gameObject, playerStats.damage);
            }
        }

        Destroy(gameObject, lifeTime);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}