﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shockwave : Projectile
{
    void Awake()
    {
		type = SpellType.Electricity;
    }

    // implementation for spell collision
    void OnCollisionEnter(Collision col)
    {
        Enemy e = col.gameObject.GetComponent<Enemy>();
     
        if (e)
        {
            e.takeDamage(dmg);
        }

        Destroy(gameObject, 0f);
    }
}
