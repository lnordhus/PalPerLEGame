using UnityEngine;
using System.Collections;

public static class MouseHelper {

	public static string GetMouseTag(){
		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit ObjectTag;
		if (Physics.Raycast (camRay, out ObjectTag, 1000)) { 
			return ObjectTag.transform.gameObject.tag;
			
		}
		return "";
	}

}
