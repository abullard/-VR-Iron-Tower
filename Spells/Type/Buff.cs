using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : Spell {

    private PlayerStats stats;

	void Awake() 
	{
		type = SpellType.Buff;
	}

	void Update() 
	{
		// if the player is moving while the spell is being cast, 
		// keep it's position on them
		transform.position = stats.transform.position;
	}

	// implementation for casting a LockedOn Spell
	public override void Cast(PlayerStats player)
	{
        stats = player;

		// play audio and subtract mana cost
		base.Cast(player);
	}
}