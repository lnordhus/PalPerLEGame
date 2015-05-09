using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed;
	
	private Rigidbody rb;
	
	void Start ()
	{
		rb = GetComponent<Rigidbody>();
	}


	void FixedUpdate(){
		var moveHorizontal = Input.GetAxis ("Horizontal");
		var moveVertical = Input.GetAxis ("Vertical");
		var movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		rb.AddForce (movement * speed);
	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.tag == "Pick Up")
		{
			other.gameObject.SetActive (false);
		}
	}
}
