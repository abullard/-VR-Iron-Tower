using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : Projectile {

	void Awake()
	{
		type = SpellType.Ice;
	}

	// implementation for spell collision
	void OnCollisionEnter(Collision col)
	{
		Enemy e = col.gameObject.GetComponent<Enemy>();

		if (e)
		{
			// apply damage to the enemy
			e.takeDamage(dmg);
			// slow the enemies movement speed
			e.SlowMovementSpeed();
		}

		Destroy(gameObject, 0.0f);
	}
}
