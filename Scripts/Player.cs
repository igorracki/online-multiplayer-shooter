using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent(typeof(HUDController))]
public class Player : NetworkBehaviour {

	private string playerName;

	// A network-synchronized boolean that checks if the player is dead.
	[SyncVar]
	private bool _isDead = false;
	public bool isDead
	{
		get { return _isDead; }
		protected set { _isDead = value; }
	}

	// A network-synchronized integer that stores the player's points.
	[SyncVar]
	private int _playersPoints = 0;
	public int playersPoints
	{
		get { return _playersPoints; }
		protected set { _playersPoints = value; }
	}

	// A network-synchronized integer that stores the player's kills.
	[SyncVar]
	private int _playerKills = 0;
	public int playerKills
	{
		get { return _playerKills; }
		protected set { _playerKills = value; }
	}

	// A network-synchronized integer that stores the player's deaths.
	[SyncVar]
	private int _playerDeaths = 0;
	public int playerDeaths
	{
		get { return _playerDeaths; }
		protected set { _playerDeaths = value; }
	}

	// Variable storing the player's health.
    [SerializeField]
    private int maxHealth = 100;

	// Network-synchronized variable storing the player's current health.
    [SyncVar]
    private int currentHealth;

	// An array of behaviours that should be disabled when player dies.
	[SerializeField]
	private Behaviour[] disableOnDeath;
	// An array of booleans corresponding to the disabled behaviours.
	private bool[] wasEnabled;

	// Variable storing the instance of the hud controller.
	private HUDController hudController;

	[SerializeField]
	private GameObject explosion;
	[SerializeField]
	private GameObject graphics;

	private AudioSource audioSource;
	[SerializeField]
	private AudioClip hitSound;
	[SerializeField]
	private AudioClip explosionSound;

	//-----------------------------------------------------------
	/**
	 * Method to setup the player in the game.
	 */ 
    public void Setup () {
		audioSource = GetComponent<AudioSource> ();

		wasEnabled = new bool[disableOnDeath.Length];
		// Finding elements that were enabled before the player died.
		for (int i = 0; i < wasEnabled.Length; i++) {
			wasEnabled[i] = disableOnDeath[i].enabled;
		}

		// Finding the hud controller in the components.
		hudController = GetComponent<HUDController> ();

		// Setting the defaults for the player.
        SetDefaults();
    }

	/**
	 * Update method used for HUD Controller.
	 */ 
	void Update () {
		if (!isLocalPlayer) {
			return;
		}

		UpdateHUD ();
	}

	/**
	 * RemoteProcedureCalls method for the player to receive damage.
	 * The method receives an amount and subtracts it from the network-synchronized currentHealth variable
	 * allowing all connected clients to see the change. It also receives the shooter's ID and passes the shooters reference
     * when killed.
	 */ 
	[ClientRpc]
    public void RpcTakeDamage (string shooterID, int amount) {
		Player shotPlayer = GameManager.GetPlayer(transform.name);

		audioSource.PlayOneShot (hitSound);

		// Reduce the players health by the received amount and update the HUD.
		shotPlayer.currentHealth -= amount;

		// Increase the shooters points depending on the amount of damage that they have dealt.
		Player shooterPlayer = GameManager.GetPlayer (shooterID);
		shooterPlayer.playersPoints += amount;

		// Points have changed for a user, update the list.
		GameManager.PointsUpdate();

		// If health is lost, kill the player.
		if (currentHealth <= 0) {
			Die(shooterPlayer);
		}
    }

	/**
	 * Method to update the Current Health Slider on HUD (per frame).
	 */ 
	private void UpdateHUD() {
		hudController.UpdateHealthSlider (currentHealth);
	}

	/**
	 * Method to kill a player if health is finished.
	 * Receives the shooter's reference so that it is known which player shot the current player.
	 * Prepares the player to be respawned, updates the statistics and respawns the player.
	 */ 
	private void Die(Player shooterPlayer) {
		isDead = true;

		audioSource.PlayOneShot (explosionSound);

		GameObject cExplosion = (GameObject)Instantiate (explosion, transform.position, Quaternion.identity);
		Destroy (cExplosion, 1.0f);
		graphics.SetActive (false);

		// Disable the behaviours that should be disabled when a player dies.
		for (int i = 0; i < disableOnDeath.Length; i++) {
			disableOnDeath[i].enabled = false;
		}

		// Find and disable the player's colider.
		Collider colider = GetComponent<Collider>();
		if (colider != null) {
			colider.enabled = false;
		}

		// Update the list.
		GameManager.PointsUpdate();
		// Update the statistics.
		UpdatePoints (shooterPlayer);

		// Start countdown and respawn the player.
		StartCoroutine(Respawn());
	}

	/**
	 * Method to respawn the player.
	 * This method uses an Enumerator to count a set amount of time before respawning a player.
	 * After time is passed, set the defaults again for this player and spawn them in one of the spawn points,
	 * which are stored in the Network Manager.
	 */ 
	private IEnumerator Respawn () {
		yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);

		graphics.SetActive (true);
		SetDefaults();
		Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
		transform.position = spawnPoint.position;
		transform.rotation = spawnPoint.rotation;
	}

	/**
	 * Method to set the default attributes for the player.
	 */ 
    public void SetDefaults () {
		isDead = false;

        currentHealth = maxHealth;
		hudController.UpdateHealthSlider (currentHealth);

		for (int i = 0; i < disableOnDeath.Length; i++) {
			disableOnDeath[i].enabled = wasEnabled[i];
		}

		Collider colider = GetComponent<Collider>();
		if (colider != null) {
			colider.enabled = true;
		}
    }

	/**
	 * Method to update the points of all involved players.
	 */
	public void UpdatePoints(Player shooterPlayer) {
		// Update the shooters points for killing a player.
		shooterPlayer.playersPoints += 100;
		// Add this kill to the shooters kill score.
		shooterPlayer.playerKills++;
		// Add a death to the player which has been shot down (local player).
		Player deadPlayer = GameManager.GetPlayer(transform.name);
		deadPlayer.playerDeaths++;
	}	

	public void SetPlayerName(string name) {
		playerName = name;
	}

	public string GetPlayerName() {
		return playerName;
	}
}
