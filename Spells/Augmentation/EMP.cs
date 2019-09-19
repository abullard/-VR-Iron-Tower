using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMP : Projectile {

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
			// disable the robot (there aren't any organic monsters for our game)
			// e.EmpDisable();
		}

		Destroy(gameObject, 0f);
	}
   
}
