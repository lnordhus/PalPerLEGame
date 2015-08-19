using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	Animator anim;
	RaycastHit hit;

	private float fallSpeed;
	private float horizontalSpeed;
	private float verticalSpeed;
	private int floorMask;
	private float camRayLength = 10000f;

	public float speed;
	public float jump;

	protected Rigidbody rb;
	protected Vector3 GoalPos;
	protected float dist;

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
			if (Physics.Raycast (camRay, out floorHit, camRayLength)) { 
				GoalPos = floorHit.point;
				GoalPos.y = transform.position.y;

				GameObject cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
				 
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

	protected virtual void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.tag == "Pick Up")
		{
			other.gameObject.SetActive (false);
		}
	}

	//beveger player
	protected virtual void MovePlayer (Vector3 walkVector){
		dist = (transform.position - GoalPos).sqrMagnitude;
		var dist2 = (transform.position - GoalPos).magnitude;
		if (dist2 > 0.1f) {
			rb.MovePosition (transform.position + (walkVector * speed * Time.deltaTime));//
		}
	}

	//Animer spiller
	protected virtual void Animating (float dist)
	{
		// Tell the animator whether or not the player is walking.
		anim.SetBool ("Walk", ShouldWalk (dist));
	}

	protected virtual bool ShouldWalk(float dist){
		// Create a boolean that is true if either of the input axes is non-zero.
		return dist >= 0.5f;
	}
}
