using UnityEngine;
using System.Collections;

public class MyCamera : MonoBehaviour {

	public Transform myHead;
	public Transform ThirdPersonCenter;
	public Camera main_camera;
	public Camera mini_map_camera;
	
	public bool isFirstPerson = false;
	public bool isTrackingMouse = true;
	
	public float RotateSpeed = 180.0f;
	public float ZoomSpeed = 10f;
	public int Vertical_Max = 75;
	public int Vertical_Min = -75;
	private float rotationX;
	
	public float Camera_Distance_Length = 30.0f;
	private Vector3 mini_map_camera_distance = new Vector3( 0 , 100 , 0 );
	private float Camera_Distance = 30.0f;
	private bool isGameInit = true;
	
	void Start () {
//for later use
		//if(networkView.isMine != true)
		//	enabled = false;
		
		if(null==myHead)
			myHead = transform.FindChild("Head");
		if(null==ThirdPersonCenter)
			ThirdPersonCenter = transform.FindChild("ThirdPersonCenter");
		if(null==main_camera)
			main_camera = Camera.main;
		if(null==mini_map_camera)
			mini_map_camera = GameObject.FindGameObjectWithTag("MiniMapCamera").camera;
		
		myHead.parent = ThirdPersonCenter;
	}
	
	void Update () {
		if(isFirstPerson)
		{
			FirstPersonCamera();
		}else
		{
			ThirdPersonCamera();
		}
		
		if(null!=mini_map_camera)
		{
			mini_map_camera.transform.position = transform.position + mini_map_camera_distance;
			mini_map_camera.transform.LookAt( transform , Vector3.forward ) ;
		}
		
		if(Input.GetKeyDown(KeyCode.KeypadPlus))
		{
			mini_map_camera.orthographicSize=Mathf.Clamp(mini_map_camera.orthographicSize+3, 20, 60);
		}
		
		if(Input.GetKeyDown(KeyCode.KeypadMinus))
		{
			mini_map_camera.orthographicSize=Mathf.Clamp(mini_map_camera.orthographicSize-3, 20, 60);
		}
	}//Update End
	
	void FirstPersonCamera()
	{
		//camera position to head
		main_camera.transform.position = myHead.position;
		main_camera.transform.rotation = myHead.rotation;
		
		if(Input.GetAxis("Mouse ScrollWheel") != 0)
		{
			if(Input.GetAxis("Mouse ScrollWheel")<0)
			{
				if(isFirstPerson)
					isFirstPerson = false;
			}
			
		}
		
		if(Camera_Distance>0)
		{
			Camera_Distance = Mathf.Lerp(Camera_Distance,0,Time.deltaTime*1);
			if(Camera_Distance<0.01)
				Camera_Distance=0;
		}
		
		if(isTrackingMouse)
		{
			rotationX += Input.GetAxis("Mouse Y")*Time.deltaTime*RotateSpeed;
			rotationX = Mathf.Clamp(rotationX, Vertical_Min, Vertical_Max);
			myHead.localEulerAngles = new Vector3(-rotationX,0,0);
			
			float rollY = Input.GetAxis("Mouse X")*Time.deltaTime*RotateSpeed;
			transform.Rotate(new Vector3(0, rollY, 0),Space.Self);
		}
		else
		{
			if(Input.GetKey(KeyCode.Mouse1))
			{
				rotationX += Input.GetAxis("Mouse Y")*Time.deltaTime*RotateSpeed;
				rotationX = Mathf.Clamp(rotationX, Vertical_Min, Vertical_Max);
				myHead.localEulerAngles = new Vector3(-rotationX,0,0);
			
				float rollY = Input.GetAxis("Mouse X")*Time.deltaTime*RotateSpeed;
				transform.Rotate(new Vector3(0, rollY, 0),Space.Self);
			}
		}
	}//FirstPersonCamera End
	
	void ThirdPersonCamera()
	{
		if(null==ThirdPersonCenter)
		{
			Debug.Log("There is no ThirdPersonCenter under player object");
			return;
		}
		
		Vector3 newCameraPos = myHead.TransformPoint( new Vector3(0, 0, -Camera_Distance) );
		//main_camera.transform.position = newCameraPos;
		main_camera.transform.position = Vector3.Lerp(main_camera.transform.position, newCameraPos, Time.deltaTime*ZoomSpeed);
		main_camera.transform.LookAt( myHead.position , Vector3.up ) ;	
		
		if(Input.GetAxis("Mouse ScrollWheel") != 0)
		{
			if(Input.GetAxis("Mouse ScrollWheel")>0)
			{
				if(Camera_Distance < 5)
				{
					isFirstPerson=true;
				}
				else
					Camera_Distance = Mathf.Clamp(Camera_Distance - Input.GetAxis("Mouse ScrollWheel")*ZoomSpeed, 0, Camera_Distance_Length);
			}else
			{
				Camera_Distance = Mathf.Clamp(Camera_Distance - Input.GetAxis("Mouse ScrollWheel")*ZoomSpeed, 0, Camera_Distance_Length);
			}
		}
		
		if(Input.GetKey(KeyCode.Mouse1))
		{
			rotationX += Input.GetAxis("Mouse Y")*Time.deltaTime*RotateSpeed;
			rotationX = Mathf.Clamp(rotationX, Vertical_Min, Vertical_Max);
			myHead.localEulerAngles = new Vector3(-rotationX,0,0);
			
			float rollY = Input.GetAxis("Mouse X")*Time.deltaTime*RotateSpeed;
			transform.Rotate(new Vector3(0, rollY, 0),Space.Self);
		}
		else if(Input.GetKey(KeyCode.Mouse2))
		{		
			rotationX += Input.GetAxis("Mouse Y")*Time.deltaTime*RotateSpeed;
			rotationX = Mathf.Clamp(rotationX, Vertical_Min, Vertical_Max);
			myHead.localEulerAngles = new Vector3(-rotationX,0,0);
			
			float rollY = Input.GetAxis("Mouse X")*Time.deltaTime*RotateSpeed;
			ThirdPersonCenter.Rotate(new Vector3(0, rollY, 0),Space.Self);
			//transform.Rotate(new Vector3(0, rollY, 0),Space.Self);
		}
		
	}//ThirdPersonCamera End
}
