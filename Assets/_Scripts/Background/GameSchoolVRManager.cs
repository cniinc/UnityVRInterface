using UnityEngine;
using System.Collections;

public class GameSchoolVRManager : MonoBehaviour {
	private GameObject GSText;
//	private GameObject GSButton;


	// Use this for initialization
	void Awake () {
		GSText = Resources.Load ("Text") as GameObject;

	
	}
	
	// Update is called once per frame
	void Update () {
			
	}

	public void InstantiateTextInFrontOfCamera()
	{
		Instantiate (GSText, new Vector3 (0, 0, 5), Quaternion.identity);
	}
}
