using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement)), RequireComponent(typeof(Stats))]
public class RangedCombat : MonoBehaviour
{
    private Movement moveScript;
    private Stats stats;
    private Animator anim;

    [Header("Target")]
    public GameObject targetEnemy;

    [Header("Ranged Attack Variables")]
    public bool performRangedAttack = true;
    private float attackInterval;
    private float nextAttackTime = 0    ;

    [Header("Ranged Projectile Variables")]
    public GameObject attackProjectile;
    public Transform attackSpawnPoint;
    private GameObject spawnedProjectile;

    void Start()
    {
        moveScript = GetComponent<Movement>();
        stats = GetComponent<Stats>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        attackInterval = stats.attackSpeed / ((500 + stats.attackSpeed) * 0.01f);

        targetEnemy = moveScript.targetEnemy;

        if (targetEnemy != null && performRangedAttack && Time.time > nextAttackTime)
        {
            if (Vector3.Distance(transform.position, targetEnemy.transform.position) <= moveScript.stoppingDistance)
            {
                StartCoroutine(RangedAttackInverval());
            }
        }
    }
    private IEnumerator RangedAttackInverval()
    {
        performRangedAttack = false;

        anim.SetBool("isAttacking", true);

        yield return new WaitForSeconds(attackInterval);

        if (targetEnemy == null)
        {
            anim.SetBool("isAttacking", false);
            performRangedAttack = true;
        }
    }
    private void RangedAttack()
    {
        spawnedProjectile = Instantiate(attackProjectile, attackSpawnPoint.transform.position, attackSpawnPoint.transform.rotation);

        TargetEnemy targetEnemyScript = spawnedProjectile.GetComponent<TargetEnemy>();

        if(targetEnemyScript != null)
        {
            targetEnemyScript.SetTarget(targetEnemy.transform);
        }

        nextAttackTime = Time.time + attackInterval;
        performRangedAttack = true;

        anim.SetBool("isAttacking", false);
    }
}
