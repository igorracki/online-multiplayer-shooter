using UnityEngine;
using System.Collections;

public class SinglePlayerBootController : MonoBehaviour {

	private bool isActive;
	private int level;
	private int maxHealth = 100;
	private bool isDead = false;
	[SerializeField]
	private int currentHealth;

	private int deads;
	private int points;

	[SerializeField]
	private SinglePlayerMenuController singlePlayerMenuController;
	[SerializeField]
	private SinglePlayerGameController singlePlayerGameController;
	[SerializeField]
	private SinglePlayerController singlePlayerController;

	[SerializeField]
	private SinglePlayerBaloon baloon1;
	[SerializeField]
	private SinglePlayerBaloon baloon2;
	[SerializeField]
	private SinglePlayerBaloon baloon3;


	[SerializeField]
	private Transform respawnPoint;

	[SerializeField]
	private SplineWalker splineWalker;

	//private bool playerInside = false;
	private Vector3 playerPosition;

	[SerializeField]
	private PlayerWeapon currentWeapon;
	[SerializeField]
	private float shootingDelay = 0.5f;
	private float shootingTimer;
	[SerializeField]
	private LayerMask mask;
	[SerializeField]
	private Transform laserSpawnPoint;
	[SerializeField]
	private GameObject laserPrefabGO;
	[SerializeField]
	private float laserLifetime = 2.0F;

	[SerializeField]
	private GameObject explosion;
	[SerializeField]
	private GameObject graphics;

	public bool isBusy = false;
	public bool takeBaloon = false;

	[SerializeField]
	private AudioClip hitSound;
	[SerializeField]
	private AudioClip explosionSound;
	[SerializeField]
	private AudioClip shotSound;
	[SerializeField]
	private AudioClip lockSound;

	private AudioSource audioSource;


	void Start() {
		audioSource = GetComponent<AudioSource> ();
	}
	// Update is called once per frame
	void FixedUpdate () {
		Patrol ();
		shootingTimer += Time.fixedDeltaTime;
	}

	public void Patrol()
	{
		if (!isBusy) {
			if (!splineWalker.IsEscaping) {
				//Debug.Log ("UPDATE BOT CONTROLER line 62");
				if (baloon1.IsCapturedByEnemy) {
					splineWalker.SetPath ("captureBalloon1");
					isBusy = true;
				} 
				if (baloon2.IsCapturedByEnemy && !baloon1.IsCapturedByEnemy) {
					splineWalker.SetPath ("captureBalloon2");
					isBusy = true;
				} 
				if (baloon3.IsCapturedByEnemy && !baloon1.IsCapturedByEnemy && !baloon2.IsCapturedByEnemy) {
					splineWalker.SetPath ("captureBalloon3");
					isBusy = true;
				}
				if (!baloon3.IsCapturedByEnemy && !baloon1.IsCapturedByEnemy && !baloon2.IsCapturedByEnemy) {
					splineWalker.SetPath ("patrol");
					isBusy = true;
				}
			}
		}

		if (!baloon3.IsCapturedByEnemy) {
			isBusy = false;
		} else if (!baloon2.IsCapturedByEnemy) {
			isBusy = false;
		} else if (!baloon1.IsCapturedByEnemy) {
			isBusy = false;
		}
	}

