using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;

/// <summary>
/// Class Player stats.
/// Responsible for providing player statistics for other classes across the game
/// and Loading and saving them to the file.
/// </summary>
public class PlayerStats : MonoBehaviour
{
	// singielton
	public static PlayerStats playerStats;

	//separate stats for single player and for multi?
	private int expirience;
	private int kills;
	private int deads;
	private string playerName;

	/// <summary>
	/// The list of saved files.
	/// </summary>
	private FileInfo[] listOfSavedFiles;

	// Use this for initialization
	void Awake ()
	{
		if (playerStats == null) {
			DontDestroyOnLoad (gameObject);
			playerStats = this;
		} else if (playerStats != this) {
			Destroy (gameObject);
		}

		LoadLastSave ();
	}

	// Save and load methods are called only by program (automaticly)
	// no need of user interaction
	public void Save ()
	{
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Open (Application.persistentDataPath + "/" + PlayerName + ".save", FileMode.OpenOrCreate);
		// create object to serilaize
		PlayerStatsData data = new PlayerStatsData (playerName, expirience, kills, deads);
		// serialize data and put it to file
		bf.Serialize (file, data);
		file.Close ();
	}

	/// <summary>
	/// Loads the last save. From default persistent directory.
	/// Methods creates two object of type PlayerStatsData and compare lastSaveTime field.
	/// Object with the latest date is loaded to stats
	/// </summary>
	public void LoadLastSave ()
	{
		PlayerStatsData currentData = null;
		PlayerStatsData tempData = null;

		listOfSavedFiles = GetListOfSavedFiles ();
		int compareResult;

		foreach (FileInfo file in listOfSavedFiles) {

			BinaryFormatter bf = new BinaryFormatter ();
			FileStream fs = file.OpenRead ();

			// If a file in directory not contents object of class TestObject then 
			// deserialization of it will return Exeption, 
			try {
				tempData = (PlayerStatsData)bf.Deserialize (fs);
				if (tempData != null) {
					if (currentData != null) {
						// compare which of data has latest lastSaveTime
						compareResult = currentData.LastSaveTime.CompareTo (tempData.LastSaveTime);
						if (compareResult <= 0) { // currentData is later than tempData.LastSaveTime 
							currentData = tempData;
						}
					} else {
						currentData = tempData;
					}
				}

			} catch (Exception ex) {
				print (file.Name + " - File is corrupted");
				print (ex.StackTrace);
			}
			fs.Close();
			// if no current save then turn off the continue game button
			if (currentData != null) {
				IntializeValueOfStats (currentData);
			} else {
				Debug.Log ("nie ma plikow do zaladowania");
				//print ("nie ma plikow do zaladowania");
				// MainMenuController.SetContinueGameButtonInteractable (false);
			}
		}
	}
	/// <summary>
	/// Load the specified fileName. 
	/// </summary>
	/// <param name="fileName">File name.</param>
	public void Load (string fileName)
	{
		if (File.Exists (Application.persistentDataPath + "/" + fileName)) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/" + fileName, FileMode.Open);
			PlayerStatsData data = (PlayerStatsData)bf.Deserialize (file);
			file.Close ();

			IntializeValueOfStats (data);
		}
	}

	/// <summary>
	/// Method iniciates stats of the new player with name and default values.
	/// </summary>
	/// <param name="playerName">Player name.</param>
	public void newPlayer (string playerName)
	{
		this.Expirience = 0;
		this.PlayerName = playerName;
		this.Kills = 0;
		this.Deads = 0;
	}

	/// <summary>
	/// Gets the list of saved files.
	/// </summary>
	/// <returns>The list of saved files.</returns>
	public FileInfo[] GetListOfSavedFiles ()
	{
		DirectoryInfo dir = new DirectoryInfo (@Application.persistentDataPath); // Directory is Application.persistentDataPath
		return dir.GetFiles ("*.save"); //Getting save files		
	}

	/// <summary>
	/// Intializes the value of stats.
	/// </summary>
	/// <param name="playerStatsData">Player stats data.</param>
	private void IntializeValueOfStats (PlayerStatsData playerStatsData)
	{
		this.Expirience = playerStatsData.Expirience;
		this.PlayerName = playerStatsData.PlayerName;
		this.Kills = playerStatsData.Kills;
		this.Deads = playerStatsData.Deads;
	}

	/////////////////// for test purpose //////////////////////////////////////////////
	/// <summary>
	/// Raises the GU event.
	/// </summary>
	/*
	void OnGUI ()
	{
		GUI.Label (new Rect (300, 10, 150, 30), "playerName: " + playerName);
		GUI.Label (new Rect (300, 40, 150, 30), "kills: " + kills);
		GUI.Label (new Rect (300, 70, 150, 30), "deads: " + deads);
		GUI.Label (new Rect (300, 100, 150, 30), "Exp: " + expirience);
		GUI.Label (new Rect (300, 130, 500, 60), "Place where saves are keept (Application.persistentDataPath):\n " + Application.persistentDataPath);
	}*/

	/////////////////// for test purpose //////////////////////////////////////////////

	/* ************************* Methods to update score after match end *************************** */ 
	public void UpdatePlayerStarsAfterMatxh (int expirience, int kills, int deads) {
		this.expirience += expirience;
		this.kills += kills;
		this.deads += deads;
		Save ();
	}

	////////////////////////////////////////// Geters and seters ///////////////////////
	public int Expirience {
		get {
			return expirience;
		}
		set {
			expirience = value;
		}
	}

	public int Kills {
		get {
			return kills;
		}
		set {
			kills = value;
		}
	}

	public int Deads {
		get {
			return deads;
		}
		set {
			deads = value;
		}
	}

	public string PlayerName {
		get {
			return playerName;
		}
		set {
			playerName = value;
		}
	}
}