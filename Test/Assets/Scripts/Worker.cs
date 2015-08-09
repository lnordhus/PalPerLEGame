using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

public class Worker : PlayerController {

	protected Object[] GameObjects;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		GameObjects = HelpersMethodes.InitiateAllGameObjects();
	}

	private float TimePast = 0;
	private int WoodCollected = 0;
	private bool IsCuttingWood;
	private bool IsWoodCutter;

	// Update is called once per frame
	void Update () {
		if (dist <= 2 && IsWoodCutter && WoodCollected != 1){
			Debug.Log ("Chopping tree");
			IsCuttingWood = true;
			TimePast += Time.deltaTime;

			if (TimePast >= 5){
				WoodCollected = 1;
				IsCuttingWood = false;
				Debug.Log ("Tree chopped");
				SetGoalObject<LumberCamp>();
			}
		}
		if ( Input.GetMouseButtonDown (1) && HelpersMethodes.GetMouseTag () == "Tree" ){
			Debug.Log ("ChoppChopp");
			IsWoodCutter = true;

		}

	}

	protected override void MovePlayer (Vector3 walkVector){
		if (IsCuttingWood == true) {	
			return;
		}
		base.MovePlayer(walkVector);
	}

	private void SetGoalObject <type> () where type : Building {
		var ShortestGoalPosition = new Vector3 ();
		var myPosition = rb.transform.position;
		var shortestDistFound = float.MaxValue;

		foreach (var element in GameObjects) {
			var correctTypeOfBuilding = element as type;
			if (correctTypeOfBuilding != null) {

				var dist =  (correctTypeOfBuilding.transform.position - myPosition).sqrMagnitude; 		//hvor langt er det fra element til meg

				if(dist < shortestDistFound){										//hvis element er nærmere, dvs dist er lavere enn tidligere funnet
					shortestDistFound = dist;										//sett element til å være beste kandidat og sett hittil beste avstand til dist
					ShortestGoalPosition = correctTypeOfBuilding.transform.position;
				}
			}
		}
		GoalPos = ShortestGoalPosition;
	} //correctTypeOfBuilding.transform.position;
}
