using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Worker : PlayerController {

	private float TimePast = 0;
	private int WoodCollected = 0;
	private bool IsWorking;					//True når gjør en jobb
	private bool IsWoodCutter;				//True når worker går mot tre
	private bool IsCarryingRescourse;			//True når worker bærer ressurser
	private Tree TreeImChopping;
	private StaticObjects GoalObject;

	protected List <StaticObjects> ListOfGameObjects;		//Masterlisten med posisjon over alle objekter

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		ListOfGameObjects = HelpersMethodes.InitiateAllGameObjects();
	}
	
	// Update is called once per frame
	void Update () {

		//hugge trær
		if ( Input.GetMouseButtonDown (1) && HelpersMethodes.GetGameObject ().tag == "Tree" ){
			Debug.Log ("ChoppChopp");
			IsWoodCutter = true;
			var clickedObject = HelpersMethodes.GetGameObject();
			var existingObject = ListOfGameObjects.Find(x=>x.gameObject.GetInstanceID() == clickedObject.GetInstanceID());
			TreeImChopping = ((Tree)existingObject);
		}

		if (dist <= 4 && IsWoodCutter && WoodCollected != 1){
			Debug.Log ("Chopping tree");
			IsWorking = true;

			TimePast += Time.deltaTime;
			
			if (TreeImChopping != HelpersMethodes.GetGameObject () && Input.GetMouseButtonDown (1)){
				TimePast = 0;
				IsWorking = false;
			}
			if (TimePast >= 1){
				WoodCollected = 1;
				TreeImChopping.gameObject.SetActive (false);
				IsWorking = false;
				Debug.Log ("Tree chopped");
				SetGoalObject<LumberCamp>();
				TimePast = 0;
			}
		} 

	}

	protected override void MovePlayer (Vector3 walkVector){
		if (IsWorking == true) {
			return;
		}
		base.MovePlayer(walkVector);
	}

	protected override bool ShouldWalk(float dist){
		// Create a boolean that is true if either of the input axes is non-zero.
		return base.ShouldWalk(dist) && !IsWorking;
	}

	private void SetGoalObject <type> () where type : StaticObjects {
		var ShortestGoalPosition = new Vector3 ();
		var myPosition = rb.transform.position;
		var shortestDistFound = float.MaxValue;
		StaticObjects tempGoalObject = null;

		foreach (var element in ListOfGameObjects) {
			var correctTypeOfStaticObject = element as type;
			if (correctTypeOfStaticObject != null) {

				var dist =  (correctTypeOfStaticObject.transform.position - myPosition).sqrMagnitude; 		//hvor langt er det fra element til meg

				if(dist < shortestDistFound){										//hvis element er nærmere, dvs dist er lavere enn tidligere funnet
					shortestDistFound = dist;										//sett element til å være beste kandidat og sett hittil beste avstand til dist
					ShortestGoalPosition = correctTypeOfStaticObject.transform.position;
					tempGoalObject = correctTypeOfStaticObject;
				}
			}
		}
		if (tempGoalObject != null) {
			GoalObject = tempGoalObject;
			GoalPos = ShortestGoalPosition;
		}
	} 

	protected override void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.tag == "LumberCamp")
		{
			WoodCollected = 0;
			ListOfGameObjects.Remove(TreeImChopping);
			SetGoalObject<Tree>();
			var tempGoal = GoalObject as Tree;
			if(tempGoal!=null){
				TreeImChopping = tempGoal;
			}
			//ListOfGameObjects.
		}
	}
	
}
