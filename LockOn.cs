using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOn : MonoBehaviour 
{
	GameObject obj;

	 public float sphereRadius = 2.0f;
	 public float maxDistance = 40.0f;

	public LayerMask mask;

	Vector3 origin;
    Vector3 direction;

	// sphere casts in the direction the controller that attemps to lock on is facing.
	public Enemy GetLock(SteamVR_TrackedController _controller) 
	{
		// 1. sphere cast out from the controller's position
		// 2. get first enemy you run into with sphere cast
		// 3. Set lock on that enemy
		// 4. If nothing hits, clear the lock on

		origin = _controller.transform.position;
		direction = _controller.transform.forward;
		RaycastHit hit;

        // sphere cast in the direction the controller is facing
        if (Physics.SphereCast(origin, sphereRadius, direction, out hit, maxDistance, mask, QueryTriggerInteraction.Ignore))
		{
            // grab whatever the sphere cast hit
            obj = hit.transform.gameObject;
            Enemy e = obj.GetComponent<Enemy>();

			if (e) {
                e.reticle.enabled = true;
                return e;
			} else {
				return null;
			}
		}
        return null;
	}
}