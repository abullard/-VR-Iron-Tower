using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongShield : Shield
{
    private PlayerStats stats;
    public int health = 300;

    private void Update()
    {
        transform.position = stats.transform.position;
    }

    public override void Cast(PlayerStats player)
    {
        stats = player;
        base.Cast(player);
        SetHealth(health);
    }

    private void OnCollisionEnter(Collision col)
    {
        // if col is of type enemy projectile
        EnemyProjectile proj = col.gameObject.GetComponent<EnemyProjectile>();

        if (proj)
        {
            TakeDamage(proj.damageValue);
        }
    }
}
