using UnityEngine;
using System.Collections;

public class Tree : StaticObjects {
	public static int TreCounter = 0;
	public int myID;
	// Use this for initialization
	void Start () {
		TreCounter++;
		myID = TreCounter;
	}
	
	// Update is called once per frame
	void Update () {

	}
}
