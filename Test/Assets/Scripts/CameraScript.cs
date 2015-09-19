using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour{

    //Camera limits 
    public struct BoxLimit
    {
        public float LeftLimit;
        public float TopLimit;
        public float BottomLimit;
        public float RightLimit;
    }

    public static BoxLimit cameraLimits = new BoxLimit();

    public Transform target;
    public Vector3 targetOffset;
    public float distance = 5.0f;
    public float maxDistance = 60f;
    public float minDistance;
    public float xSpeed = 200.0f;
    public float ySpeed = 200.0f;
    public int zoomRate = 80;
    public float panSpeed = 0.3f;
    public float zoomDampening = 5.0f;
    private float ScrollSpeed = 6;
    private float RotateAmount = 10;
    private float RotateSpeed = 200;
    private int ScrollWidth = 15;
    private float MaxCameraHeight = 1000;
    private float MinCameraHeight;
    private float MaxVerticalRotation = 65f;
    private float MinVerticalRotation = 0f;
    [HideInInspector]
    public float CameraHeight;

    public Terrain WorldTerrain;
    public float WorldTerrainPadding = 0f;

    private float xDeg = 0.0f;
    private float yDeg = 0.0f;
    private float currentDistance;
    private float desiredDistance;
    private Quaternion currentRotation;
    private Quaternion desiredRotation;
    private Quaternion rotation;
    private Vector3 position;

    void Start()
    {
        Init();

        cameraLimits.LeftLimit = WorldTerrain.transform.position.x + WorldTerrainPadding;
        cameraLimits.RightLimit = WorldTerrain.terrainData.size.x - WorldTerrainPadding;
        cameraLimits.TopLimit = WorldTerrain.terrainData.size.z - WorldTerrainPadding;
        cameraLimits.BottomLimit = WorldTerrain.transform.position.z + WorldTerrainPadding;
    }

    void OnEnable()
    {
        Init();
    }

    void LateUpdate()
    {
        CameraMovement();
    }

    public void Init()
    {
        //If there is no target, create a temporary target at 'distance' from the cameras current viewpoint 
        if (!target)
        {
            GameObject go = new GameObject("Cam Target");
            go.transform.position = transform.position + (transform.forward * distance);
            target = go.transform;
        }

        distance = Vector3.Distance(transform.position, target.position);
        currentDistance = distance;
        desiredDistance = distance;

        //be sure to grab the current rotations as starting points. 
        position = transform.position;
        rotation = transform.rotation;
        currentRotation = transform.rotation;
        desiredRotation = transform.rotation;

        xDeg = Vector3.Angle(Vector3.right, transform.right);
        yDeg = Vector3.Angle(Vector3.up, transform.up);
    }

    public void CameraMovement()
    {
        float xpos = Input.mousePosition.x;
        float ypos = Input.mousePosition.y;

        Vector3 movement = new Vector3(0, 0, 0);

        // If left mouse and left alt are selected? ORBIT 
        if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftAlt))
        {
            xDeg += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            yDeg -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

            ////////OrbitAngle 

            //Clamp the vertical axis for the orbit 
            yDeg = Mathf.Clamp(yDeg, 0, 65);

            // set camera rotation  
            desiredRotation = Quaternion.Euler(yDeg, xDeg, 0);
            currentRotation = transform.rotation;

            rotation = Quaternion.Lerp(currentRotation, desiredRotation, Time.deltaTime * zoomDampening);

            transform.rotation = rotation;

            float lockPos = 0;
            target.rotation = Quaternion.Euler(lockPos, this.transform.rotation.eulerAngles.y, lockPos);
        }
        // otherwise if middle mouse is selected, we pan by way of transforming the target in screenspace 
        /*else if (Input.GetMouseButton(2)) 
        { 
        //grab the rotation of the camera so we can move in a psuedo local XY space 
        target.rotation = transform.rotation; 
        target.Translate(Vector3.right * -Input.GetAxis("Mouse X") * panSpeed); 
        target.Translate(transform.up * -Input.GetAxis("Mouse Y") * panSpeed, Space.World); 
        }*/

        //Horizontal camera movement 
        if ((xpos >= 0 && xpos < ScrollWidth) || Input.GetKey(KeyCode.A))
        {
            movement.x -= ScrollSpeed;
        }
        else if ((xpos <= Screen.width && xpos > Screen.width - ScrollWidth) || Input.GetKey(KeyCode.D))
        {
            movement.x += ScrollSpeed;
        }

        //Vertical camera movement 
        if ((ypos >= 0 && ypos < ScrollWidth) || Input.GetKey(KeyCode.S))
        {
            movement.z -= ScrollSpeed;
        }
        else if ((ypos <= Screen.height && ypos > Screen.height - ScrollWidth) || Input.GetKey(KeyCode.W))
        {
            movement.z += ScrollSpeed;
        }

        movement.y = 0;

        target.Translate(movement * Time.deltaTime * ScrollSpeed, Space.Self);

        ////////Orbit Position 

        minDistance = WorldTerrain.SampleHeight(transform.position) + 30.0f;

        // affect the desired Zoom distance if we roll the scrollwheel 
        desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomRate * Mathf.Abs(desiredDistance);
        //clamp the zoom min/max 
        desiredDistance = Mathf.Clamp(desiredDistance, minDistance, maxDistance);
        // For smoothing of the zoom, lerp distance 
        currentDistance = Mathf.Lerp(currentDistance, desiredDistance, Time.deltaTime * zoomDampening);

        // calculate position based on the new currentDistance  
        position = target.position - (rotation * Vector3.forward * currentDistance + targetOffset);
        transform.position = position;
    }

    /*private static float ClampAngle(float angle, float min, float max) 
    { 
    if (angle < -360) 
    angle += 360; 
    if (angle > 360) 
    angle -= 360; 
    return Mathf.Clamp(angle, min, max); 
    }*/
}
