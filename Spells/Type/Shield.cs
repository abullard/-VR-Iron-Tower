using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Spell {

    public int shieldHealth = 0;

	void Awake() 
	{
		type = SpellType.Shield;
	}

	// implementation for casting a shield spell
	public override void Cast(PlayerStats player)
	{
		base.Cast(player);
	}

	// subtract from the leftover health of the shield
	public void TakeDamage(int value)
	{
		shieldHealth -= value;

		if(shieldHealth < 0) 
		{
			Destroy(gameObject, 0.0f);
		}
	}

    public void SetHealth(int value)
    {
        shieldHealth = value;
    }

}
