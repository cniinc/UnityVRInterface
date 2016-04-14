using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneSelect : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void LoadSceneCalled(string SceneName)
	{
		SceneManager.LoadScene (SceneName);
	}
}
