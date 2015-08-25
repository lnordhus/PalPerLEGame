using UnityEngine;
using System.Collections;

public class Tree : StaticObjects {

	Animator anim;

	private float TimePast;

	public bool IsBeingChoppet;
	public bool IsFalling;
	
	void Start () {

		anim = GetComponent <Animator> ();
		
	}

	void Update () {

		Animating ();
		
	}

	//Animerer trekutting og at tre faller.
	void Animating ()
	{
		
		// Tell the animator whether or not the player is walking.
		anim.SetBool ("Cutting", IsBeingChoppet);
		anim.SetBool ("FallingTree", IsFalling);
		
	}
}
