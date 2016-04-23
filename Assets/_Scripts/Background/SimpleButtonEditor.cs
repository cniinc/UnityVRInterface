using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(SimpleButton))]
public class SimpleButtonEditor : Editor {
	private Camera ViewersCamera;

	// Use this for initialization
	void Start () {
		ViewersCamera = Camera.main;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector ();


		SimpleButton SimpleButtonScript = (SimpleButton)target;
		if(GUILayout.Button("Orient To Camera"))
		{
			if (!ViewersCamera)
				ViewersCamera = Camera.main;

			SimpleButtonScript.LookAt(ViewersCamera.gameObject);
		}
	}
}
