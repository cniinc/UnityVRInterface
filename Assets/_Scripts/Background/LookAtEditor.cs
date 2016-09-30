using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;


[CustomEditor(typeof(LookAtObject))]
public class LookAtEditor : Editor {
	bool AutoOrientToCamera = false;
	public Camera ViewersCamera;

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

		AutoOrientToCamera = EditorGUILayout.Toggle ("Auto Orient to Camera", AutoOrientToCamera);

		if (AutoOrientToCamera)
			LookAtCamera ();

		LookAtObject LookAtScript = (LookAtObject)target;
		if(GUILayout.Button("Orient To Camera"))
		{
			LookAtCamera ();
		}
	}

	private void LookAtCamera()
	{
		LookAtObject LookAtScript = (LookAtObject)target;
		if (!ViewersCamera)
			ViewersCamera = Camera.main;

		LookAtScript.LookAt(ViewersCamera.gameObject);
	}
}
#endif
