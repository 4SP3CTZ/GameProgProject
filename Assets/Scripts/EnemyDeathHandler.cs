using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathHandler : MonoBehaviour
{
    public int experienceValue = 50;

    public void Die()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            player.SendMessage("GainExperience", experienceValue);
        }

        Destroy(gameObject);
    }
}
