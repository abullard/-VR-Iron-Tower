using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Haste : Buff {
	
	public float duration = 1.0f; 
	private float startTime = 0.0f;

    private PlayerStats stats;

	void Awake()
	{
		type = SpellType.Buff;
	}

	void Update() 
	{

		// if the duration is over, return the spell casting speed
		// to normal, and destroy the spell
		if(Time.deltaTime > (startTime + duration))
		{
			stats.SetSpellSpeed(stats.spellCooldown);
			Destroy(gameObject, 0.0f);
		}
	}

	// implementation for casting a spell
	public override void Cast(PlayerStats player)
	{
        stats = player;

		base.Cast(player);

		// 1. start duration of spell
		startTime = Time.time;

		// 2. increase player spell casting speed
		player.SetSpellSpeed(stats.quickSpellCooldown);
	}
}
