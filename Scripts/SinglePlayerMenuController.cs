using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SinglePlayerMenuController : MonoBehaviour
{
	// reference to menu panels
	[Header ("Reference to menu panels")]
	[SerializeField]
	private GameObject startGamePanel;
	[SerializeField]
	private GameObject boot1Panel;
	[SerializeField]
	private GameObject boot2Panel;
	[SerializeField]
	private GameObject boot3Panel;
	[SerializeField]
	private GameObject endGamePanel;
	[SerializeField]
	private GameObject resultsPanel;
	[SerializeField]
	private GameObject HUDPanel;

	/*******************Move to HUDController z Igorem*******************************/
	[Header ("HUD menu panels fields")]
	[SerializeField]
	private Text redScore;
	[SerializeField]
	private Text blueScore;
	[SerializeField]
	private Slider healthPointSlider;
	[SerializeField]
	private Slider speedSlider;
	[SerializeField]
	private Text playerPointsHUD;

	public void UpdatespeedSlider (int value) {
		speedSlider.value = value;
	}
	public void UpdateHealthPointSlider (int value) {
		healthPointSlider.value = value;
	}
	/******************* END Move to HUDController z Igorem***************************/

	/************************ refernces to result panel text fields *****************/
	[Header ("Result panel text fields")]
	[SerializeField]
	private Text gameResult;
	[SerializeField]
	private Text playerDeads;
	[SerializeField]
	private Text playerKills;
	[SerializeField]
	private Text playerPoints;
	[SerializeField]
	private Text boot1Deads;
	[SerializeField]
	private Text boot1Kills;
	[SerializeField]
	private Text boot1Points;
	[SerializeField]
	private Text boot2Deads;
	[SerializeField]
	private Text boot2Kills;
	[SerializeField]
	private Text boot2Points;
	[SerializeField]
	private Text boot3Deads;
	[SerializeField]
	private Text boot3Kills;
	[SerializeField]
	private Text boot3Points;

	[Header ("Boot setup panels fields")]
	[SerializeField]
	private Toggle boot1Toogle;
	[SerializeField]
	private Toggle boot2Toogle;
	[SerializeField]
	private Toggle boot3Toogle;
	[SerializeField]
	private Dropdown boot1Dropdown;
	[SerializeField]
	private Dropdown boot2Dropdown;
	[SerializeField]
	private Dropdown boot3Dropdown;

	// to start game after pressing button we nned acces to controller
	[Header ("Single Player Game Controller")]
	[SerializeField]
	private SinglePlayerGameController singlePlayerGameController;

	[SerializeField]
	private GameObject endGameButton;

	// Use this for initialization
	void Start ()
	{
		// show start panel and boots configuration
		SetPanelsUnactive ();
		startGamePanel.SetActive (true);
		boot1Panel.SetActive (true);
		boot2Panel.SetActive (true);
		boot3Panel.SetActive (true);
		endGameButton.SetActive (false);
	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Tab)){
			resultsPanel.SetActive (true);
		}
		if (Input.GetKeyUp (KeyCode.Tab)){
			resultsPanel.SetActive (false);
		}
		if (Input.GetKey (KeyCode.Escape)) {
			Cursor.visible = !Cursor.visible;
			endGameButton.SetActive (!endGameButton.activeSelf);
		}

	}

	/// <summary>
	/// Sets the panels unactive.
	/// </summary>
	private void SetPanelsUnactive ()
	{
		startGamePanel.SetActive (false);
		boot1Panel.SetActive (false);
		boot2Panel.SetActive (false);
		boot3Panel.SetActive (false);
		endGamePanel.SetActive (false);
		resultsPanel.SetActive (false);
		HUDPanel.SetActive (false);
	}

	/********************** BUttons Actions ***************/

	public void EndGameButtonAction() {
		SceneManager.LoadScene ("mainMenu");	
	}

	/// <summary>
	/// Starts the match button action.
	/// </summary>
	public void StartMatchButtonAction ()
	{
		// hide menu 
		SetPanelsUnactive ();
		HUDPanel.SetActive (true);
		singlePlayerGameController.StartMach ();
		Cursor.visible = false;
	}

	/// <summary>
	/// Backs to menu button action.
	/// </summary>
	public void BackToMenuButtonAction ()
	{
		SceneManager.LoadScene ("mainMenu");	
	}

	/// <summary>
	/// Continues the match buttton action.
	/// </summary>
	public void ContinueMatchButttonAction ()
	{
		
	}

	/// <summary>
	/// Shows the game summary.
	/// After match is over
	/// </summary>
	public void showGameSummary ()
	{
		SetPanelsUnactive ();
		resultsPanel.SetActive (true);
		endGamePanel.SetActive (true);
	}

	/********************** Score Update ********************************/
	/*******************Move to HUDController z Igorem *******************************/
	public void RedScoreDisplayUpdate (float value)
	{
		redScore.text = "Red: " + value;
		
	}

	public void BlueScoreDisplayUpdate (float value)
	{
		blueScore.text = "Blue: " + value;

	}
	public void PlayerPointsHUDUpdate (int value)
	{
		playerPointsHUD.text = "Score: " + value;
	}

	/******************* END Move to HUDController z Igorem ***************************/

	/****************** Boots configuration *********************/

	/// <summary>
	/// Ises the boot1 active.
	/// </summary>
	/// <returns><c>true</c>, if boot1 active was ised, <c>false</c> otherwise.</returns>
	public bool IsBoot1Active ()
	{
		return boot1Toogle.isOn;
	}

	/// <summary>
	/// Ises the boot2 active.
	/// </summary>
	/// <returns><c>true</c>, if boot2 active was ised, <c>false</c> otherwise.</returns>
	public bool IsBoot2Active ()
	{
		return boot2Toogle.isOn;
	}

	/// <summary>
	/// Ises the boot3 active.
	/// </summary>
	/// <returns><c>true</c>, if boot3 active was ised, <c>false</c> otherwise.</returns>
	public bool IsBoot3Active ()
	{
		return boot3Toogle.isOn;
	}

	/// <summary>
	/// Gets the boot1 level.
	/// </summary>
	/// <returns>The boot1 level.</returns>
	public int GetBoot1Level ()
	{
		return boot1Dropdown.value;
	}

	/// <summary>
	/// Gets the boot2 level.
	/// </summary>
	/// <returns>The boot2 level.</returns>
	public int GetBoot2Level ()
	{
		return boot2Dropdown.value;
	}

	/// <summary>
	/// Gets the boot3 level.
	/// </summary>
	/// <returns>The boot3 level.</returns>
	public int GetBoot3Level ()
	{
		return boot3Dropdown.value;
	}

	/*********** Setters for results panel Field ******************/
	public void SetGameResult (string str)
	{
		gameResult.text = str;
	}

	public void SetPlayerDeads (float value)
	{
		playerDeads.text = "" + value;
	}

	public void SetPlayerKills (float value)
	{
		playerKills.text = "" + value;
	}

	public void SetPlayerPoints (float value)
	{
		playerPoints.text = "" + value;
	}

	public void SetBoot1Deads (float value)
	{
		boot1Deads.text = "" + value;
	}

	public void SetBoot1Kills (float value)
	{
		boot1Kills.text = "" + value;
	}

	public void SetBoot1Points (float value)
	{
		boot1Points.text = "" + value;
	}


	public void SetBoot2Deads (float value)
	{
		boot2Deads.text = "" + value;
	}

	public void SetBoot2Kills (float value)
	{
		boot2Kills.text = "" + value;
	}

	public void SetBoot2Points (float value)
	{
		boot2Points.text = "" + value;
	}

	public void SetBoot3Deads (float value)
	{
		boot3Deads.text = "" + value;
	}

	public void SetBoot3Kills (float value)
	{
		boot3Kills.text = "" + value;
	}

	public void SetBoot3Points (float value)
	{
		boot3Points.text = "" + value;
	}


	/********END Setters for results panel Field ******************/

}
