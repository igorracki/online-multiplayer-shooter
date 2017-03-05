using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour {

	// Singleton instance of the GameManager.
	public static GameManager instance;

	// Instance of the match settings script.
	public MatchSettings matchSettings;

	// Constant variable for player prefix.
	private const string PLAYER_ID_PREFIX = "Player ";

	// Points change counter to update the list only when necesary
	private static int pointsChangeCounter = 0;

	// A dictionary to store all players that are connected to our game, together with their IDs.
	private static Dictionary<string, Player> players = new Dictionary<string, Player>();

	//----------------------------------------------------------------------------------------
	/**
	 * Method performing whenever the script is called, checking for singleton instance.
	 */ 
	void Awake () {
		if (instance != null) {
			Debug.LogError("More than one GameManager in scene.");
		} else	{
			instance = this;
		}
	}

	/**
	 * Method to register a new player in the game (add to dictionary).
	 */ 
	public static void RegisterPlayer (string netID, Player player) {
		string playerID = PLAYER_ID_PREFIX + netID;
		players.Add(playerID, player);
		player.transform.name = playerID;
	}

	/**
	 * Method to remove a player from the game (remove from dictionary).
	 */ 
	public static void UnregisterPlayer (string playerID) {
		players.Remove(playerID);
	}

	/**
	 * Method to find and return a player that is in the game (dictionary).
	 */ 
	public static Player GetPlayer (string playerID) {
		return players[playerID];
	}

	/**
	 * Method to return all players that are currently in the game.
	 */ 
	public static Player[] GetAllPlayers() {
		return players.Values.ToArray ();
	}

	/**
	 * Method to update a player's statistic to the save file.
	 */ 
	public static void UpdatePlayerStats(string playerID) {
		Player player = GetPlayer (playerID);
		PlayerStats.playerStats.UpdatePlayerStarsAfterMatxh (player.playersPoints, player.playerKills, player.playerDeaths);
	}

	/**
	 * Method to update the points change counter when the list should be updated.
	 * Executed by Player.
	 */ 
	public static void PointsUpdate() {
		pointsChangeCounter++;
	}

	/**
	 * Method to return points change counter. Used by Score List.
	 */ 
	public static int GetPointsChange() {
		return pointsChangeCounter;
	}
}
