using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceLightning : LockedOn {

    public int damage = 1;

	void Awake() 
	{
		type = SpellType.Electricity;
	}

    public override void Cast(PlayerStats player) 
	{
        base.Cast(player);

        // TODO
        // make electricity go to the rigidbody by having the particles
        // go that way.
        
        // make the enemy take damage
        e.takeDamage(damage);

        Destroy(gameObject, 0f);
    }
}
