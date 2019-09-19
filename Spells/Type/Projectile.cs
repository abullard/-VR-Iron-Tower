using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Spell {

	public float force;

	private float lifetime = 2.0f;
	private float initTime = 0.0f;

	void Update() {
		// if the projectile hasn't hit anything we need to destroy it
		if(Time.time > initTime + lifetime) {
			Destroy(gameObject, 0.0f);
		}
	}

	// implementation for casting a projectile
	public override void Cast(PlayerStats player)
	{
		base.Cast(player);

		// when was the fireball cast?
		initTime = Time.time;

        // push the spell forward from the players location
        spellobjectRB.velocity = transform.forward * force;
	}
}

