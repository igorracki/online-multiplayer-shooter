using UnityEngine;
using System.Collections;

public class SinglePlayer : MonoBehaviour {

	private bool isDead;

	// Variable storing the instance of the hud controller.
	private HUDController hudController;

	// Variable storing the player's health.
	[SerializeField]
	private int maxHealth = 100;

	// Network-synchronized variable storing the player's current health.
	private int currentHealth;


	// Use this for initialization
	void Start () {
		// Finding the hud controller in the components.
		hudController = GetComponent<HUDController> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/**
	 * Method to set the default attributes for the player.
	 */ 
	public void SetDefaults () {
		isDead = false;

		currentHealth = maxHealth;
		hudController.UpdateHealthSlider (currentHealth);


		Collider colider = GetComponent<Collider>();
		if (colider != null) {
			colider.enabled = true;
		}
	}




}
