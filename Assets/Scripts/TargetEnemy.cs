using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetEnemy : MonoBehaviour
{
    public Transform target;
    private Transform originalTarget;
    private Rigidbody theRB;

    private Stats playerStats;

    public float projectileSpeed;

    void Start()
    {
        originalTarget = target;
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<Stats>();
        theRB = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (target != null)
        {
            Vector3 direction = target.position - transform.position;
            theRB.velocity = direction.normalized * projectileSpeed;
        }
        else if (originalTarget != null)
        {
            Vector3 direction = originalTarget.position - transform.position;
            theRB.velocity = direction.normalized * projectileSpeed;
        }
        else {
            Destroy(gameObject);
        }
    }
    
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (target != null && ReferenceEquals(other.gameObject, target.gameObject))
        {
            Stats targetStats = target.gameObject.GetComponent<Stats>();
            targetStats?.TakeDamage(target.gameObject, playerStats.damage);
            Destroy(gameObject);
        }
        else if (originalTarget != null && ReferenceEquals(other.gameObject, originalTarget.gameObject))
        {
            Stats originalTargetStats = originalTarget.gameObject.GetComponent<Stats>();
            originalTargetStats?.TakeDamage(originalTarget.gameObject, playerStats.damage);
            Destroy(gameObject);
        }   
    }
}
