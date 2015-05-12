using UnityEngine;
using System.Collections;
using RTS;

public class CameraController : MonoBehaviour {

	private Player player;
	public int speed;

	void Start()
	{
		player = transform.root.GetComponent<Player> ();
	}
	
	void Update () {

		if (player.isHuman) 
		{
			KeyboardMoveCamera ();
		}
	}

	//Moves camera with keyboard keys along world plane
	private void KeyboardMoveCamera()
	{
		float h = speed * Time.deltaTime * Input.GetAxis ("Horizontal");
		float v = speed * Time.deltaTime * Input.GetAxis ("Vertical");
		Vector3 moveDir = new Vector3 (h, 0f, v);
		Vector3 camFwd = Camera.main.transform.forward;
		Vector3 camFwdProj = Vector3.ProjectOnPlane (camFwd, Vector3.up);
		Quaternion camFwdRot = Quaternion.LookRotation (camFwdProj, Vector3.up);
		Vector3 movement = camFwdRot * moveDir;
		Camera.main.transform.Translate (movement, Space.World);
	}
}
