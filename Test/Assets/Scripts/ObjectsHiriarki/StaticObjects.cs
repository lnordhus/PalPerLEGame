using System;
using UnityEngine;


public class StaticObjects : MonoBehaviour{

	public StaticObjects [] buildings;
	private GameObject buildingPlacement;
	private StaticObjects currentBuilding;
	private bool hasPlaced;
	
	public StaticObjects (){

	}

	void Update (){
		buildingPlacement = HelpersMethodes.GetGameObject ();
		if (currentBuilding != null && !hasPlaced) {
			var mousePos = HelpersMethodes.GetMousePosition();
			currentBuilding.transform.position = new Vector3(mousePos.x,currentBuilding.transform.position.y,mousePos.z);
			if (Input.GetMouseButtonDown (0)) {
				hasPlaced = true;
				Worker.ListOfGameObjects.Add((StaticObjects)currentBuilding);
			}
		}
	}

	void OnGUI ()  {
		for (int i = 0; i < buildings.Length; i++) {
			if (GUI.Button(new Rect(Screen.width/20, Screen.height/15 + Screen.height/12 *i, 100, 30), buildings[i].name )){
				SetItem(buildings[i]);

			}
		}
	}

	public void SetItem(StaticObjects b){
		hasPlaced = false;
		currentBuilding = ((StaticObjects)Instantiate(b));
		Debug.Log (b);
	}
}


