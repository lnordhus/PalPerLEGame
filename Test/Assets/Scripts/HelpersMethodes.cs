using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

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

	public static UnityEngine.Object[] InitiateAllGameObjects(){
		return UnityEngine.Object.FindObjectsOfType (typeof(LumberCamp));
	}



	public static List<Waypoint> CreatePath(Vector3 goal,Vector3 start){ 
		var path = new List<Waypoint>();
		var mapWidt = 100;
		var mapHeight = 100;
		var map = new List<Cell>();
		for( var x = -mapWidt/2; x<mapWidt/2; x++){
			for(var z = -mapHeight/2; z<mapHeight/2; z++){
				var pos = start;
				pos.x += x;
				pos.z += z; 
				
				map.Add(new Cell{
					IsAvailable = true,
					distToCell = 0,
					pos = pos,
					distToGoal = (goal-pos).sqrMagnitude
				});
			}
		}
		var CellsToExplore = new  List<Cell>();
		var CellsExplored = new  List<Cell>();
		var startCell = map.Find (x => x.pos.x == start.x && x.pos.z == start.z); 
		addNeighbours (CellsToExplore,map,startCell);
		CellsExplored.Add (startCell); 
		for (var x = 0; x<1000; x++) {
			var minDist = float.MaxValue;
			Cell closestCell = null;
			foreach(var element in CellsToExplore)
			{
				var elementDist = element.distToGoal+element.distToCell;
				if(minDist > elementDist)
				{
					closestCell = element;
					minDist = elementDist;
				}
			}
			if(closestCell != null){
				if(closestCell.pos.x == goal.x && closestCell.pos.z == goal.z){
					return path;
			}
			path.Add(new Waypoint{
				pos = closestCell.pos,
				hasVisited = false,
			});
				addNeighbours (CellsToExplore,map,closestCell);
			}
		}
		return path;
	}

	private static void addNeighbours(List<Cell> cellsToExplore,List<Cell> map, Cell start){
		var neightbours = map.FindAll (x=> (x.pos-start.pos).sqrMagnitude <2);
		cellsToExplore.AddRange (neightbours); 
	}

//	public static Vector3 GetClosestObject(){
//		LumberCamp[] GameObjects = Object.FindObjectsOfType(typeof(LumberCamp));
//	}

	public static TElement FirsOrDefault<TElement>(List<TElement> myList,Predicate<TElement> predicate ) {
		foreach (var element in myList) {
			if(predicate(element)){
				return element;
			}
		}
		return default(TElement);
	}


}

public class Cell{
	public bool IsAvailable;
	public float distToCell;
	public float distToGoal;
	public Vector3 pos;
}

public class Waypoint{
	public Vector3 pos;
	public bool hasVisited;
}


