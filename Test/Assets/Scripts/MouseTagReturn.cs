using UnityEngine;
using System.Collections;

public class MouseTagReturn : MonoBehaviour {

	RaycastHit hit;
	private float camRayLength = 10000f;
	public string MouseTag = "";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	string Update () {
		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit ObjectTag;
		if (Physics.Raycast (camRay, out ObjectTag, camRayLength)) { 
			MouseTag = ObjectTag.transform.gameObject.name;

		}
		return MouseTag;
	}
}
