using UnityEngine;
using System.Collections;

public class MyCamera : MonoBehaviour {

	public Transform myHead;
	public Camera main_camera;
	
	void Start () {
		if(null==myHead)
			myHead = transform.FindChild("Head");
		
		if(null==main_camera)
			main_camera = Camera.main;
	}
	
	public float RotateSpeed = 180.0f;
	public float ZoomSpeed = 5f;
	public int Horizontal_Max = 360;
	public int Horizontal_Min = 0;
	public int Vertical_Max = 100;
	public int Vertical_Min = -100;
	public float Camera_Distance_Length = 10f;
	private float Camera_Distance = 10f;
	
	void Update () {
		//camera position to head
		main_camera.transform.position = myHead.position;
		main_camera.transform.rotation = myHead.rotation;
		
		if(Input.GetKey(KeyCode.Mouse1))
		{
			float mX = Input.GetAxis("Mouse X")*Time.deltaTime*RotateSpeed;
			float mY = Input.GetAxis("Mouse Y")*Time.deltaTime*RotateSpeed;
			myHead.Rotate(new Vector3(-mY, 0, 0),Space.Self);
			transform.Rotate(new Vector3(0, mX, 0),Space.Self);
		}

		if(Input.GetAxis("Mouse ScrollWheel") != 0)
		{
			Camera_Distance = Mathf.Clamp(Camera_Distance - Input.GetAxis("Mouse ScrollWheel")*ZoomSpeed, 0, Camera_Distance_Length);
		}
		
	}//Update End
}
