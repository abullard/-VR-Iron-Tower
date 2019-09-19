using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpellType {Fire, Ice, Electricity, Shield, Buff, Debuff, LockedOn};

public class Spell : MonoBehaviour 
{
    public int dmg;
    public int manaCost;

	public SpellType type;

	private AudioSource spellAudio;

    public Rigidbody spellobjectRB;

    // Base Spell class, all spells must play their sound 
    // effect, and subtract their mana cost
    public virtual void Cast(PlayerStats player)
    {
        spellobjectRB = GetComponent<Rigidbody>();

        // if the player has no mana, we cannot cast a spell
        if (!player.HasMana()) 
		{
			// TODO - how will we alert the player we don't have mana?
			// Play audio to alert the player
			// ideas: 
				// Credit card left in machine too long sound
				// halflife 2 healing sound
				// something that actually makes sense sound
		} 
		else 
		{
            // 1. play sound effect
            spellAudio = GetComponent<AudioSource>();
			if (spellAudio)
			{
                spellAudio.Play();
			}

			// 2. subtract mana cost
			player.LoseMana(manaCost);
		}
    }
}
