using UnityEngine;
using System.Collections;

public class SinglePlayerPlanePilot : MonoBehaviour {

	// Storing the speed variables
	[SerializeField]
	private float speed = 35f;
	[SerializeField]
	private float minSpeed = 30f;
	[SerializeField]
	private float maxSpeed = 150f;

	// Storing the mouse input
	private float mouseY = 0f;
	private float mouseX = 0f;

	// Storing keyboard input.
	private float keyboardRotation = 0f;

	[SerializeField]
	Camera playerCamera;

	[SerializeField]
	private SinglePlayerMenuController singlePlayerMenuController;

	//---------------------------------------------------------------------------
	void Start () {
		// Finding the hud controller script in the components
		//hudController = GetComponent<HUDController> ();
	}

	// Update is called once per frame
	void Update () {
		
		// Storing input variables
		float speedMovement = Input.GetAxisRaw ("Vertical");
		float mouseYMovement = Input.GetAxis ("Mouse Y");
		float mouseXMovement = Input.GetAxis ("Mouse X");
		float keyboardRotation = Input.GetAxisRaw ("Horizontal");

		// Informing the pilot to execute changes
		ControlSpeed (speedMovement);
		ControlRotation (mouseYMovement, mouseXMovement);
		ControlKeyboardRotation (keyboardRotation);


		// Performing control methods every frame
		CameraSettings ();
		KeepOffTerrain ();
		PerformMovement ();
		PerformRotation ();
		PerformKeyboardRotation ();
	}

	/**
	 * Receiver method to control player's speed. 
	 * The speed cannot be higher than defined.
	 * Updating the HUD with the speed change.
	 */
	public void ControlSpeed(float receivedSpeed) {
		if (speed > maxSpeed) {
			speed = maxSpeed;
		} else if (speed < minSpeed) {
			speed = minSpeed;
		} else {
			speed += receivedSpeed;
		}
		singlePlayerMenuController.UpdatespeedSlider ((int)speed);
		//hudController.UpdateSpeedSlider (speed);
	}

	/**
	 * Receiver method to control the player's rotation.
	 */ 
	public void ControlRotation(float receivedY, float receivedX) {
		mouseY = receivedY/2;
		mouseX = receivedX/2;
	}

	/**
	 * Receiver method to control the player's rotation from keyboard input.
	 */ 
	public void ControlKeyboardRotation(float receivedKeyboardRotation) {
		keyboardRotation = receivedKeyboardRotation/2;
	}

	/**
	 * Method to perform the movement of the player (speed up, slow down). 
	 */
	private void PerformMovement() {
		//transform.position += transform.forward * Time.deltaTime * speed;
		GetComponent<Rigidbody> ().position += transform.forward * Time.deltaTime * speed;
	}

	/**
	 * Method to perform the rotation of the player (up, right, bottom, left).
	 */ 
	private void PerformRotation() {
		transform.Rotate (mouseY, 0.0f,  -mouseX);
	}

	/**
	 * Method to perform the rotation of the player from keyboard input.
	 */ 
	private void PerformKeyboardRotation() {
		if (keyboardRotation > 0) {
			transform.Rotate (0, keyboardRotation, 0);
		} else if (keyboardRotation < 0) {
			transform.Rotate (0, keyboardRotation, 0);
		}
	}

	/**
	 * Method for camera movement.
	 * The camera must follow the player at all times, it is transformed through a Vector3.
	 */ 
	private void CameraSettings() {
		Vector3 moveCamTo = transform.position - transform.forward * 20.0f + Vector3.up * 10.0f;
		float bias = 0.96f;
		playerCamera.transform.position = playerCamera.transform.position * bias + moveCamTo * (1.0f - bias);
		playerCamera.transform.LookAt (transform.position + transform.forward * 30.0f);
	}

	/**
	 * Method to keep the player off the terrain.
	 */ 
	private void KeepOffTerrain() {
		float terrainHeightWhereWeAre = Terrain.activeTerrain.SampleHeight (transform.position) +3.0f;
		if (terrainHeightWhereWeAre > transform.position.y) {
			transform.position = new Vector3 (transform.position.x, terrainHeightWhereWeAre, transform.position.z);
		}
	}
}
