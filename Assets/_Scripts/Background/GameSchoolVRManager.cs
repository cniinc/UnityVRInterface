using UnityEngine;
using System.Collections;

public class GameSchoolVRManager : MonoBehaviour {
	private GameObject GSText;
	private GameObject GSButton;
	public static GameSchoolVRManager instance;


	// Use this for initialization
	void Awake () {
		instance = this;
		GSText = Resources.Load ("Text") as GameObject;
		GSButton = Resources.Load ("Button") as GameObject;
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp (KeyCode.F))
			InstantiateTextInFrontOfCamera ();
			
	}

	public void InstantiateTextInFrontOfCamera()
	{
		
		Vector3 cameraFront = Camera.main.transform.forward* 5;
		GameObject newText = Instantiate (GSText, cameraFront, Quaternion.identity) as GameObject;
		newText.gameObject.GetComponent<LookAtCamera> ().LookAt (Camera.main.gameObject);
	}

	public void InstantiateButtonInFrontOfCamera() //TODO - make one function that passes in the right GO.
	{

		Vector3 cameraFront = Camera.main.transform.forward* 5;
		GameObject newText = Instantiate (GSText, cameraFront, Quaternion.identity) as GameObject;
		newText.gameObject.GetComponent<LookAtCamera> ().LookAt (Camera.main.gameObject);
	}


}
