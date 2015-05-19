using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private Rigidbody rb;
	RaycastHit hit;
	public float speed;
	public float jump;

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
		Vector3 walkVector = new Vector3 (0, 0, 0);
	}

	void FixedUpdate()
	{

		//float moveHorizontal = Input.GetAxisRaw ("Horizontal");
		//float moveVertical = Input.GetAxisRaw ("Vertical");

		//Vector3 down = new Vector3 (0, -1, 0);
		//Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

		//lagrer bevegelse hvis i luften
		//if (Physics.Raycast (transform.position, down, out hit, dist) == true) 
		//{
		//	horizontalSpeed = moveHorizontal;
		//	verticalSpeed = moveVertical;
		//}

		//hopper
		//if (Input.GetKey ("space") && Physics.Raycast(transform.position,down,out hit,dist) == true)
		//{ 
		//
		//	rb.velocity = new Vector3 (0 , jump , 0);
		//}
		//Hvis hopper så fortset x og z bevegelse
		//if (Physics.Raycast(transform.position,down,out hit,dist) == false)
		//	{
		//	movement = new Vector3 (0,0,0);
		//	movement = new Vector3 (horizontalSpeed,0,verticalSpeed);
		//
		//	}


		//beregner fallhastighet
		fallSpeed = rb.velocity.y;

		//finner hvor du klikker venstre musetast på terrain og lager en boks
		if (Input.GetMouseButtonDown (0)) {
			Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit floorHit;
			if (Physics.Raycast (camRay, out floorHit, camRayLength, floorMask)) {
				Vector3 playerToMouse = floorHit.point - transform.position;
				playerToMouse.y = 0f;
				GameObject cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
				cube.transform.position = playerToMouse;
				cube.tag = "Pick Up";
				Destroy (cube, 5);
				walkVector = rb.position + playerToMouse;
				print ("safas");
			}

		} 
		else 
		{
			walkVector = new Vector3 (0f, 0f, 0f);
		}

		
		//roterer figur
		if (walkVector != rb.position) {
			Quaternion retning = Quaternion.LookRotation (walkVector);
			rb.MoveRotation (retning);
		}
		
		//beveger player
		rb.MovePosition (rb.position + (walkVector.normalized * speed * Time.deltaTime));
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
