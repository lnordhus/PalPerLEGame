using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private Rigidbody rb;
	RaycastHit hit;
	public float speed;
	public float jump;
	public GameObject Marker;

	private float fallSpeed;
	private float horizontalSpeed;
	private float verticalSpeed;
	private float dist;
	private int floorMask;
	private float camRayLength = 10000f;

	//private float yBound = 0.01;
	
	void Start()
	{ 
		rb = GetComponent<Rigidbody> ();
		dist = 0.6f;
		floorMask = LayerMask.GetMask ("Terrain"); 
		GoalPos = rb.position;
	}

	Vector3 GoalPos;
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

				GameObject cube = GameObject.CreatePrimitive (PrimitiveType.Cube);

				var marker = Instantiate(Marker,GoalPos,new Quaternion());
				//marker.transform.position = GoalPos;
				//marker.tag = "Pick Up";
				Destroy (marker, 5);  
			}

		}    
		var walkVector = (GoalPos - transform.position).normalized;
		walkVector.y = 0f;
		
		//roterer figur
		//if ( walkVector.sqrMagnitude>1f) {
			Quaternion retning = Quaternion.LookRotation (walkVector);
			rb.MoveRotation (retning);
		//}
		
		//beveger player
		var dist = (transform.position - GoalPos).sqrMagnitude;
		var dist2 = (transform.position - GoalPos).magnitude;
		if (dist2 > 0.1f) {
			rb.MovePosition (transform.position + (walkVector * speed * Time.deltaTime));//
		}

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


	
}
