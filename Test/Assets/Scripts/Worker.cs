using UnityEngine;
using System.Collections;

public class Worker : PlayerController {

	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}

	private float TimePast = 0;
	private int WoodCollected = 0;
	private bool IsCuttingWood;
	private bool IsWoodCutter;
	// Update is called once per frame
	void Update () {
		if (dist <= 10 && IsWoodCutter && WoodCollected != 1){
			Debug.Log ("Chopping tree");
			IsCuttingWood = true;
			TimePast += Time.deltaTime;

			if (TimePast >= 5){
				WoodCollected = 1;
				IsCuttingWood = false;
				Debug.Log ("Tree chopped");
			}
		}
		if ( Input.GetMouseButtonDown (1) && MouseHelper.GetMouseTag () == "Tree" ){
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
}
