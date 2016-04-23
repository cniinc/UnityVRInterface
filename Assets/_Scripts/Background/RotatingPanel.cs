using UnityEngine;
using System.Collections;


[ExecuteInEditMode]
public class RotatingPanel : MonoBehaviour {

	public Camera ViewersCamera;
	public float MeterDistanceFromCamera;
	public float HeightAdjustFromViewerEyeLevel;
	//public float TotalRotationDegrees;
	public float RotationRate;
	public GameObject[] ObjectsInOrder;
	private bool forceObjectsToLookAtCamera = true;


	// Use this for initialization
	void Awake () {
		FindCamera ();
		transform.position = ViewersCamera.transform.position;
	}

	void FindCamera()
	{
		ViewersCamera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {

		//only runs if in editor, not playing. You can call the placeObjectsInOrder() function 
		//elsewhere when you add something new.
		if(!Application.isPlaying && ObjectsInOrder.Length>0)
			placeObjectsInOrder ();

		if (Input.GetKey (KeyCode.RightArrow))
			rotateRight ();
		if (Input.GetKey (KeyCode.LeftArrow))
			rotateLeft ();
	}

	void placeObjectsInOrder()
	{
		
		//place the objects at MeterDistance from camera
		foreach (GameObject go in ObjectsInOrder) 
		{
			if (go != null) {
				
				go.transform.SetParent (this.transform);
				go.transform.localPosition = Vector3.zero;
			}
		}

		//place objects on a 180' spectrum
		for(int i =0; i< ObjectsInOrder.Length && ObjectsInOrder[i] != null; i++)
		{
			//set standard Left and adjust height
			Vector3 tempPosition = Vector3.left * MeterDistanceFromCamera;
			tempPosition.y = ViewersCamera.transform.position.y + HeightAdjustFromViewerEyeLevel;

			ObjectsInOrder [i].transform.position = tempPosition;


			ObjectsInOrder[i].transform.RotateAround(transform.position, Vector3.up, 180 * (i + 1) / (ObjectsInOrder.Length+1));



			//Look at the camera
			if (forceObjectsToLookAtCamera) {
				ObjectsInOrder [i].transform.LookAt (2*ObjectsInOrder[i].transform.position - ViewersCamera.transform.position); //to orient the right way. Just "lookat(viewersCamera.transform) makes it a mirror image.
			}
		}
	}


	private void rotateRight()
	{
		print ("Rotating Right");
		transform.Rotate (Vector3.up * Time.deltaTime * RotationRate);
	}

	private void rotateLeft()
	{
		print ("Rotate Left");
		transform.Rotate (-1*Vector3.up * Time.deltaTime*RotationRate);
	}
}
