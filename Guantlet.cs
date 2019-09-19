using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Yarn;


public class Guantlet : MonoBehaviour {
    // controller specific stuff
    private SteamVR_TrackedController _controller;
    uint handIndex;
    const uint LEFTCONTROLLER = 1;
    const uint RIGHTCONTROLLER = 2;

	private GauntletUI ani;

	// we need a reference for the player
	public PlayerStats player;

    // start with unarmed and destruction runes selected in the guantlets
    public Rune[] runes;
    private Rune selected; // unarmed rune

    // particle variables for charging spells
    private float charge;
    public ParticleSystem smokeParticles;
    public ParticleSystem chargeParticles;
    ParticleSystem.EmissionModule emissionOne;
    ParticleSystem.EmissionModule emissionTwo;
    ParticleSystem.MainModule mainOne;
    ParticleSystem.MainModule mainTwo;

    // velocity stuff
    public float trailTime = 0.25f;
    Vector3[] trailPos;
    Vector3[] trailForward;
    int frame = 0;

    // variables to track swipe for rune select
    Vector2 initialTouch = new Vector2();
    Vector2 finalTouch = new Vector2();

    float tempX = 0.0f, tempY = 0.0f;

	private AudioSource runeChangeAudio;
    
    private void OnEnable()
    {
        _controller = GetComponent<SteamVR_TrackedController>();
        handIndex = _controller.controllerIndex;

        _controller.TriggerClicked += HandleTriggerClicked;
        _controller.TriggerUnclicked += HandleTriggerUnclicked;
        _controller.PadClicked += HandlePadClicked;
        _controller.PadUnclicked += HandlePadUnclicked;
        _controller.PadTouched += HandlePadTouched;
        _controller.PadUntouched += HandlePadUntouched;
        _controller.Gripped += HandleGripClicked;
        _controller.Ungripped += HandleGripUnclicked;
    }

    private void OnDisable()
    {
        _controller.TriggerClicked -= HandleTriggerClicked;
        _controller.TriggerUnclicked -= HandleTriggerUnclicked;
        _controller.PadClicked -= HandlePadClicked;
        _controller.PadUnclicked -= HandlePadUnclicked;
        _controller.PadTouched -= HandlePadTouched;
        _controller.PadUntouched -= HandlePadUntouched;
        _controller.Gripped -= HandleGripClicked;
        _controller.Ungripped -= HandleGripUnclicked;
    }

    // TODO - REMOVE IF NO IMPLEMENTATION
    private void HandleTriggerClicked(object sender, ClickedEventArgs e)
    {
        if (handIndex == RIGHTCONTROLLER)
        {
            //Debug.Log("Right trigger was pressed.");
        }
        else if (handIndex == LEFTCONTROLLER)
        {
            //Debug.Log("Left trigger was pressed.");
        }
    }

    private void HandleTriggerUnclicked(object sender, ClickedEventArgs e)
    {
        // TODO - balance this charge
        //if (charge > player.spellCooldown)
        if(charge > 0.1f)
        {
            charge = 0;
			CastSpell(getDeltaPos(), getAverageForward());
        }
    }

    // TODO - REMOVE IF NO IMPLEMENTATION 
    private void HandlePadClicked(object sender, ClickedEventArgs e)
    {
        if (handIndex == RIGHTCONTROLLER)
        {
            //Debug.Log("Right pad was clicked.");
        }
        else if (handIndex == LEFTCONTROLLER)
        {
            //Debug.Log("left pad was clicked.");
        }
    }

    // TODO - REMOVE IF NO IMPLEMENTATION
    private void HandlePadUnclicked(object sender, ClickedEventArgs e)
    {
        //Debug.Log("Pad was unclicked");
    }

    private void HandlePadTouched(object sender, ClickedEventArgs e)
    {
        initialTouch.x = e.padX;
        initialTouch.y = e.padY;

        // TODO - zoom out on rune icons
    }

    private void HandlePadUntouched(object sender, ClickedEventArgs e)
    {
        // get the x,y position on the pad (vec2)
        Vector2 finalTouch = new Vector2(tempX, tempY);

        if(Vector2.Distance(initialTouch, finalTouch) > 0.4f)
        {
            UpdateRune(finalTouch);
        }
    }

	// Grip buttons are used to lock onto an enemy, we use a sphere cast
	// to look for an enemy in front of the controller
    private void HandleGripClicked(object sender, ClickedEventArgs e)
    {
		ani.point();

        LockOn enemyLock = GetComponent<LockOn>();
		// we need to hold a reference to the enemy
		Enemy enemy;

		// do the sphere cast to get a lock on
		enemy = enemyLock.GetLock(_controller);

		// if there is an enemy
		if (enemy) {
			// let the player know were locked on, 
			// and give them a reference to the enemy
			player.LockOntoEnemy (true, enemy);
		} else {
			player.LockOntoEnemy (false, null);
		}
    }

