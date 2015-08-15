using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class HelpersMethodes {

	public static GameObject GetGameObject(){		//Returnerer objektet musen holder over
		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit ObjectTag;
		if (Physics.Raycast (camRay, out ObjectTag, 1000)) { 
			return ObjectTag.transform.gameObject;

			
		}
		return null;
	}

	public static List <StaticObjects> InitiateAllGameObjects(){		//Returnerer alle objekter av typen Building (hittil LumberCamp og Tree)
		var ret = new List <StaticObjects>();
		foreach (var element in Object.FindObjectsOfType (typeof(StaticObjects))) {
			var mycastedObject = element as StaticObjects;
			if(mycastedObject != null)
				ret.Add(mycastedObject);
		}
		return ret;
	}

	public static Vector3 GetMousePosition(){					//returnerer positionen til musa.
		var Mouseposition = new Vector3 ();
		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit floorhit;
		if (Physics.Raycast (camRay, out floorhit, 1000)) { 
			Mouseposition = floorhit.point;
			return Mouseposition;

		}
		return Mouseposition;
	}

//	public static Object[] UpdateListOfGameObjects (object[] Liste){
//
//	}
}
