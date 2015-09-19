using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Threading;

public class PlayerController : MonoBehaviour {

	Animator anim;
	RaycastHit hit;

	public float speed;
	public float jump;
	//public GameObject Marker;
	
	private float fallSpeed;	
	private float horizontalSpeed;
	private float verticalSpeed;
	private int floorMask;
	private float camRayLength = 10000f;

	protected Rigidbody rb;
	protected Vector3 GoalPos;
	protected float dist;

	//private float yBound = 0.01;
	
	protected virtual void Start()
	{ 
		rb = GetComponent<Rigidbody> ();
		floorMask = LayerMask.GetMask ("Terrain"); 
		GoalPos = rb.position;

		anim = GetComponent <Animator> ();
	}


	void FixedUpdate()
	{
		//beregner fallhastighet
		fallSpeed = rb.velocity.y;

		//finner hvor du klikker hoyre musetast på terrain og lager en boks
		if (Input.GetMouseButtonDown (1)) {
			Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit floorHit;
			if (Physics.Raycast (camRay, out floorHit, camRayLength, floorMask)) { 
				GoalPos = floorHit.point;
				GoalPos.y = transform.position.y;

				//GameObject cube = GameObject.CreatePrimitive (PrimitiveType.Cube);

				//var marker = Instantiate(Marker,GoalPos,new Quaternion());
				//marker.transform.position = GoalPos;
				//marker.tag = "Pick Up";
				//Destroy (marker, 5);  
			}
		}    
		var walkVector = (GoalPos - transform.position).normalized;
		walkVector.y = 0f;
		
		//roterer figur
		//if ( walkVector.sqrMagnitude>1f) {
			Quaternion retning = Quaternion.LookRotation (walkVector);
			rb.MoveRotation (retning);
		//}
		

		MovePlayer (walkVector);
		Animating (dist);

	}

	void OnGUI()
	{
		GUI.Box(new Rect(10,10,100,90), "Measurements");
		
		GUI.Label(new Rect(20,40,80,20), fallSpeed + "m/s");

	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.tag == "Pick Up")
		{
			other.gameObject.SetActive (false);
		}
	}
	private bool _pathCreationStarted;
	private Vector3 myPos;
	private List<Waypoint> Path;
	//beveger player
	protected virtual void MovePlayer (Vector3 walkVector){
		dist = (transform.position - GoalPos).sqrMagnitude;
		var dist2 = (transform.position - GoalPos).magnitude;
		if (dist2 > 1f) {
			if (Path == null && !_pathCreationStarted) {	
				myPos = transform.position;
				new Thread(UpdatePath).Start();
			}
			 
			var firstWaypoint = HelpersMethodes.FirsOrDefault (Path, x => !x.hasVisited);
			if (firstWaypoint != null) {
				firstWaypoint.hasVisited = true;
				rb.MovePosition (firstWaypoint.pos);
			}			
			//rb.MovePosition (transform.position + (walkVector * speed * Time.deltaTime));//
		} else {
			Path = null;
		}
	}

	private void UpdatePath(){
		_pathCreationStarted = true;
		Path = HelpersMethodes.CreatePath (GoalPos, myPos);
		_pathCreationStarted = false;
	}

	//Animer spiller
	void Animating (float dist)
	{
		// Create a boolean that is true if either of the input axes is non-zero.
		bool walking = dist >= 0.5f;

		if (walking) {
			Debug.Log ("hoa");
		}

		// Tell the animator whether or not the player is walking.
		anim.SetBool ("Walk", walking);
	}
}
