using UnityEngine;
using System.Collections;


[ExecuteInEditMode]
public class RotatingPanel : MonoBehaviour {

	public Camera ViewersCamera;
	public float MeterDistanceFromCamera;
	public float TotalWrapAroundRangeInDegrees;
	public GameObject[] ObjectsInOrder;

	//public bool forceObjectsToLookAtCamera;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		//only runs if in editor, not playing. You can call the placeObjectsInOrder() function 
		//elsewhere when you add something new.
		if(!Application.isPlaying && ObjectsInOrder.Length>0)
			placeObjectsInOrder ();
		
	}

	void placeObjectsInOrder()
	{
		print ("running");
		//place the objects at MeterDistance from camera
		foreach (GameObject go in ObjectsInOrder) 
		{
			if (go != null) {
				go.transform.parent = this.transform;
				transform.localPosition = Vector3.zero;
			}
		}

		//place objects on a 180' spectrum
		for(int i =0; i< ObjectsInOrder.Length && ObjectsInOrder[i] != null; i++)
		{
			ObjectsInOrder[i].transform.position = Vector3.left * MeterDistanceFromCamera;
			ObjectsInOrder[i].transform.RotateAround(transform.position, Vector3.up, 180 * (i + 1) / (ObjectsInOrder.Length+1));



			//currently too buggy
			/*
			if(forceObjectsToLookAtCamera)
			ObjectsInOrder [i].transform.LookAt (ViewersCamera.transform);
			*/
		}
	}
}
