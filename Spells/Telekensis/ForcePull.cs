using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForcePull : LockedOn {

	public float thrust = 1.0f;
 
	void Awake() 
	{
		type = SpellType.LockedOn;
	}

	void Cast(PlayerStats player) 
	{
        Vector3 towardsPlayer = player.transform.position - e.transform.position;

		base.Cast(player);

        // move the enemy towards the player
        erb.AddForce(towardsPlayer * thrust);
    }
}
