using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : Buff {

	public int healAmount = 50;

    private PlayerStats stats;

	void Awake()
	{
		type = SpellType.Buff;
	}

    private void Update()
    {
        transform.position = stats.transform.position;
    }

    // implementation for casting a spell
    public override void Cast(PlayerStats player)
	{
        stats = player;

		base.Cast(player);

		// regen some health to the player
		player.ChangeHealth(healAmount);

		// kill the gameObject, it's served its purpose.
		Destroy(gameObject, 0.0f);
	}
}
