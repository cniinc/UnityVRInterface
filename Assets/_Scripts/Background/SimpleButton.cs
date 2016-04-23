using UnityEngine;
using System.Collections;
using System;
using VRStandardAssets.Utils;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SimpleButton : MonoBehaviour
{
	/* 
	 * A modified version of MenuButton from VR Assets. Takes 3 sprites (normal, hover, pressed)  
	 * and operates like the UI button. A temporary solution until Gaze works with Unity's UI system natively. 
	 * 
	*/


	public event Action<SimpleButton> OnButtonSelected;                   // This event is triggered when the selection of the button has finished.

	private Sprite CurrentSprite;
	[SerializeField] private Sprite NormalSprite;
	[SerializeField] private Sprite HoverSprite;
	[SerializeField] private Sprite PressedSprite;

	[Header("When Button is Selected...")]
	public bool FadeOutToNewScene;
	public string m_SceneToLoad;

	[Header("Options")]
	[Tooltip("If pressed, stays with the 'pressed' sprite")]
	[SerializeField] private bool ToggleStyle;

	private VRCameraFade m_CameraFade;                 // This fades the scene out when a new scene is about to be loaded.
	private SelectionRadial m_SelectionRadial;         // This controls when the selection is complete.
	private VRInteractiveItem m_InteractiveItem;       // The interactive item for where the user should click to load the level.
	private Image m_ButtonImage;
	private BoxCollider m_ButtonCollider;
	private Canvas m_canvas;

	private bool m_GazeOver;                                            // Whether the user is looking at the VRInteractiveItem currently.


	void Awake()
	{
		m_CameraFade = Camera.main.gameObject.GetComponent<VRCameraFade> () as VRCameraFade;
		m_SelectionRadial = Camera.main.gameObject.GetComponent<SelectionRadial> () as SelectionRadial;
		m_InteractiveItem = GetComponent<VRInteractiveItem> () as VRInteractiveItem;
		m_ButtonImage = GetComponent<Image> () as Image;
		m_ButtonCollider = GetComponent<BoxCollider> () as BoxCollider;
		m_canvas = m_ButtonImage.canvas;
	}

	private void OnEnable ()
	{
		m_InteractiveItem.OnOver += HandleOver;
		m_InteractiveItem.OnOut += HandleOut;
		m_SelectionRadial.OnSelectionComplete += HandleSelectionComplete;

		CurrentSprite = NormalSprite;
		m_ButtonImage.sprite = CurrentSprite;
		updateSprite ();
	}


	private void OnDisable ()
	{
		m_InteractiveItem.OnOver -= HandleOver;
		m_InteractiveItem.OnOut -= HandleOut;
		m_SelectionRadial.OnSelectionComplete -= HandleSelectionComplete;
	}


	private void HandleOver()
	{
		// When the user looks at the rendering of the scene, show the radial.
		m_SelectionRadial.Show();

		m_GazeOver = true;

		CurrentSprite = HoverSprite;
		//m_ButtonImage.sprite = CurrentSprite;
		updateSprite();
		//print (CurrentSprite.textureRect);
	}


	private void HandleOut()
	{
		// When the user looks away from the rendering of the scene, hide the radial.
		m_SelectionRadial.Hide();

		m_GazeOver = false;

		CurrentSprite = NormalSprite;
		//m_ButtonImage.sprite = CurrentSprite;
		updateSprite ();
	}


	private void HandleSelectionComplete()
	{
		// If the user is looking at the rendering of the scene when the radial's selection finishes, activate the button.
		if(m_GazeOver && FadeOutToNewScene)
			StartCoroutine (FadeToNewScene());

		//how do I make this run the OnClick event? 
		if (ToggleStyle)
			CurrentSprite = PressedSprite;
		else
			StartCoroutine (ShowSpriteForSeconds (PressedSprite, .3f));
		m_ButtonImage.sprite = CurrentSprite;
		updateSprite ();
	}

	private void updateSprite()
	{
		m_ButtonImage.sprite = CurrentSprite;
		m_ButtonCollider.size = new Vector3 (CurrentSprite.textureRect.width, CurrentSprite.textureRect.height, 1);

		//resize canvas as well
//		Rect r = new Rect (m_canvas.pixelRect.x, m_canvas.pixelRect.y, CurrentSprite.textureRect.width, CurrentSprite.textureRect.height);
//		m_canvas.pixelRect = r;

		RectTransform rt = m_canvas.transform as RectTransform;
		rt.sizeDelta = new Vector2(CurrentSprite.textureRect.width, CurrentSprite.textureRect.height);

	}

	private IEnumerator ShowSpriteForSeconds(Sprite sp, float seconds)
	{
		CurrentSprite = sp;
		updateSprite ();
		yield return new WaitForSeconds (seconds);
		CurrentSprite = NormalSprite;
		updateSprite ();

	}


	private IEnumerator FadeToNewScene()
	{
		// If the camera is already fading, ignore.
		if (m_CameraFade.IsFading)
			yield break;

		// If anything is subscribed to the OnButtonSelected event, call it.
		if (OnButtonSelected != null)
			OnButtonSelected(this);

		// Wait for the camera to fade out.
		yield return StartCoroutine(m_CameraFade.BeginFadeOut(true));

		// Load the level.
		SceneManager.LoadScene(m_SceneToLoad, LoadSceneMode.Single);
	}

	public void LookAt(GameObject go)
	{
		if (!m_canvas) 
		{
			m_canvas = GetComponent<Image> ().canvas;
		}

		m_canvas.transform.LookAt(2*transform.position - go.transform.position);

	}
}
