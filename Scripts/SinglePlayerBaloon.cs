using UnityEngine;
using System.Collections;
/// <summary>
/// Single player baloon.
/// Changing color of baloon when baloonPoint are lower then 100 to red 
/// and for blue when are higher
/// </summary>
public class SinglePlayerBaloon : MonoBehaviour {

	// reference to SinglePlayer Game Controller
	// add using tric with 2 inspectors
	[SerializeField]
	private SinglePlayerGameController singlePlayerGameController;
	[SerializeField]
	private SinglePlayerController singlePlayerController;
	[SerializeField]
	private SinglePlayerBootController singlePlayerBoot1Controller;
	[SerializeField]
	private SinglePlayerBootController singlePlayerBoot2Controller;
	[SerializeField]
	private SinglePlayerBootController singlePlayerBoot3Controller;
	[SerializeField]
	private GameObject baloonGO;

	//materials
	[Header ("Ballon Materials")]
	[SerializeField]
	private Material blue;
	[SerializeField]
	private Material red;
	[SerializeField]
	private Material white;
	[SerializeField]
	private float baloonPoint;
	private float minPoint = 0;
	private float maxPoint = 200;

	private bool playerInside;
	private bool boot1Inside;
	private bool boot2Inside;
	private bool boot3Inside;

	private float playerTimer;
	private float bot1Timer;
	private float bot2Timer;
	private float bot3Timer;

	private bool isCapturedByEnemy;
	public bool IsCapturedByEnemy {
		get { return isCapturedByEnemy; }
	}

	[SerializeField]
	private float timeBetween = 0.1f;

	// Use this for initialization
	void Start () {
		SetBaloonDefaultValue ();
	}
	
	// Update is called once per frame
	void Update () {
		playerTimer += Time.deltaTime;
		bot1Timer += Time.deltaTime;
		bot2Timer += Time.deltaTime;
		bot3Timer += Time.deltaTime;

		if (playerInside && playerTimer > timeBetween) {
			playerTimer = 0f;
			if (baloonPoint < maxPoint ) {
				baloonPoint++;
				singlePlayerController.UpdatePoints (1);
				if (baloonPoint == maxPoint) {
					singlePlayerController.UpdatePoints (100);
				}
			} 
		}

		if (boot1Inside && bot1Timer > timeBetween) {
			
			bot1Timer = 0f;
			if (baloonPoint > minPoint) {
				baloonPoint--;
				singlePlayerBoot1Controller.UpdatePoints (1);
				if (baloonPoint == minPoint) {
					singlePlayerBoot1Controller.UpdatePoints (100);
				}
			} 
		}
		if (boot2Inside && bot2Timer > timeBetween) {

			bot2Timer = 0f;
			if (baloonPoint > minPoint) {
				baloonPoint--;
				singlePlayerBoot2Controller.UpdatePoints (1);
				if (baloonPoint == minPoint) {
					singlePlayerBoot2Controller.UpdatePoints (100);
				}
			} 
		}
		if (boot3Inside && bot3Timer > timeBetween) {

			bot3Timer = 0f;
			if (baloonPoint > minPoint) {
				baloonPoint--;
				singlePlayerBoot3Controller.UpdatePoints (1);
				if (baloonPoint == minPoint) {
					singlePlayerBoot3Controller.UpdatePoints (100);
				}
			} 
		}
	

		if (baloonPoint == maxPoint) {
			isCapturedByEnemy = true;
			baloonGO.GetComponent <MeshRenderer> ().material = blue;
			singlePlayerGameController.UpdateScoreTeamRed (-1);
		} else if (baloonPoint == minPoint) {
			isCapturedByEnemy = false;
			baloonGO.GetComponent <MeshRenderer> ().material = red;
			singlePlayerGameController.UpdateScoreTeamBlue (-1);
		}
	}

	/// <summary>
	/// Sets the baloon default value.
	/// </summary>
	public void SetBaloonDefaultValue()
	{
		baloonPoint = 100;
		baloonGO.GetComponent <MeshRenderer>().material = white;
		playerInside = false;
		boot1Inside = false;
		boot2Inside = false;
		boot3Inside = false;
		isCapturedByEnemy = true;
	}
		
	/// <summary>
	/// Raises the trigger enter event.
	/// </summary>
	/// <param name="collider">Collider.</param>
	void OnTriggerEnter (Collider collider) {

		if (collider.gameObject.CompareTag ("Player")) {
			playerInside = true;
		} else if (collider.gameObject.CompareTag ("Boot1")) {
			boot1Inside = true;
		} else if (collider.gameObject.CompareTag ("Boot2")){
			boot2Inside = true;
		} else if (collider.gameObject.CompareTag ("Boot3")){
			boot3Inside = true;
		}
	}

	/// <summary>
	/// Raises the trigger exit event.
	/// </summary>
	/// <param name="collider">Collider.</param>
	void OnTriggerExit (Collider collider) {
 
		if (collider.gameObject.CompareTag ("Player")) {
			playerInside = false;
		} else if (collider.gameObject.CompareTag ("Boot1")) {
			boot1Inside = false;
		} else if (collider.gameObject.CompareTag ("Boot2")){
			boot2Inside = false;
		} else if (collider.gameObject.CompareTag ("Boot3")){
			boot3Inside = false;
		}
	}
}
			

		
