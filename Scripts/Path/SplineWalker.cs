using UnityEngine;

public class SplineWalker : MonoBehaviour {

	private SinglePlayerBootController singlePlayerBoot1Controller;
	private BezierSpline spline;
	[Header ("Patrol Path")]
	[SerializeField]
	private BezierSpline patrolPath;

	[Header ("Capture Paths")]
	[SerializeField]
	private BezierSpline captureBalloon1Path;
	[SerializeField]
	private BezierSpline captureBalloon2Path;
	[SerializeField]
	private BezierSpline captureBalloon3Path;

	private BezierSpline prevSpline;
	private BezierSpline korkociag;
	private BezierSpline petla;
	private BezierSpline beczka;

	[Header ("Escape Paths")]
	[SerializeField]
	private GameObject korkociagGO;
	[SerializeField]
	private GameObject petlaGO;
	[SerializeField]
	private GameObject beczkaGO;


//	[SerializeField]
//	private Transform laserSpawnPoint;

	private bool isAttacking;
	private bool isEscaping;
	public bool IsEscaping
	{
		get { return isEscaping; }
	}

	private float duration;

	//public bool lookForward;

	private SplineWalkerMode mode;

	private float progress;
	private bool goingForward = true;
	private bool isOnPath = false;
	private Vector3 currentPosition;
	[SerializeField]
	private float speed = 200f;
	private float t = 0f;
	private float MinDistToPlayer = 30f;
	private GameObject currentEscapePath;

	void Start() {
		spline = patrolPath;
		duration = spline.CalculateDuration (speed);
		progress = 0;
		currentPosition = GetCurrentPosition();
		singlePlayerBoot1Controller = GetComponent<SinglePlayerBootController> ();
	}

	private void FixedUpdate () {

		if (!isAttacking) { // nie atakuje
			if (!isOnPath) {
				//Debug.Log ("Lece do sciezki " + spline.name);
				GetToPath ();

			} else {
				
				progress += Time.deltaTime / duration;

				if (mode == SplineWalkerMode.Loop) {
					if (progress > 1f) {
						progress -= 1f;
					}
					Vector3 position = spline.GetPoint (progress);
					GetComponent<Rigidbody> ().position = position;
					transform.LookAt (position + spline.GetDirection (progress));
					//GetComponent<Rigidbody>().position += transform.forward * speed * Time.deltaTime;

				} else if (mode == SplineWalkerMode.Once) {
					if (progress > 1f) {
						//Debug.Log ("koniec ucieczki");
						progress = 0f;	// wczesniej 1f;
						//SetPath("patrol");
						singlePlayerBoot1Controller.Patrol ();
						Destroy (currentEscapePath);
						isOnPath = false;
						isEscaping = false;
						spline = prevSpline;
						mode = SplineWalkerMode.Loop;
					} else {
						Vector3 position = spline.GetPoint (progress);
						GetComponent<Rigidbody> ().position = position;
						transform.LookAt (position + spline.GetDirection (progress));
						//GetComponent<Rigidbody>().position += transform.forward * speed * Time.deltaTime;
					}
				}
			}
		}
	}

	public void SetPath(string pathName) {
		switch (pathName) {
		case "death": 
			spline = prevSpline;
			isOnPath = false;
			isAttacking = false;
			isEscaping = false;
			break;
		case "followPlayer":
			isAttacking = true;
			break;
		case "patrol":
			if (spline != patrolPath) {
				prevSpline = patrolPath;
				spline = patrolPath;
				duration = spline.CalculateDuration (speed);
				mode = SplineWalkerMode.Loop;
				progress = 0f;
			
				isOnPath = false;
				isAttacking = false;
			}
			break;
		case "captureBalloon1":
			if (spline != captureBalloon1Path) {
				prevSpline = captureBalloon1Path;
				spline = captureBalloon1Path;
				duration = spline.CalculateDuration (speed);
				mode = SplineWalkerMode.Loop;
				progress = 0f;
			
				isOnPath = false;
				isAttacking = false;
			}
			break;
		case "captureBalloon2":
			if (spline != captureBalloon2Path) {
				prevSpline = captureBalloon2Path;
				spline = captureBalloon2Path;
				duration = spline.CalculateDuration (speed);
				mode = SplineWalkerMode.Loop;
				progress = 0f;
			
				isOnPath = false;
				isAttacking = false;
			}
			break;
		case "captureBalloon3":
			if (spline != captureBalloon3Path) {
				prevSpline = captureBalloon3Path;
				spline = captureBalloon3Path;
				duration = spline.CalculateDuration (speed);
				mode = SplineWalkerMode.Loop;
				progress = 0f;
			
				isOnPath = false;
				isAttacking = false;
			}
			break;
		case "escape1":
			if (spline != korkociag && spline != petla && spline != beczka) {
				prevSpline = spline;
				currentEscapePath = (GameObject)Instantiate (korkociagGO, transform.position, transform.rotation);
				spline = currentEscapePath.GetComponent<BezierSpline>();
				duration = spline.CalculateDuration (speed);
				mode = SplineWalkerMode.Once;
				progress = 0f;
				isEscaping = true;
				isOnPath = false;
				isAttacking = false;
			}
			break;
		case "escape2":
			if (spline != korkociag && spline != petla && spline != beczka) {
				prevSpline = spline;
				currentEscapePath = (GameObject)Instantiate (petlaGO, transform.position, transform.rotation);
				spline = currentEscapePath.GetComponent<BezierSpline>();
				duration = spline.CalculateDuration (speed);
				mode = SplineWalkerMode.Once;
				progress = 0f;
				isEscaping = true;
				isOnPath = false;
				isAttacking = false;
			}
			break;
		case "escape3":
			if (spline != korkociag && spline != petla && spline != beczka) {
				prevSpline = spline;
				currentEscapePath = (GameObject)Instantiate (beczkaGO, transform.position, transform.rotation);
				spline = currentEscapePath.GetComponent<BezierSpline>();
				duration = spline.CalculateDuration (speed);
				mode = SplineWalkerMode.Once;
				progress = 0f;
				isEscaping = true;
				isOnPath = false;
				isAttacking = false;
			}
			break;
		}
	}

	private void FollowPath( BezierSpline path) {
		spline = path;
		duration = spline.CalculateDuration (speed);
		isEscaping = true;
		t = 0;
		mode = SplineWalkerMode.Once;
	}
		

	private Vector3 GetCurrentPosition() {
		return transform.position;
	}

	private void GetToPath() {
		Vector3 startOfThePath = spline.GetPoint (0);
		transform.LookAt (startOfThePath);
		//GetComponent<Rigidbody>().position += transform.forward * speed * Time.deltaTime;
		GetComponent<Rigidbody>().position = Vector3.MoveTowards(transform.position, startOfThePath, speed * Time.deltaTime);
		if (startOfThePath == GetCurrentPosition ()) {
			isOnPath = true;
			//Debug.Log ("IS ON PATH = " + isOnPath + " PATH = " + spline.name);
		}
	}
		
	public void GoToPlayer(Vector3 playerPosition) {
		transform.LookAt (playerPosition);
		if(Vector3.Distance(transform.position, playerPosition) >= MinDistToPlayer){
			GetComponent<Rigidbody>().position += transform.forward * speed/2 * Time.deltaTime;
		//GetComponent<Rigidbody>().position = Vector3.MoveTowards(transform.position, new Vector3(playerPosition.x-20f, playerPosition.y, playerPosition.z-20f), (speed * 0.8f) * Time.deltaTime);
		}
	}
}