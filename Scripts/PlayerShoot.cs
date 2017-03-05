using UnityEngine;
using UnityEngine.Networking;

//[RequireComponent (typeof (WeaponManager))]
public class PlayerShoot : NetworkBehaviour {

	// Creating a player tag.
	private const string PLAYER_TAG = "Player";

	[SerializeField]
	private Camera cam;

	// Creating a field to choose, what mask should the laser hit.
	[SerializeField]
	private LayerMask mask;

	// Creating a field with current weapon options.
	[SerializeField]
	private PlayerWeapon currentWeapon;

	// Fields for the laser variables.
	[SerializeField]
	private Transform laserSpawnPoint;
	[SerializeField]
	private GameObject laserPrefabGO;
	[SerializeField]
	private float laserLifetime = 5.0F;

	private AudioSource audioSource;

	[SerializeField]
	private AudioClip shootSound;

	//-----------------------------------------------------
	/**
	 * Start method to check if the player has their own camera instance.
	 */ 
	void Start () {
		if (cam == null) {
			Debug.LogError("PlayerShoot: No camera referenced!");
			this.enabled = false;
		}

		audioSource = GetComponent<AudioSource> ();
	}

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
	 * Method called on the server.
	 * This method is called when the player is shooting and calls a method that will spawn a laser.
	 */ 
	[Command]
	void CmdPlayerShooting () {
		RpcCreateShootingEffect();
    	}

	/**
	 * RemoteProcedureCalls method that instantiates a laser.
	 * This method makes the laser visible for all players connected to the game.
	 */ 
	[ClientRpc]
	void RpcCreateShootingEffect () {
		GameObject laser = (GameObject)Instantiate(laserPrefabGO, laserSpawnPoint.position,transform.rotation);
		Destroy(laser, laserLifetime);
		audioSource.PlayOneShot (shootSound);
	}

	/**
	 * Method called on the server.
	 * This method is called whenever a player hits something and calls a method which creates an effect when a player gets hit.
	 */ 
	[Command]
	void CmdLaserHitSomething (Vector3 pos, Vector3 normal) {
		RpcCreateHitEffect(pos, normal);
   	}

	/**
	 * RemoteProcedureCalls method that creats a hit effect.
	 */ 
	[ClientRpc]
	void RpcCreateHitEffect(Vector3 pos, Vector3 normal) {

	}

	/**
	 * Method called on the client.
	 * This method is called when the user presses the fire button.
	 */ 
	[Client]
	void Shoot () {
		if (!isLocalPlayer) {
			return;
		}

		// The player is currently shooting, call the shoot method on the server.
		CmdPlayerShooting();

		// Using a raycast to see what the player is aiming at, and then shooting to that place.
		RaycastHit hit;
		if (Physics.Raycast(cam.transform.position + cam.transform.forward * 35.0f, cam.transform.forward, out hit, currentWeapon.range, mask)) {
			if (hit.collider.tag == PLAYER_TAG) {
				CmdPlayerShot(hit.collider.name, currentWeapon.damage);
			}

			// Call the hit method on the server to create a hit effect.
			CmdLaserHitSomething(hit.point, hit.normal);
		}
	}

	/**
	 * Method called on the server.
	 * This method is called whenever another player gets hit.
	 * This method finds the hit player and reduces their health.
	 */ 
	[Command]
	void CmdPlayerShot (string playerID, int damage) {
		Player player = GameManager.GetPlayer(playerID);
		player.RpcTakeDamage(transform.name, damage);
	}
}
