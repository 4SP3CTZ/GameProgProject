using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Abilities : MonoBehaviour
{
    [Header("Keys")]
    public KeyCode ability1Key = KeyCode.Q;
    public KeyCode ability2Key = KeyCode.W;
    public KeyCode ability3Key = KeyCode.E;

    [Header("Cooldowns")]
    public float ability1Cooldown = 5f;
    public float ability2Cooldown = 7f;
    public float ability3Cooldown = 10f;

    [Header("Mana Costs")]
    public float ability1ManaCost = 30f;
    public float ability2ManaCost = 30f;
    public float ability3ManaCost = 30f;

    [Header("UI")]
    public Image abilityImage1;
    public Image abilityImage2;
    public Image abilityImage3;
    public Text abilityText1;
    public Text abilityText2;
    public Text abilityText3;

    [Header("Indicators")]
    public Canvas ability1Canvas;
    public Image ability1Skillshot;

    public Canvas ability2Canvas;
    public Image ability2RangeIndicator;
    public float maxAbility2Distance = 7f;

    public Canvas ability3Canvas;
    public Image ability3Cone;

    [Header("Ability Projectile")]
    public GameObject abilityProjectilePrefab;
    public Transform projectileSpawnPoint;
    public int ability3ProjectileCount = 12;
    public float ability3SpreadAngle = 120f;
    public float skill3Radius = 4f;
    public float skill3Duration = 2.5f;

    private ManaSystem manaSystem;

    private Ray ray;
    private RaycastHit hit;
    private Vector3 targetPos;

    private float cd1, cd2, cd3;
    private bool onCD1, onCD2, onCD3;

    void Start()
    {
        manaSystem = GetComponent<ManaSystem>();

        ability1Canvas.enabled = false;
        ability2Canvas.enabled = false;
        ability3Canvas.enabled = false;

        ability1Skillshot.enabled = false;
        ability2RangeIndicator.enabled = false;
        ability3Cone.enabled = false;
    }

    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Ability1();
        Ability2();
        Ability3();

        HandleCooldown(ref cd1, ref onCD1, ability1Cooldown, ability1ManaCost, abilityImage1, abilityText1);
        HandleCooldown(ref cd2, ref onCD2, ability2Cooldown, ability2ManaCost, abilityImage2, abilityText2);
        HandleCooldown(ref cd3, ref onCD3, ability3Cooldown, ability3ManaCost, abilityImage3, abilityText3);
    }


    void Ability1()
    {
        if (Input.GetKeyDown(ability1Key) && !onCD1 && manaSystem.CanAffordAbility(ability1ManaCost))
        {
            ability1Canvas.enabled = true;
            ability1Skillshot.enabled = true;
            Cursor.visible = false;
        }

        if (ability1Skillshot.enabled && Physics.Raycast(ray, out hit))
        {
            targetPos = hit.point;
            ability1Canvas.transform.rotation =
                Quaternion.LookRotation(targetPos - transform.position);
        }

        if (ability1Skillshot.enabled && Input.GetMouseButtonDown(0))
        {
            manaSystem.UseAbility(ability1ManaCost);
            onCD1 = true;
            cd1 = ability1Cooldown;

            FireProjectile((targetPos - projectileSpawnPoint.position).normalized);

            ability1Canvas.enabled = false;
            ability1Skillshot.enabled = false;
            Cursor.visible = true;
        }
    }

    void Ability2()
    {
        if (Input.GetKeyDown(ability2Key) && !onCD2 && manaSystem.CanAffordAbility(ability2ManaCost))
        {
            ability2Canvas.enabled = true;
            ability2RangeIndicator.enabled = true;
            Cursor.visible = false;
        }

        if (ability2Canvas.enabled && Physics.Raycast(ray, out hit))
        {
            Vector3 dir = (hit.point - transform.position).normalized;
            float dist = Mathf.Min(Vector3.Distance(hit.point, transform.position), maxAbility2Distance);
            ability2Canvas.transform.position = transform.position + dir * dist;
        }

        if (ability2Canvas.enabled && Input.GetMouseButtonDown(0))
        {
            manaSystem.UseAbility(ability2ManaCost);
            onCD2 = true;
            cd2 = ability2Cooldown;

            Vector3 spawnPos = ability2Canvas.transform.position + Vector3.up * 6f;

            GameObject proj = Instantiate(abilityProjectilePrefab, spawnPos, Quaternion.identity);
            proj.GetComponent<AbilityProjectile>().Init(Vector3.down, gameObject);

            ability2Canvas.enabled = false;
            ability2RangeIndicator.enabled = false;
            Cursor.visible = true;
        }
    }


    void Ability3()
    {
        if (Input.GetKeyDown(ability3Key) && !onCD3 && manaSystem.CanAffordAbility(ability3ManaCost))
        {
            ability3Canvas.enabled = true;
            ability3Cone.enabled = true;
            Cursor.visible = false;
        }

        if (ability3Cone.enabled && Physics.Raycast(ray, out hit))
        {
            targetPos = hit.point;
            ability3Canvas.transform.rotation =
                Quaternion.LookRotation(targetPos - transform.position);
        }

        if (ability3Cone.enabled && Input.GetMouseButtonDown(0))
        {
            manaSystem.UseAbility(ability3ManaCost);
            onCD3 = true;
            cd3 = ability3Cooldown;

            Collider[] hits = Physics.OverlapSphere(
            transform.position,
            skill3Radius
            );

            foreach (Collider hitEnemy in hits)
            {
                if (!hitEnemy.CompareTag("Enemy")) continue;

                EnemyCombat enemy = hitEnemy.GetComponent<EnemyCombat>();
                if (enemy != null)
                {
                    StartCoroutine(RootEnemy(enemy));
                }
            }
            ability3Canvas.enabled = false;
            ability3Cone.enabled = false;
            Cursor.visible = true;
        }
    }

    IEnumerator RootEnemy(EnemyCombat enemy)
    {
        enemy.SetRooted(true);
        yield return new WaitForSeconds(skill3Duration);
        enemy.SetRooted(false);
    }

   
    void FireProjectile(Vector3 dir)
    {
        GameObject proj = Instantiate(
            abilityProjectilePrefab,
            projectileSpawnPoint.position,
            Quaternion.LookRotation(dir)
        );

        proj.GetComponent<AbilityProjectile>().Init(dir, gameObject);
    }


    void HandleCooldown(ref float current, ref bool active, float max, float mana, Image img, Text txt)
    {
        if (active)
        {
            current -= Time.deltaTime;
            if (current <= 0)
            {
                active = false;
                current = 0;
            }
            img.fillAmount = 1;
            txt.text = Mathf.Ceil(current).ToString();
        }
        else
        {
            img.fillAmount = manaSystem.CanAffordAbility(mana) ? 0 : 1;
            txt.text = manaSystem.CanAffordAbility(mana) ? "" : "X";
        }
    }
}
