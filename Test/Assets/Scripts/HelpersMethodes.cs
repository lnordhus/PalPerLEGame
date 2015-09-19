using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public static class HelpersMethodes {

	public static GameObject GetGameObject(){		//Returnerer objektet musen holder over
		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit ObjectTag;
		if (Physics.Raycast (camRay, out ObjectTag, 1000)) { 
			return ObjectTag.transform.gameObject;

			
		}
		return null;
	}

	public static List <StaticObjects> InitiateAllStaticGameObjects(){		//Returnerer alle objekter av typen StaticObjects
		var ret = new List <StaticObjects>();
		foreach (var element in Object.FindObjectsOfType (typeof(StaticObjects))) {
			var mycastedObject = element as StaticObjects;
			if(mycastedObject != null)
				ret.Add(mycastedObject);
		}
		return ret;
	}

	public static List <PlayerController> InitiateAllDynamicGameObjects(){		//Returnerer alle objekter av typen PlayerController
		var ret = new List <PlayerController>();
		foreach (var element in Object.FindObjectsOfType (typeof(PlayerController))) {
			var mycastedObject = element as PlayerController;
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

	public static Texture2D LoadPNG(string filePath) {		//Leser av png-fil og returnerer en array med bildet 
		
		Texture2D tex = null;
		byte[] fileData;
		
		if (File.Exists(filePath))     {
			fileData = File.ReadAllBytes(filePath);
			tex = new Texture2D(2, 2);
			tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
		}
		return tex;
	}

}
