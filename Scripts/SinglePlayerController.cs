using UnityEngine;
using System.Collections;

public class SinglePlayerController : MonoBehaviour {

	// Storing an instance of the plane pilot (player motor)
	[SerializeField]
	private SinglePlayerPlanePilot singlePlayerPlanePilot;
	[SerializeField]
	private SinglePlayerMenuController singlePlayerMenuController;
	[SerializeField]
	private SinglePlayerGameController singlePlayerGameController;

	[SerializeField]
	private Transform respawnPoint1;

	//Main camera reference to disable on start
	[SerializeField]
	Camera sceneCamera;
	[SerializeField]
	Camera playerCamera;

	[SerializeField]
	private int maxHealth = 100;

	[SerializeField]
	private GameObject explosion;
	[SerializeField]
	private GameObject graphics;

	// Storing the player's current health.
	private int currentHealth;
	private bool isDead;
	private int deads;
	private int points;

	[SerializeField]
	private AudioClip hitSound;
	[SerializeField]
	private AudioClip explosionSound;
	[SerializeField]
	private AudioClip lockSound;

	private AudioSource audioSource;

	void Update ()
	{
		if (!isDead) {
			// Storing input variables
			float speedMovement = Input.GetAxisRaw ("Vertical");
			float mouseYMovement = Input.GetAxis ("Mouse Y");
			float mouseXMovement = Input.GetAxis ("Mouse X");
			float keyboardRotation = Input.GetAxisRaw ("Horizontal");

			// Informing the pilot to execute changes
			singlePlayerPlanePilot.ControlSpeed (speedMovement);
			singlePlayerPlanePilot.ControlRotation (mouseYMovement, mouseXMovement);
			singlePlayerPlanePilot.ControlKeyboardRotation (keyboardRotation);
		} 
	}
		
	/// <summary>
	/// Raises the collision enter event.
	/// Kedy jebnie w balin albo w co innego to smierc
	/// </summary>
	/// <param name="c">C.</param>
	void OnCollisionEnter(Collision c) {
		if(!c.gameObject.CompareTag("Untagged")) {
			TakeDamage (maxHealth);
		}
	}

	//-----------------------------------------------------------
	void Start ()
	{
		audioSource = GetComponent<AudioSource> ();
		singlePlayerPlanePilot.enabled = false;
	}

	/// <summary>
	/// Starts the match.
	/// </summary>
	public void StartMatch() {
		SwichCameraToPlayerCamera ();
		// set all fields for default value;
		currentHealth = maxHealth;
		singlePlayerMenuController.UpdateHealthPointSlider (currentHealth);
		isDead = false;
		deads = 0;
		points = 0;
		singlePlayerPlanePilot.enabled = true;

		transform.position = respawnPoint1.position;
		transform.rotation = respawnPoint1.rotation;
	}

	/// <summary>
	/// Updates the points.
	/// </summary>
	/// <param name="value">Value.</param>
	public void UpdatePoints(int value) {
		points += value; 
	}

	/// <summary>
	/// Takes the damage.
	/// Methods called by other players  
	/// </summary>
	/// <param name="value">Value.</param>
	public void TakeDamage (int value) {
		//Debug.Log (value);
		if (currentHealth > 0) {
			currentHealth -= value;
			audioSource.PlayOneShot (hitSound);
			audioSource.PlayOneShot (lockSound);
			// update slider on HUD
			singlePlayerMenuController.UpdateHealthPointSlider (currentHealth);
			// check if player has died form taken damages 
			if (currentHealth <= 0) {
				playerDied ();
			}
		}
	}


	/// <summary>
	/// Players has died.
	/// </summary>
	public void playerDied () {
		Debug.Log ("Zabity");
		audioSource.PlayOneShot (explosionSound);
		GameObject cExplosion = (GameObject)Instantiate (explosion, transform.position, Quaternion.identity);
		Destroy (cExplosion, 1.0f);
		graphics.SetActive (false);

		isDead = true;
		deads++;
		// update game score
		singlePlayerGameController.UpdateScoreTeamBlue (-1000);
		StartCoroutine (Respawn());
	}

	/// <summary>
	/// Respawn the player after dead.
	/// </summary>
	private IEnumerator Respawn() {
		yield return new WaitForSeconds(2.0f);
		// respawn delay NIE DZIAL
		currentHealth = maxHealth;
		isDead = false;
		graphics.SetActive (true);
		singlePlayerMenuController.UpdateHealthPointSlider (currentHealth);
		// move to respawn position and rotation
		transform.position = respawnPoint1.position;
		transform.rotation = respawnPoint1.rotation;
		// enable user controll
		singlePlayerPlanePilot.enabled = true;
	}

	/// <summary>
	/// Ends the match.
	/// </summary>
	public void EndMatch() {
		SwichCameraToSceneCamera ();
		singlePlayerPlanePilot.enabled = false;
	}

	/**************************** NIE BARDZO WIEM CO I JAK ALE DZIALA **************************/
	private void SwichCameraToSceneCamera () {
		sceneCamera = Camera.main;
		if (sceneCamera != null) {
			sceneCamera.gameObject.SetActive(false);
		}
	}

	private void SwichCameraToPlayerCamera () {
		playerCamera = Camera.main;
		if (playerCamera != null) {
			playerCamera.gameObject.SetActive(false);
		}
	}
	/**************************** NIE BARDZO WIEM CO I JAK ALE DZIALA **************************/

	/// <summary>
	/// Gets the deads.
	/// </summary>
	/// <value>The deads.</value>
	public int Deads {
		get {
			return deads;
		}
	}

	/// <summary>
	/// Gets the points.
	/// </summary>
	/// <value>The points.</value>
	public int Points {
		get {
			return points;
		}
	}
 }
