using UnityEngine;
using System.Collections;

public class UserInput : MonoBehaviour {

	public struct BoxLimit
	{
		public float LeftLimit;
		public float TopLimit;
		public float BottomLimit;
		public float RightLimit;
	}

	public static BoxLimit cameraLimits = new BoxLimit();
	
	public Terrain WorldTerrain;

	//Camera variables
	private float ScrollSpeed = 250;
	private float RotateSpeed = 100;
	private float RotateAmount = 10;
	private int ScrollWidth = 15;
	private float MaxCameraHeight = 180;
	private float MinCameraHeight;

	public float worldTerrainPadding = 25f;

	
	void Start () {

		cameraLimits.LeftLimit = WorldTerrain.transform.position.x + worldTerrainPadding;
		cameraLimits.RightLimit = WorldTerrain.terrainData.size.x - worldTerrainPadding;
		cameraLimits.TopLimit = WorldTerrain.terrainData.size.z - worldTerrainPadding;
		cameraLimits.BottomLimit = WorldTerrain.transform.position.z + worldTerrainPadding;
	}

	void Update () {

			Vector3 desiredPosition = MoveCamera ();

			if (isDesiredPositionOverBoundaries(desiredPosition))
			{
				MoveCamera();
			}

			RotateCamera();
			MinCameraHeight = WorldTerrain.SampleHeight(transform.position) + 10f;
	}

	//Moves camera
	private Vector3 MoveCamera()
	{
		float xpos = Input.mousePosition.x;
		float ypos = Input.mousePosition.y;
		Vector3 movement = new Vector3(0,0,0);

		//Horizontal camera movement
		if((xpos >= 0 && xpos < ScrollWidth) || Input.GetKey (KeyCode.A)) {
			movement.x -= ScrollSpeed;
		} else if((xpos <= Screen.width && xpos > Screen.width - ScrollWidth) || Input.GetKey (KeyCode.D)) {
			movement.x += ScrollSpeed;
		}
		
		//Vertical camera movement
		if((ypos >= 0 && ypos < ScrollWidth) || Input.GetKey (KeyCode.S)) {
			movement.z -= ScrollSpeed;
		} else if((ypos <= Screen.height && ypos > Screen.height - ScrollWidth) || Input.GetKey (KeyCode.W)) {
			movement.z += ScrollSpeed;
		}

		//Camera only moves direction camera is facing
		movement = Camera.main.transform.TransformDirection(movement);
		movement.y = 0;

		movement.y -= ScrollSpeed * Input.GetAxis("Mouse ScrollWheel") * 3;

		Vector3 origin = Camera.main.transform.position;
		Vector3 destination = origin;
		destination.x += movement.x;
		destination.y += movement.y;
		destination.z += movement.z;

		if(destination.y > MaxCameraHeight) {
			destination.y = MaxCameraHeight;
		} else if(destination.y < MinCameraHeight) {
			destination.y = MinCameraHeight;
		}

		//Moves camera
		if(destination != origin) {
			Camera.main.transform.position = Vector3.MoveTowards(origin, destination, Time.deltaTime * ScrollSpeed);
		}

		return destination;
	}

	//Rotates camera
	private void RotateCamera()
	{
		Vector3 originAngle = Camera.main.transform.eulerAngles;
		Vector3 destinationAngle = originAngle;
		
		//Detect rotation amount if ALT is being held and the Right mouse button is down
		if((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetMouseButton(0)) {
			destinationAngle.x -= Input.GetAxis("Mouse Y") * RotateAmount;
			destinationAngle.y += Input.GetAxis("Mouse X") * RotateAmount;
		}
		
		//If a change in position is detected perform the necessary update
		if(destinationAngle != originAngle) {
			Camera.main.transform.eulerAngles = Vector3.MoveTowards(originAngle, destinationAngle, Time.deltaTime * RotateSpeed);
		}
	}

	public bool isDesiredPositionOverBoundaries(Vector3 destination)
	{
		bool overBoundaries = false;

		Vector3 desiredWorldPosition = this.transform.TransformPoint (destination);

		//Check boundaries
		if (desiredWorldPosition.x < cameraLimits.LeftLimit)
			overBoundaries = true;
		
		if (desiredWorldPosition.x > cameraLimits.RightLimit)
			overBoundaries = true;
		
		if (desiredWorldPosition.z > cameraLimits.TopLimit)
			overBoundaries = true;
		
		if (desiredWorldPosition.z < cameraLimits.BottomLimit)
			overBoundaries = true;

		return overBoundaries;
	}
}
