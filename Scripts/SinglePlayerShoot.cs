using UnityEngine;
using System.Collections;

public class SinglePlayerShoot : MonoBehaviour {

	// Fields for the laser variables.
	[SerializeField]
	private Transform laserSpawnPoint;
	[SerializeField]
	private GameObject laserPrefabGO;
	[SerializeField]
	private float laserLifetime = 2.0F;

	private const string PLAYER_TAG = "Player";

	// Creating a field with current weapon options.
	[SerializeField]
	private PlayerWeapon currentWeapon;

	[SerializeField]
	private Camera cam;

	[SerializeField]
	private LayerMask mask;

	[SerializeField]
	private AudioClip shotSound;

	private AudioSource audioSource;

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	/**
	 * Update method collecting the input and performing methods.
	 * When the player holds down the shoot button, the script invokes a repeat of the shoot method.
	 * Whenever the player releases the shoot button, the repeating is canceled.
	 */ 
	void Update () {
		if (currentWeapon.fireRate <= 0f) {
			if (Input.GetButtonDown("Fire1")) {
				Shoot();
			}
		} else {
			if (Input.GetButtonDown("Fire1")) {
				InvokeRepeating("Shoot", 0f, 1f/currentWeapon.fireRate);
			} else if (Input.GetButtonUp ("Fire1"))	{
				CancelInvoke("Shoot");
			}
		}
	}



	/**
	 *  This method is called when the user presses the fire button.
	 */
	void Shoot () {
	//	print("Shoot method");
		DoShootEffect ();
		// The player is currently shooting, call the shoot method on the server.
		//CmdOnShoot();

		// Using a raycast to see what the player is aiming at, and then shooting to that place.
		RaycastHit hit;
		if (Physics.Raycast(cam.transform.position + cam.transform.forward * 35.0f, cam.transform.forward, out hit, currentWeapon.range, mask)) {
			Debug.Log ("We hit - " + hit.collider.tag);
			if (hit.collider.tag == "Boot1") {
				//GameObject target = hit.collider.gameObject;
				SinglePlayerBootController boot1 = hit.collider.gameObject.GetComponent <SinglePlayerBootController> ();
				boot1.TakeDamage (currentWeapon.damage, PLAYER_TAG);
			} 
			if (hit.collider.tag == "Boot2") {
				//GameObject target = hit.collider.gameObject;
				SinglePlayerBootController boot2 = hit.collider.gameObject.GetComponent <SinglePlayerBootController> ();
				boot2.TakeDamage (currentWeapon.damage, PLAYER_TAG);
			} 
			if (hit.collider.tag == "Boot3") {
				//GameObject target = hit.collider.gameObject;
				SinglePlayerBootController boot3 = hit.collider.gameObject.GetComponent <SinglePlayerBootController> ();
				boot3.TakeDamage (currentWeapon.damage, PLAYER_TAG);
			} 
		}
	}

	private void DoShootEffect () {
		GameObject laser = (GameObject)Instantiate(laserPrefabGO, laserSpawnPoint.position,transform.rotation);
		Destroy(laser, laserLifetime);
		audioSource.PlayOneShot (shotSound);
	}
}
