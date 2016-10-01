using UnityEngine;
using System.Collections;
using UnityEngine.VR;

public class CameraUtilities : MonoBehaviour {
	bool firstUpdateFrame = true;
	/*
	 * a series of simple functions to streamline functions.
	 */

	// Use this for initialization
	void Update () {

		if (firstUpdateFrame) {
			firstUpdateFrame = false;
			if (VRDevice.isPresent) {
				GetComponent<SimpleSmoothMouseLook> ().enabled = false;
				Debug.Log ("Turning off SimpleSmoothMouseLook because " + VRDevice.model + " detected");
			} else {
				
				GetComponent<GvrHead> ().trackRotation = false;
				GetComponentInChildren<GvrViewer> ().VRModeEnabled = false;
			}


		}
	
	}

}
