using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedOn : Spell {

	private AudioSource SpellFailedAudio;

	public Enemy e;

    public Rigidbody erb;

    // implementation for casting a LockedOn Spell
    public override void Cast(PlayerStats player)
	{
        if (player.isLockedOn()) {
            // play audio and subtract mana cost
            base.Cast(player);

            // get the locked on enemy and then retrieve their rigidbody
            e = player.GetLockedEnemy();
            erb = e.GetComponent<Rigidbody>();

        } else {
            SpellFailedAudio = GetComponent<AudioSource>();
			if (SpellFailedAudio)
			{
                // play audio to let player know that the spell can't be cast.
                SpellFailedAudio.Play();
			}
		}
	}
}