	public void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.CompareTag ("Player")) {
			audioSource.PlayOneShot (lockSound);
			// PLAY some scery music
		}
	}

	public void OnTriggerExit(Collider collider) {
		if (collider.gameObject.CompareTag ("Player")) {
			Debug.Log("KOnie ataku");
			isBusy = false;
			StopAttacingPlayer ();
		}
	}

	public void OnTriggerStay(Collider collider) {
		if (collider.gameObject.CompareTag ("Player")) {
			playerPosition = collider.gameObject.transform.position;
			Debug.Log ("Atak");
			AttackPlayer ();
		}
	}

	private void StopAttacingPlayer (){
		splineWalker.SetPath ("patrol");	
	}

	private void AttackPlayer() {
		Shoot ();
		isBusy = true; // turn off update method action
		splineWalker.SetPath ("followPlayer"); //turn off flighing on path
		splineWalker.GoToPlayer (playerPosition); // fly to player
	}

	void Shoot () {
		if (shootingTimer > shootingDelay) {
			DoShootEffect ();
			Debug.Log("Boot Strzelam");
			// Using a raycast to see what the player is aiming at, and then shooting to that place.
			RaycastHit hit;
			if (Physics.Raycast (transform.position + transform.forward * 35.0f, transform.forward, out hit, currentWeapon.range, mask)) {
				if (hit.collider.tag == "Player") {
					Debug.Log("Boot trafia playera");
					singlePlayerController.TakeDamage (currentWeapon.damage);
				} 
			}
			shootingTimer = 0;
		}
	}

	private void DoShootEffect () {
		GameObject laser = (GameObject)Instantiate(laserPrefabGO, laserSpawnPoint.position,transform.rotation);
		Destroy(laser, laserLifetime);
		audioSource.PlayOneShot (shotSound);
	}

	public void TakeDamage (int value , string tag) {
		if (!isDead) {
			currentHealth -= value;
			audioSource.PlayOneShot (hitSound);
			if (tag.Equals ("Player")) {
				singlePlayerController.UpdatePoints (value);
			}

			// check if player has died form taken damages 
			if (currentHealth <= 0) {
				bootDied ();
				if (tag.Equals ("Player")) {
					singlePlayerController.UpdatePoints (100); // add 100 pkt to player score
					singlePlayerGameController.UpdatePlayerKills ();
				}
			} else if (currentHealth > 0) {
				AvoidAttack ();
			}
		}
	}
	/// <summary>
	/// ZAMINENIC PARAMETRY na escape2/////////////////
	/////////////
	//////////////
	/// </summary>
	private void AvoidAttack() {
		int random = Random.Range (1, 4);

		if (random == 1) {
			Debug.Log ("korkociag");
			splineWalker.SetPath ("escape1");
		} else if (random == 2) {
			Debug.Log ("petla");
			splineWalker.SetPath ("escape2");
		} else if (random == 3) {
			Debug.Log ("beczka");
			splineWalker.SetPath ("escape3");
		}
	}

	private void bootDied() {
		isDead = true;
		splineWalker.SetPath ("death");
		isBusy = false;

		audioSource.PlayOneShot (explosionSound);
		GameObject cExplosion = (GameObject)Instantiate (explosion, transform.position, Quaternion.identity);
		Destroy (cExplosion, 1.0f);
		graphics.SetActive (false);

		print ("Zabiles bota1"); 
		//every dead decrise match score by 1000
		singlePlayerGameController.UpdateScoreTeamRed (-1000);
		// move to respan point
		StartCoroutine (Respawn());
	}

	public void StartMatch() {
		BootSetup ();
		deads = 0;
		points = 0;
		// start coorutine with default values; move to respan point
		//StartCoroutine (Respawn());
		// move to respawn position and rotation
		transform.position = respawnPoint.position;
		transform.rotation = respawnPoint.rotation;
	}

	private IEnumerator Respawn() {
		// respawn delay
		yield return new WaitForSeconds(2.0f);
		isDead = false;
		graphics.SetActive (true);
		currentHealth = maxHealth;
		// move to respawn position and rotation
		transform.position = respawnPoint.position;
		transform.rotation = respawnPoint.rotation;

	}

	private void BootSetup () {
		currentHealth = maxHealth;
		isActive = singlePlayerMenuController.IsBoot1Active ();
		level = singlePlayerMenuController.GetBoot1Level ();
	}

	public void UpdatePoints(int value) {
		points += value;
	}

	public int Deads {
		get {
			return deads;
		}
	}

	public int Points {
		get {
			return points;
		}
	}
}