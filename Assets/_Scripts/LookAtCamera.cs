using UnityEngine;
using System.Collections;

public class LookAtCamera : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

				
	
	}



	public void LookAt(GameObject go)
	{
		transform.LookAt(2*transform.position - go.transform.position);

	}


}
