using UnityEngine;
using System.Collections;

/// <summary>
/// Single player game controller.
/// Read boot conf from menu a
/// Set player and boots in position
/// Keep and Calculate points
/// poassing points to SinglePlayerMenuController
/// Respawn player and boots after dead
/// </summary>
public class SinglePlayerGameController : MonoBehaviour
{
	// to accessing HUD and menu
	[SerializeField]
	private SinglePlayerMenuController singlePlayerMenuController;

	[SerializeField]
	private SinglePlayerController singlePlayerController;
	[SerializeField]
	private SinglePlayerBootController singlePlayerBoot1Controller;
	[SerializeField]
	private SinglePlayerBootController singlePlayerBoot2Controller;
	[SerializeField]
	private SinglePlayerBootController singlePlayerBoot3Controller;


	[SerializeField]
	private int scoreRed = 40000;
	[SerializeField]
	private int scoreBlue = 40000;

	// time to wait for respawn
	private float respawnDeley = 10f;

	//flag to stop points update if is not during game
	private bool gameOn;
	[SerializeField]
	private Camera sceneCamera;

	/*
	[SerializeField]
	private GameObject playerGO;
	[SerializeField]
	private GameObject boot1GO;
	[SerializeField]
	private GameObject boot2GO;
	[SerializeField]
	private GameObject boot3GO;
//respawn points in 4 conners of map
	[SerializeField]
	private  GameObject respawnPoint1;
	[SerializeField]
	private  GameObject respawnPoint;
	[SerializeField]
	private  GameObject respawnPoint3;
	[SerializeField]
	private  GameObject respawnPoint4;
	Vector3 resPoint1;
*/
	// update to this variable commes from outside
	// and are used to calculate mach result
	private int playerDeads;
	private int playerKills;
	private int playerPoints;

	private bool isBoot1Active;
	private bool isBoot2Active;
	private bool isBoot3Active;

	private int boot1Level;
	private int boot2Level;
	private int boot3Level;

	private int boot1Deads;
	private int boot1Kills;
	private int boot1Points;

	private int boot2Deads;
	private int boot2Kills;
	private int boot2Points;

	private int boot3Deads;
	private int boot3Kills;
	private int boot3Points;

	public void StartMach ()
	{
		//Cursor.visible = false;
		// collect boot setings
		isBoot1Active = singlePlayerMenuController.IsBoot1Active ();
		isBoot2Active = singlePlayerMenuController.IsBoot2Active ();
		isBoot3Active = singlePlayerMenuController.IsBoot3Active ();

		singlePlayerController.StartMatch ();
		singlePlayerBoot1Controller.StartMatch ();
		singlePlayerBoot2Controller.StartMatch ();
		singlePlayerBoot3Controller.StartMatch ();

		gameOn = true;

		//boot1Level = singlePlayerMenuController.getBoot1Level ();
		//boot2Level = singlePlayerMenuController.getBoot2Level ();
		//boot3Level = singlePlayerMenuController.getBoot3Level ();

		//playerGO.transform = respawnPoint1.transform.position;
	}

	private void EndMatch ()
	{
		gameOn = false;
		singlePlayerController.EndMatch ();
		// show game summary
		string message = "";
		if (scoreRed < scoreBlue) {
			message = "Team Blue WON";
		} else {
			message = "Teem Red WON";
		}
		singlePlayerMenuController.SetGameResult (message);
		singlePlayerMenuController.showGameSummary ();

		// update player stats
		UpdatePlayerStats ();
	}




	// Use this for initialization
	void Start ()
	{
		gameOn = false;
	}

	// Update is called once per frame
	void Update ()
	{
		 
		if (gameOn) {
			// Collect stats from player and boots
			CollectStatsFromPlayersBootsBallons ();
			//update display of score
			SendResults (); 
			//chaeck if match ended
			if (scoreRed <= 0 || scoreBlue <= 0) { 
				EndMatch ();
			}
		} else {
			
		}
	}

	public void CollectStatsFromPlayersBootsBallons ()
	{
		//player
		playerDeads = singlePlayerController.Deads;
		playerPoints = singlePlayerController.Points;

		if (isBoot1Active) {
			boot1Deads = singlePlayerBoot1Controller.Deads;
			boot1Points = singlePlayerBoot1Controller.Points;

		}
/*
		if (isBoot2Active) {
			boot2Deads = singlePlayerBoot2Controller.Deads;
			boot2Points = singlePlayerBoot2Controller.Points;
		}
		if (isBoot3Active) {
			boot3Deads = singlePlayerBoot3Controller.Deads;
			boot3Points = singlePlayerBoot3Controller.Points;
		}
*/
	}

	private void UpdatePlayerStats ()
	{
		PlayerStats.playerStats.UpdatePlayerStarsAfterMatxh (playerPoints, playerKills, playerDeads);
	}

	public void UpdateScoreTeamRed (int value)
	{
		scoreRed += value;
	}

	public void UpdateScoreTeamBlue (int value)
	{
		scoreBlue += value;
	}

	/*********** Setters for results panel Field ******************/


	public void SetPlayerDeads (int value)
	{
		playerDeads = value;
	}

	public void UpdatePlayerKills ()
	{
		playerKills++;
	}

	public void SetPlayerPoints (int value)
	{
		playerPoints = value;
	}

	public void SetBoot1Deads (int value)
	{
		boot1Deads = value;
	}

	public void UpdateBoot1Kills ()
	{
		boot1Kills++;
	}

	public void SetBoot1Points (int value)
	{
		boot1Points = value;
	}

	public void SetBoot2Deads (int value)
	{
		boot2Deads = value;
	}

	public void UpdateBoot2Kills ()
	{
		boot2Kills++;
	}

	public void SetBoot2Points (int value)
	{
		boot2Points = value;
	}

	public void SetBoot3Deads (int value)
	{
		boot3Deads = value;
	}

	public void UpdateBoot3Kills ()
	{
		boot3Kills++;
	}

	public void SetBoot3Points (int value)
	{
		boot3Points = value;
	}
	/********END Setters for results panel Field ******************/

	/// <summary>
	/// Sends the results.to Single Player Menu Controller
	/// </summary>
	private void SendResults ()
	{
		singlePlayerMenuController.RedScoreDisplayUpdate (scoreRed);
		singlePlayerMenuController.BlueScoreDisplayUpdate (scoreBlue);
		singlePlayerMenuController.PlayerPointsHUDUpdate (playerPoints);

		singlePlayerMenuController.SetPlayerDeads (playerDeads);
		singlePlayerMenuController.SetPlayerKills (playerKills);
		singlePlayerMenuController.SetPlayerPoints (playerPoints);

		singlePlayerMenuController.SetBoot1Deads (boot1Deads);
		singlePlayerMenuController.SetBoot1Kills (boot1Kills);
		singlePlayerMenuController.SetBoot1Points (boot1Points);

		singlePlayerMenuController.SetBoot2Deads (boot2Deads);
		singlePlayerMenuController.SetBoot2Kills (boot2Kills);
		singlePlayerMenuController.SetBoot2Points (boot2Points);

		singlePlayerMenuController.SetBoot3Deads (boot3Deads);
		singlePlayerMenuController.SetBoot3Kills (boot3Kills);
		singlePlayerMenuController.SetBoot3Points (boot3Points);
	}
}
