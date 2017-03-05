using UnityEngine;
using System.Collections;

public class LaserMotor : MonoBehaviour {

	// Storing variables for the laser motor
	private float laserSpeed = 1200f;

	//-------------------------------------------------
	/**
	 * Update method that moves the laser forward with the defined speed.
	 */ 
	void Update () {
		transform.position += transform.forward * Time.deltaTime * laserSpeed;
	}
}
