using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreList : MonoBehaviour {

	[SerializeField]
	private GameObject playerScorePrefab;

	private int changeCounter;

	void Start() {
		changeCounter = 0;
	}

	// Update is called once per frame
	void Update () {
		UpdateList ();
	}

	public void UpdateList() {
		// If the counter is the same, this means that there has been no change
		// and there is no need to update the list again, so escape the Update loop.
		// Without this, the FPS could be affected by the same operation being executed over and over.
		if (GameManager.GetPointsChange () == changeCounter) {
			return;
		}

		// Otherwise store the latest change.
		changeCounter = GameManager.GetPointsChange ();

		Player[] players = GameManager.GetAllPlayers ();

		// Refresh the list by deleting every record on it.
		while(this.transform.childCount > 0) {
			Transform child = this.transform.GetChild(0);
			// Remove it from the hierarchy so that it is no longer considered as a child element. Otherwise this would be an infinite loop.
			child.SetParent(null);
			Destroy (child.gameObject);
		}

		// Get all players and print their scores to the screen.
		foreach(Player player in players) {
			GameObject playerScore = (GameObject)Instantiate(playerScorePrefab);
			playerScore.transform.SetParent(this.transform);
			if (player.isLocalPlayer) {
				playerScore.transform.Find ("Username").GetComponent<Text> ().text = player.transform.name + " (You) ";
			} else {
				playerScore.transform.Find ("Username").GetComponent<Text> ().text = player.transform.name;
			}
			playerScore.transform.Find ("Kills").GetComponent<Text> ().text = player.playerKills.ToString ();
			playerScore.transform.Find ("Deaths").GetComponent<Text> ().text = player.playerDeaths.ToString();
			playerScore.transform.Find ("Points").GetComponent<Text> ().text = player.playersPoints.ToString ();
		}
	}
}
