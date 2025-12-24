using UnityEngine;

public class AbilityProjectile : MonoBehaviour
{
    public float speed = 18f;
    public float lifeTime = 5f;

    private Vector3 direction;
    private Stats playerStats;

    public void Init(Vector3 dir, GameObject player)
    {
        direction = dir.normalized;
        playerStats = player.GetComponent<Stats>();

        Debug.Log("Projectile Init dir: " + direction);

        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;

        Debug.Log("Ability 3 hit: " + other.name);

        Stats enemyStats = other.GetComponent<Stats>();
        if (enemyStats != null)
        {
            enemyStats.TakeDamage(other.gameObject, playerStats.damage);
        }

        Destroy(gameObject);
    }
}