    // TODO - REMOVE IF NO IMPLEMENTATION
    private void HandleGripUnclicked(object sender, ClickedEventArgs e)
    {
        //Debug.Log("Grip was unclicked");
    }

    // Use this for initialization
    void Awake ()
    {
        selected = runes[0];

        trailPos = new Vector3[Mathf.RoundToInt(trailTime / Time.fixedDeltaTime)];
        trailForward = new Vector3[Mathf.RoundToInt(trailTime / Time.fixedDeltaTime)];

        for (int i = 0; i < trailPos.Length; i++)
        {
            trailPos[i] = transform.position;
            trailForward[i] = transform.forward;
        }
           
        emissionOne = chargeParticles.emission;
        emissionTwo = smokeParticles.emission;
        mainOne = chargeParticles.main;
        mainTwo = smokeParticles.main;

		runeChangeAudio = GetComponent<AudioSource>();

		ani = GetComponent<GauntletUI>();
		ani.idle();
    }

    void Update () {
        if (_controller.triggerPressed)
        {
			ani.charge();

            if(!selected.aName.Equals("Unarmed"))
            {
                charge += Time.deltaTime / player.spellCooldown;
            }
        }
        else if (!_controller.triggerPressed)
        {
            charge -= Time.deltaTime / player.spellCooldown;
        }

        if (_controller.padTouched)
        {
            tempX = _controller.controllerState.rAxis0.x;
            tempY = _controller.controllerState.rAxis0.y;
        }

        // Spell Charging stuff
        charge = Mathf.Clamp01(charge);
        emissionOne.rateOverTimeMultiplier = 100f * charge;
        emissionTwo.rateOverTimeMultiplier = 100f * charge;        
    }

    void FixedUpdate()
    {
        frame = (frame + 1) % trailPos.Length;
        trailPos[frame] = transform.position;
        trailForward[frame] = transform.forward;
    }
		
    private Vector3 getDeltaPos()
    {
        return trailPos[frame] - trailPos[(frame + 1) % trailPos.Length];
    }
		 
	private Vector3 getAverageForward()
	{
        return (trailForward[frame] + trailForward[(frame + 1) % trailForward.Length]) / 2.0f;
	}

	private void CastSpell(Vector3 deltaPos, Vector3 aveForward)
    {
		ani.cast();

        // normalize the gesture vectors
        deltaPos = Vector3.Normalize(deltaPos);
        aveForward = Vector3.Normalize(aveForward);

		// TODO - probs won't work
		Vector3 offsetY = new Vector3 (0, 1.5f, 0);
        Quaternion horizontalSpells = new Quaternion(0.0f, transform.rotation.y, player.transform.rotation.z, 1.0f);

        // DETERMINE IF SPELL WAS CAST
        // 1. get velocity
        // 2. check if movement vector is in the direcion of a spell being cast
        // 3. cast correct spell
        Spell spell;

        if (Vector3.Dot(deltaPos, -1 * aveForward) > 0.6f)
		{
            // cast pull in spell
			spell = Instantiate(selected.pull_in, transform.position, transform.rotation).GetComponent<Spell>();
			spell.Cast(player);
		}
		else if (Vector3.Dot(deltaPos, aveForward) > 0.6f)
		{
            // cast punch spell
            spell = Instantiate(selected.punch, transform.position, transform.rotation).GetComponent<Spell>();
			spell.Cast(player);
		}
		else if (Vector3.Dot(deltaPos, Vector3.up) > 0.6f)
        {
            // cast pull up spell
            spell = Instantiate(selected.pull_up, player.transform.position + offsetY, horizontalSpells).GetComponent<Spell>();
            spell.Cast(player);
        }
		else if (Vector3.Dot(deltaPos, aveForward) < 0.6f && Mathf.Abs(Vector3.Dot(deltaPos, Vector3.up)) < 0.6f)
        {
            // cast across body spell
            spell = Instantiate(selected.across_body, player.transform.position + offsetY, horizontalSpells).GetComponent<Spell>();
            spell.Cast(player);
        }
    }

    private void UpdateRune(Vector2 pos)
    {
        bool changed = false;

        pos = pos.normalized;

        // RIGHT: x is around 1.0, and y is around 0.0
        if (Vector2.Dot(pos, Vector2.right) > 0.9f)
        {
            selected = runes[1];
			ani.select(1);
            changed = true;
        }
        // LEFT: x is around -1.0, and y is around 0.0 
        else if (Vector2.Dot(pos, Vector2.left) > 0.9f)
        {
            selected = runes[2];
            ani.select(2);
            changed = true;
        }
        // UP: x is around 0.0, and y is around 1.0
        else if (Vector2.Dot(pos, Vector2.up) > 0.9f)
        {
            selected = runes[0];
			ani.select(0);
            changed = true;
        }
        // DOWN: x is around 0.0, and y is around -1.0
        else if (Vector2.Dot(pos, Vector2.down) > 0.9f)
        {
            selected = runes[3];
            ani.select(3);
            changed = true;
        }

        if(changed)
        {
            runeChangeAudio.Play();
        }
    }
}
