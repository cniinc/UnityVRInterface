using UnityEngine;
using System.Collections;


//[ExecuteInEditMode]
public class RotatingPanel : MonoBehaviour {

	public Camera ViewersCamera;
	public float MeterDistanceFromCamera;
	public float TotalWrapAroundRangeInDegrees;
	public GameObject[] ObjectsInOrder;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyUp (KeyCode.F))
			placeObjectsInOrder ();
	
	}

	void placeObjectsInOrder()
	{
		//place the objects at MeterDistance from camera
		foreach (GameObject go in ObjectsInOrder) 
		{
			go.transform.parent = this.transform;
			transform.localPosition = Vector3.zero;

		}

		//place objects on a 180' spectrum
		for(int i =0; i< ObjectsInOrder.Length; i++)
		{
			transform.localPosition = Vector3.left * MeterDistanceFromCamera;
			ObjectsInOrder[i].transform.RotateAround(Vector3.zero, Vector3.up, 180 * (i + 1) / (ObjectsInOrder.Length+1));

		}
	}
}
