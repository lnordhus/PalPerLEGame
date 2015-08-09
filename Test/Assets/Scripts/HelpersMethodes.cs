using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class HelpersMethodes {

	public static string GetMouseTag(){
		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit ObjectTag;
		if (Physics.Raycast (camRay, out ObjectTag, 1000)) { 
			//Debug.Log (ObjectTag.transform.gameObject.tag);
			return ObjectTag.transform.gameObject.tag;

			
		}
		return "";
	}

	public static Object[] InitiateAllGameObjects(){
		return Object.FindObjectsOfType (typeof(LumberCamp));
	}

//	public static Vector3 GetClosestObject(){
//		LumberCamp[] GameObjects = Object.FindObjectsOfType(typeof(LumberCamp));
//	}
}
