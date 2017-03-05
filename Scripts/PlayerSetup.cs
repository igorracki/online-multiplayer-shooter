using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour {
	// Storing the variables used in this script.
	[SerializeField]
	private Behaviour[] componentsToDisable;
	[SerializeField]
	private string remoteLayerName = "RemotePlayer";
	[SerializeField]
	private GameObject playerUIPrefab;

	private GameObject playerUIInstance;

	Camera sceneCamera;

	//-------------------------------------------------------------------------------------------
	void Start () {
		// Disabling all components which should only be active on the player that the client can control.
		if (!isLocalPlayer)	{
			DisableComponents();
			AssignRemoteLayer();
		} else {
			// Disable the scene camera for the local player.
			sceneCamera = Camera.main;
			if (sceneCamera != null) {
				sceneCamera.gameObject.SetActive(false);
			}

			// Create (Instantiate) HUD
			playerUIInstance = Instantiate(playerUIPrefab);
			playerUIInstance.name = playerUIPrefab.name;
		}

		// Setting up the player.
		GetComponent<Player>().Setup();
	}

	/**
	 * Method performing when a client joins the game.
	 */ 
	public override void OnStartClient() {
		base.OnStartClient();

		Cursor.visible = false;

		// Storing the players ID and the player.
		string netID = GetComponent<NetworkIdentity>().netId.ToString();
		Player player = GetComponent<Player>();

		// Register a player in the game.
		GameManager.RegisterPlayer(netID, player);

		// User has registered to the game, update the list.
		GameManager.PointsUpdate();
	}

	/**
	 * Method assigning a remote layer to all other players (that are not local on our client).
	 */ 
	void AssignRemoteLayer () {
		gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
	}

	/**
	 * Method to disable components for all non local players.
	 */ 
	void DisableComponents () {
		for (int i = 0; i < componentsToDisable.Length; i++) {
			componentsToDisable[i].enabled = false;
		}
	}

	/**
	 * Method perfromed when a player leaves the game.
	 */ 
	void OnDisable () {
		Cursor.visible = true;

		Destroy(playerUIInstance);

		// Re-enable the scene camera
		if (sceneCamera != null) {
			sceneCamera.gameObject.SetActive(true);
		}

		// User has left the game, update the list.
		GameManager.PointsUpdate();

		// Save the players statistics.
		GameManager.UpdatePlayerStats(transform.name);

		//Unregister the player
		GameManager.UnregisterPlayer(transform.name);

		SceneManager.LoadScene ("mainMenu");	
	}

}
