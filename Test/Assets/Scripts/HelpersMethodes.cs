using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class HelpersMethodes {

	public static GameObject GetGameObject(){		//Returnerer objektet musen holder over
		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit ObjectTag;
		if (Physics.Raycast (camRay, out ObjectTag, 1000)) { 
			//Debug.Log (ObjectTag.transform.gameObject.tag);
			return ObjectTag.transform.gameObject;

			
		}
		return null;
	}

	public static Object[] InitiateAllGameObjects(){		//Returnerer alle objekter av typen LumberCamp
		return Object.FindObjectsOfType (typeof(LumberCamp));
	}
	
}
