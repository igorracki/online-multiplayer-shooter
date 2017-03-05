using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDController : MonoBehaviour {

	// Creating fields for the HUD.
	[SerializeField]
	private Slider hpSlider;
	[SerializeField]
	private Slider speedSlider;
	[SerializeField]
	private GameObject scoreBoard;

	//----------------------------------------------------------------------------
	/**
	 * Start method that finds the HUD components.
	 */ 
	void Start () {
		speedSlider = GameObject.Find ("SpeedSlider").GetComponent<Slider>();
		hpSlider = GameObject.Find ("HealthPointsSlider").GetComponent<Slider> ();
		scoreBoard = GameObject.Find ("ScoreBoardPanel");
		scoreBoard.SetActive (false);
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.Tab)) {
			scoreBoard.SetActive (true);
		}
		if (Input.GetKeyUp (KeyCode.Tab)) {
			scoreBoard.SetActive (false);
		}
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Cursor.visible = !Cursor.visible;
		}
	}

	/**
	 * Method that receives the current health to display and updates the HUD.
	 */ 
	public void UpdateHealthSlider(float health) {
		hpSlider.value = health;
	}

	/**
	 * Method that receives the current speed to display and updates the HUD.
	 */
	public void UpdateSpeedSlider(float speed) {
		float speedRatio = (speed * 100f) / 150f;
		speedSlider.value = speedRatio;
	}
}
