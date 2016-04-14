using UnityEngine;
using System;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// This scripts allows interactions with objects in the scene based on a gaze.
/// This class casts a ray into the scene and if it finds a VRIneractive item it 
/// calls the events for the item to use.
/// need help? contact us: babilinapps.com
/// </summary>


public class VREventSystem : MonoBehaviour
    {
     
        public event Action<RaycastHit> OnRaycasthit;                   // This event is called every frame that the user's gaze is over a collider.

       
    [Tooltip("Show Crosshair.")]
    [SerializeField]
        private bool showCursor = true;
    [Tooltip("Layers excluded from the raycast.")]
    [SerializeField]
        private LayerMask _exclusionLayers;           
    [Tooltip("Use the ‘Auto Click Timer’ to press the item.")]
    [SerializeField]
        public bool autoClick;
    [Tooltip("Adds ‘VR Interactive Item’ script to all UI objects.")]
    [SerializeField]
         private bool AutoAddVRInteract = true;
    [Tooltip("How far into the scene the ray is cast.")]
    [SerializeField]
        private float _rayLength = 500f;
    [Tooltip("Time that has to pass to click event.")]
    public  float autoClickTime = 1;
    [Tooltip("show crosshair at all times.")]
    public bool classicCursor = true;

	public KeyCode InteractionKey;



        private PointerEventData eventSystem;                           // Is used to send simple events
        private Gaze_VRInteractiveItem _CurrentInteractible;                //The current interactive item
	private Gaze_VRInteractiveItem _LastInteractible;                   //The last interactive item
        private bool clicking;                            // calls action if time has passed or button is pressed.
        private GameObject cursor;                        // The Cursor that shows in the middle of the screen.
        private Image _imageFill;
        private float lookTime;                            //current amout of time that the user is looking at the object.
        private Transform _camera;                         // Stores the camera transform.

        public static Ray gazeRay;                        // stores the ray that hits UI objects.
        public delegate void Hover();
        public static event Hover OnHover;
        public delegate void Deselect();
        public static event Deselect OnDeselect;

        public static VREventSystem curEventSystem;
        
    // TODO Utility for other classes to get the current interactive item
	public Gaze_VRInteractiveItem CurrentInteractible
        {
            get { return _CurrentInteractible; }
        }


        void Awake()
        {
       

            if (curEventSystem == null)
            {
                curEventSystem = this;
                DontDestroyOnLoad(gameObject);

            }else
            {
                this.enabled = false;
            }

        SetUp();
    }

    void SetUp() {
        if (!GameObject.FindGameObjectWithTag("Crosshair"))
        {
            GameObject _Crosshair =Instantiate( Resources.Load("Crosshair_Canvas")) as GameObject;
            _Crosshair.transform.SetParent(Camera.main.transform);
            _Crosshair.transform.localPosition = Vector3.forward;
        }

        cursor = GameObject.FindGameObjectWithTag("Crosshair");
        _imageFill = GameObject.FindGameObjectWithTag("Crosshair_Fill").GetComponent<Image>();
        if(AutoAddVRInteract)
        FindGameObjectsWithLayer(LayerMask.NameToLayer("UI"));

    }

        void Start()
        {
            //Gets the main Camera
            _camera = Camera.main.transform;
            //Gets active event system
            eventSystem = new PointerEventData(EventSystem.current);




     

            UnityEngine.VR.InputTracking.Recenter(); // recenters the VR input
        }

        private void Update()
        {
        if (!autoClick) {
            Cursor.lockState = CursorLockMode.Locked;
			if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(InteractionKey))
            {
                HandleClick();
            }
			// TODO: listen for rotate input
			// notifiy current interactigle
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
        EyeRaycast();
        _imageFill.fillAmount = lookTime / autoClickTime;

        if (showCursor != true)
        {
            if (cursor != null)
                cursor.SetActive(false);

         
        }
       
        }
        private void EyeRaycast()
        {


            // Create a ray that points forwards from the camera.
            gazeRay = new Ray(_camera.position, _camera.forward);
            RaycastHit hit;

            // Do the raycast forweards to see if we hit an interactive item
            if (Physics.Raycast(gazeRay, out hit, _rayLength, ~_exclusionLayers))
            {
				Gaze_VRInteractiveItem interactible = hit.collider.GetComponent<Gaze_VRInteractiveItem>(); //attempt to get the VRInteractiveItem on the hit object
                _CurrentInteractible = interactible;
             
                //Sets the mouse positon at the colided point
                eventSystem.position = hit.point;

                // If we hit an interactive item and it's not the same as the last interactive item, then call Over
                if (interactible && interactible != _LastInteractible)
                {
                
                    DeactiveLastInteractible();
                }

            //We see if we are hovering over an item and then call  the action
            if (interactible && interactible == _LastInteractible)
            {
                HandleHover();
                cursor.SetActive(true);
            }
            else if (classicCursor){

                cursor.SetActive(false);
            }

                //We update the last interactible item
                _LastInteractible = interactible;


                if (OnRaycasthit != null)
                    OnRaycasthit(hit);
            }
            else
            {
                // Nothing was hit, deactive the last interactive item.
                DeactiveLastInteractible();
               if (cursor && !classicCursor)
                    cursor.SetActive(false);
            }
        }


        //Desctivates last 
        private void DeactiveLastInteractible()
        {
            lookTime = 0;
            if (_LastInteractible == null)
                return;

            ExecuteEvents.Execute(_LastInteractible.gameObject, eventSystem, ExecuteEvents.deselectHandler);
          
            _LastInteractible = null;
            if (OnDeselect != null)
            {
                OnDeselect();
            }
        }

        // Deselects all 
        private void HandleDeselect()
        {
            lookTime = 0;
            if (_LastInteractible == null)
                return;
            ExecuteEvents.Execute(_CurrentInteractible.gameObject, eventSystem, ExecuteEvents.deselectHandler);
            _LastInteractible = _CurrentInteractible;
            _CurrentInteractible = null;
            if(OnDeselect != null)
            {
                OnDeselect();
            }
        }

        //Hover Action
        private void HandleHover()
        {
            if (_CurrentInteractible == null || clicking == true)
                return;

            if (autoClick) // Does the AutoClick
                HandleAutoClick();

            if (OnHover != null) // Alows other scripts to subscribe to the  hover event.
                OnHover();

          ExecuteEvents.Execute(_CurrentInteractible.gameObject, eventSystem, ExecuteEvents.selectHandler);


        }

        //Key Up
        private void HandleUp()
        {
            if (_CurrentInteractible != null)
            {
               
                ExecuteEvents.Execute(_CurrentInteractible.gameObject, eventSystem, ExecuteEvents.pointerUpHandler);
            }
        }

        //Key Down
        private void HandleDown()
        {
            if (_CurrentInteractible != null)
            {
              
              ExecuteEvents.Execute(_CurrentInteractible.gameObject, eventSystem, ExecuteEvents.pointerDownHandler);
            }
        }

        //Click action ( button down)
        private void HandleClick()
        {
      
            if (_CurrentInteractible != null)
            {
               
                if (clicking == false)
                {
                    StartCoroutine(preformClick());
                    clicking = true;
                }
            }

        }

        //Makes sure that the press is rendered
        IEnumerator preformClick()
        {
     
         yield return new WaitForEndOfFrame();
          ExecuteEvents.Execute(_CurrentInteractible.gameObject, eventSystem, ExecuteEvents.deselectHandler);
          yield return new WaitForEndOfFrame();
        ExecuteEvents.Execute(_CurrentInteractible.gameObject, eventSystem, ExecuteEvents.pointerDownHandler);
          yield return new WaitForEndOfFrame();
        ExecuteEvents.Execute(_CurrentInteractible.gameObject, eventSystem, ExecuteEvents.pointerUpHandler);
        yield return new WaitForEndOfFrame();
        ExecuteEvents.Execute(_CurrentInteractible.gameObject, eventSystem, ExecuteEvents.pointerClickHandler);
            clicking = false;
            HandleDeselect();

        }

        //Preforms the look to click action
        private void HandleAutoClick()
        {

            if (lookTime > autoClickTime)
            {
                HandleClick();
                lookTime = 0;

            }
            else
            {
                lookTime = lookTime + Time.deltaTime;
            }
        }


    //This can be called in other scripts
    public float CursorDistance()
        {
            if (_LastInteractible != null)
                return Vector3.Distance(_LastInteractible.transform.position, _camera.transform.position);
            else
                return 1;
        }

        //Gets the active VREventSystem usefull for public bools
        public static VREventSystem GetCurrent()
        {
            return curEventSystem;
        }


        public void FindGameObjectsWithLayer (int layer) {
        GameObject[] goArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        for (int i = 0; i < goArray.Length; i++)
		{ if ((goArray[i].layer == layer && !goArray[i].GetComponent<Gaze_VRInteractiveItem>()) && 
                (goArray[i].GetComponent<Toggle>() || goArray[i].GetComponent<Button>() || goArray[i].GetComponent<Slider>()
                || goArray[i].GetComponent<Scrollbar>() || goArray[i].GetComponent<EventTrigger>() || goArray[i].GetComponent<Dropdown>()))
            {
				goArray[i].AddComponent<Gaze_VRInteractiveItem>();
            }
        }
 
    }




        void DisableEvents()
        {
           curEventSystem.enabled = false;
        }
    }


    

