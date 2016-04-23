using UnityEngine;
using System.Collections;
using System;
using VRStandardAssets.Utils;
using UnityEngine.UI;

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
//	[Tooltip("If pressed, stays with the 'pressed' sprite")]
//	[SerializeField] private bool ToggleStyle;

	private VRCameraFade m_CameraFade;                 // This fades the scene out when a new scene is about to be loaded.
	private SelectionRadial m_SelectionRadial;         // This controls when the selection is complete.
	private VRInteractiveItem m_InteractiveItem;       // The interactive item for where the user should click to load the level.
	private Image m_ButtonImage;
	private BoxCollider m_ButtonCollider;

	private bool m_GazeOver;                                            // Whether the user is looking at the VRInteractiveItem currently.


	void Awake()
	{
		m_CameraFade = Camera.main.gameObject.GetComponent<VRCameraFade> () as VRCameraFade;
		m_SelectionRadial = Camera.main.gameObject.GetComponent<SelectionRadial> () as SelectionRadial;
		m_InteractiveItem = GetComponent<VRInteractiveItem> () as VRInteractiveItem;
		m_ButtonImage = GetComponent<Image> () as Image;
		m_ButtonCollider = GetComponent<BoxCollider> () as BoxCollider;
	}

	private void OnEnable ()
	{
		m_InteractiveItem.OnOver += HandleOver;
		m_InteractiveItem.OnOut += HandleOut;
		m_SelectionRadial.OnSelectionComplete += HandleSelectionComplete;

		CurrentSprite = NormalSprite;
		m_ButtonImage.sprite = CurrentSprite;
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
		m_ButtonImage.sprite = CurrentSprite;
		print (CurrentSprite.bounds);
	}


	private void HandleOut()
	{
		// When the user looks away from the rendering of the scene, hide the radial.
		m_SelectionRadial.Hide();

		m_GazeOver = false;

		CurrentSprite = NormalSprite;
		m_ButtonImage.sprite = CurrentSprite;
	}


	private void HandleSelectionComplete()
	{
		// If the user is looking at the rendering of the scene when the radial's selection finishes, activate the button.
//		if(m_GazeOver)
//			StartCoroutine (ActivateButton());

		//how do I make this run the OnClick event? 

		CurrentSprite = PressedSprite;
		m_ButtonImage.sprite = CurrentSprite;
	}

	private void updateSprite()
	{
		m_ButtonImage.sprite = CurrentSprite;
		//m_ButtonCollider.size = new Vector3 (CurrentSprite.bounds.x, CurrentSprite.bounds.y, 1);
	}


	private IEnumerator FadeToNewScene(string SceneToLoad)
	{

		yield break;
//		// If the camera is already fading, ignore.
//		if (m_CameraFade.IsFading)
//			yield break;
//
//		// If anything is subscribed to the OnButtonSelected event, call it.
//		if (OnButtonSelected != null)
//			OnButtonSelected(this);
//
//		// Wait for the camera to fade out.
//		yield return StartCoroutine(m_CameraFade.BeginFadeOut(true));
//
//		// Load the level.
//		SceneManager.LoadScene(m_SceneToLoad, LoadSceneMode.Single);
	}
}
