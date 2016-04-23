using UnityEngine;
using System.Collections;
using UnityEngine.VR;

public class CameraUtilities : MonoBehaviour {

	/*
	 * a series of simple functions to streamline functions.
	 */

	// Use this for initialization
	void Start () {
		if (VRDevice.isPresent) 
		{
			GetComponent<SimpleSmoothMouseLook> ().enabled = false;
			Debug.Log ("Turning off SimpleSmoothMouseLook because " + VRDevice.model + " detected");
		}
	
	}

}
