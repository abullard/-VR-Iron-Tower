using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflector : Shield
{
    private PlayerStats stats;
    public float duration = 1.0f;
    private float startTime = 0.0f;

    private void Update()
    {
        transform.position = stats.transform.position;

        // reflector only lasts for a short amount of time
        if(Time.time > startTime + duration)
        {
            Destroy(gameObject, 0.0f);
        }
    }

    public override void Cast(PlayerStats player)
    {
        stats = player;
        base.Cast(player);
        startTime = Time.time;
    }

    private void OnCollisionEnter(Collision col)
    {
        // if col is of type enemy projectile
        EnemyProjectile proj = col.gameObject.GetComponent<EnemyProjectile>();
        // reflect it back at the enemy
        if(proj)
        {
            proj.Reflect();
        }
    }
}
