using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    //Camera limits
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
	private float RotateAmount = 10;
    private float RotateSpeed = 200;
	private int ScrollWidth = 15;
	private float MaxCameraHeight = 180;
	private float MinCameraHeight;
    private float MaxVerticalRotation = 65f;
    private float MinVerticalRotation = 0f;

	public float WorldTerrainPadding = 0f;

	
	void Start () {

		cameraLimits.LeftLimit = WorldTerrain.transform.position.x + WorldTerrainPadding;
		cameraLimits.RightLimit = WorldTerrain.terrainData.size.x - WorldTerrainPadding;
		cameraLimits.TopLimit = WorldTerrain.terrainData.size.z - WorldTerrainPadding;
		cameraLimits.BottomLimit = WorldTerrain.transform.position.z + WorldTerrainPadding;
	}

	void Update () {

			Vector3 desiredPosition = CameraMovementVector ();
            MinCameraHeight = WorldTerrain.SampleHeight(transform.position) + 35f;

			if (!isDesiredPositionOverBoundaries(desiredPosition))
			{
				MoveCamera();
			}

			RotateCamera();
	}


	//Creates vector for camera movement
	public Vector3 CameraMovementVector()
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

		return destination;
	}
    
    //Moves camera
    public void MoveCamera()
    {
        Vector3 origin = Camera.main.transform.position;
        Vector3 cameraDestination = origin;
        cameraDestination = CameraMovementVector();

        if (cameraDestination != origin)
        {
            Camera.main.transform.position = Vector3.MoveTowards(origin, cameraDestination, Time.deltaTime * ScrollSpeed);
        }
    }

	//Rotates camera
	public void RotateCamera()
	{
	    Vector3 originAngle = Camera.main.transform.eulerAngles;
		Vector3 destinationAngle = originAngle;
		
		//Detect rotation amount if ALT is being held and the Right mouse button is down
		if((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetMouseButton(0)) {
			destinationAngle.x -= Input.GetAxis("Mouse Y") * RotateAmount;
			destinationAngle.y += Input.GetAxis("Mouse X") * RotateAmount;
		}
		
		//If a change in position is detected perform the necessary update. Cannot look to the sky.
		if(destinationAngle != originAngle && destinationAngle.x > MinVerticalRotation && destinationAngle.x < MaxVerticalRotation) {
			Camera.main.transform.eulerAngles = Vector3.MoveTowards(originAngle, destinationAngle, Time.deltaTime * RotateSpeed);
		}
	}

    //Checks if camera destination is within boundaries
	public bool isDesiredPositionOverBoundaries(Vector3 destination)
	{
		bool overBoundaries = false;

		Vector3 desiredWorldPosition = CameraMovementVector();

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
