using UnityEngine;

[RequireComponent(typeof(PlanePilot))]
public class PlayerController : MonoBehaviour {

	// Storing an instance of the plane pilot (player motor)
	private PlanePilot pilot;

	//-----------------------------------------------------------
	void Start ()
	{
		// Finding the plane pilot script in the components
		pilot = GetComponent<PlanePilot>();
	}

	void Update ()
	{
		// Storing input variables
		float speedMovement = Input.GetAxisRaw ("Vertical");
		float mouseYMovement = Input.GetAxis ("Mouse Y");
		float mouseXMovement = Input.GetAxis ("Mouse X");
		float keyboardRotation = Input.GetAxisRaw ("Horizontal");

		// Informing the pilot to execute changes
		pilot.ControlSpeed (speedMovement);
		pilot.ControlRotation (mouseYMovement, mouseXMovement);
		pilot.ControlKeyboardRotation (keyboardRotation);
	}

}