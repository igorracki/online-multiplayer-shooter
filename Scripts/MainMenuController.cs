using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

/// <summary>
/// Class Main menu controller provide set of methods for main menu. 
/// </summary>
public class MainMenuController : MonoBehaviour
{
	// reference to menu panels
	[SerializeField]
	private GameObject startGamePanel;
	[SerializeField]
	private GameObject gameModePanel;
	[SerializeField]
	private GameObject createPlayerPanel;
	[SerializeField]
	private GameObject selectPlayerPanel;

	// reference to buttons in use
	[SerializeField]
	private Button continueGameButton;
	// prefab for dynamic created buttons
	[SerializeField]
	private GameObject loadPlayerButtonPR;
	// reference to input field used to collect user name
	[SerializeField]
	private InputField nameInputField;
	// references to RecTransform of select Player Panel used for herarhy and positioning
	// dynamicli created buttons
	[SerializeField]
	private RectTransform selectPlayerPanelRT;

	// Use this for initialization
	void Start ()
	{
		// mouse setings
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		// show start panel
		ShowPanel (startGamePanel);
		// turn off the continue game button if there is no save file
		if (PlayerStats.playerStats.PlayerName == null) {
			SetContinueGameButtonInteractable (false);
		}
	}

	/// <summary>
	/// Shows the panel.
	/// </summary>
	/// <param name="panel">Panel.</param>
	private void ShowPanel (GameObject panel)
	{
		startGamePanel.SetActive (false);
		gameModePanel.SetActive (false);
		createPlayerPanel.SetActive (false);
		selectPlayerPanel.SetActive (false);
		panel.SetActive (true);
	}

	/// <summary>
	/// Sets the continue game button interactable.
	/// </summary>
	/// <param name="value">If set to <c>true</c> value.</param>
	public void SetContinueGameButtonInteractable (bool value)
	{
		continueGameButton.interactable = value;
	}

	// ********* Methods for Buttons on Start Game Panel  *********** //

	/// <summary>
	/// Continues the game button action.
	/// </summary>
	public void ContinueGameButtonAction ()
	{
		PlayerStats.playerStats.LoadLastSave ();
		ShowPanel (gameModePanel);
	}

	/// <summary>
	/// New player button action.
	/// </summary>
	public void NewPlayerButtonAction ()
	{
		ShowPanel (createPlayerPanel);
		nameInputField.text = "";
	}

	/// <summary>
	/// Changes the player button action.
	/// </summary>
	public void ChangePlayerButtonAction ()
	{
		ShowPanel (selectPlayerPanel);
		LoadListOfSaves ();
	}

	/// <summary>
	/// Exits the game button action.
	/// </summary>
	public void ExitGameButtonAction ()
	{
		Application.Quit ();
	}

	// ********* Methods for Buttons on Game Mode Panel  *********** //

	/// <summary>
	/// Single Player button action.
	/// </summary>
	public void SinglePlayerButtonAction ()
	{
		SceneManager.LoadScene ("singlePlayer");
	}

	/// <summary>
	/// Multiplayer button action.
	/// </summary>
	public void MultiplayerButtonAction ()
	{
		SceneManager.LoadScene ("multiPlayer");
	}

	// ********* Methods for Buttons on Create Player Panel  *********** //

	/// <summary>
	/// Creates the player button action.
	/// </summary>
	public void CreatePlayerButtonAction ()
	{
		if (nameInputField.text != "") {
			// print (nameInputField.text+ " i chuj");
			PlayerStats.playerStats.newPlayer (nameInputField.text);
			PlayerStats.playerStats.Save ();
			nameInputField.text = "";
			ShowPanel (gameModePanel);

		} else {
			///////////////////////////////////////////////////////
			//Debug.Log ("Enter name");
		}
	}


	/// <summary>
	/// Back to the menu button action.
	/// </summary>
	public void BackButtonAction ()
	{
		ShowPanel (startGamePanel);
	}

	/// <summary>
	/// Loads the list of saves and display it as a buttons on Select Player Panel.
	/// </summary>
	public void LoadListOfSaves ()
	{
		// clear panel from all buttons( unupdated can exist)
		foreach (Button button in selectPlayerPanel.GetComponentsInChildren<Button>()) {
			string str = button.name;
			if (!str.Equals ("Back To Start Game Button")) {
				DestroyObject (button.gameObject);
			}
		}
		// list of saves files in folder 
		FileInfo[] listOfSavedFiles = PlayerStats.playerStats.GetListOfSavedFiles ();
		// Y position of button on the panel 
		float posY = 300f;

		// looping throug all save files and creates buttons 
		foreach (FileInfo file in listOfSavedFiles) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream fs = file.OpenRead ();
			PlayerStatsData playerStatsData = null;

			try {
				// create object from file
				playerStatsData = (PlayerStatsData)bf.Deserialize (fs);

				if (playerStatsData != null) {
					// creating button
					GameObject buttonGO = Instantiate (loadPlayerButtonPR) as GameObject;
					// make panel a parent of new GameObject
					buttonGO.transform.SetParent (selectPlayerPanelRT);
					// positioning button
					buttonGO.transform.localPosition = new Vector3 (20f, posY, 0);
					buttonGO.transform.localScale = new Vector3(1,1,1);
					// seting button label with player name
					buttonGO.GetComponentInChildren<Text> ().text = playerStatsData.PlayerName;
					// create references to the button (so far is a Game Object)
					Button button = buttonGO.GetComponent<Button> ();
					String fileName = file.Name;
					// add Action litenera to button 
					button.onClick.AddListener (delegate {
						PlayerStats.playerStats.Load (fileName);
						PlayerStats.playerStats.Save ();
						ShowPanel (gameModePanel);
					});
				}
				// very importent!
				// file need to be close(sharing error)
				fs.Close ();
			} catch (Exception ex) {
				Debug.LogError (file.Name + "File is corrupted");
				Debug.LogError (ex.StackTrace);
			}
			// nex button get new position (45 lower)
			posY -= 45f;
		}
	}
}
