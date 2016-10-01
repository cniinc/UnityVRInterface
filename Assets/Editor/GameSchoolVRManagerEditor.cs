using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CustomEditor(typeof(GameSchoolVRManager))]
public class GameSchoolVRManagerEditor : Editor {

	// Use this for initialization
	void Start () {


	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
		
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector ();

		GameSchoolVRManager GSMgrScript = (GameSchoolVRManager)target;

		if (GUILayout.Button ("Create Text Box"))
			GSMgrScript.InstantiateTextInFrontOfCamera ();

		if (GUILayout.Button ("Create Button"))
			GSMgrScript.InstantiateButtonInFrontOfCamera ();

	}

}
