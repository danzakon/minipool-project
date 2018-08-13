using UnityEngine;
using System.Collections;

public enum MouseState
{
    Down = 0,
    Stay,
    Move,
    Press,
    PressAndStay,
    PressAndMove,
    Up,
	Pinch
}

public class InputOutput : MonoBehaviour
{
    private static Vector3 _mouseScreenSpeed;
    private static Vector3 _mouseViewportPoint;
    private static Vector3 _mouseWordSpeed;
    private static Vector3 _mouseScreenPosition;
    private static Vector3 _mouseScreenSymmetricalPosition;
    private static Vector3 _mouseScreenRelativePosition;
    private static Vector3 _mouseScreenRelativeSymmetricalPosition;
    private static Vector3 _mouseWordPosition;
    private static Vector3 _view;
    private static Ray _mouseWordRay;

	public static float xrotationDir;

	public float perspectiveZoomSpeed = 0.5f;        // The rate of change of the field of view in perspective mode.
	public float orthoZoomSpeed = 0.5f;        // The rate of change of the orthographic size in orthographic mode.

    [SerializeField]
    private bool _isMobilePlatform;
    public static bool isMobilePlatform
    {
        get;
        private set;
    }
	public static bool rotationMode
	{
		get;
		set;
	}

    [SerializeField] private Camera _usedCamera;
    public static Camera usedCamera
    {
        get;
        set;
    }

    public static Vector3 mouseScreenPosition
    {
        get{ return _mouseScreenPosition; }
    }

    public static Vector3 mouseViewportPoint
    {
        get{ return _mouseViewportPoint; }
    }
    public static Vector3 mouseViewportSymmetricalPoint
    {
        get{ return new Vector3(_mouseViewportPoint.x - 0.5f, _mouseViewportPoint.y - 0.5f, 0.0f); }
    }

    public static Vector3 mouseScreenSymmetricalPosition
    {
        get{ return _mouseScreenSymmetricalPosition; }
    }

    public static Vector3 mouseScreenRelativePosition
    {
        get{ return _mouseScreenRelativePosition; }
    }

    public static Vector3 mouseScreenRelativeSymmetricalPosition
    {
        get{ return _mouseScreenRelativeSymmetricalPosition; }
    }

    public static Vector3 mouseWordPosition
    {
        get{ return _mouseWordPosition; }
    }

    public static Vector3 mouseScreenSpeed
    {
        get{ return _mouseScreenSpeed; }
    }

    public static Vector3 mouseWordSpeed
    {
        get{ return _mouseWordSpeed; }
    }

    public static Vector3 view
    {
        get{ return _view; }
    }

    public static Ray mouseWordRay
    {
        get{ return _mouseWordRay; }
    }

    public static Vector3 WorldToScreenPoint ( Vector3 position)
    {
        return usedCamera.WorldToScreenPoint(position);
    }
    public static float WorldToScreenRadius(float radius, Transform sphere)
    {
        Vector3 centerPointInScreen = InputOutput.WorldToScreenPoint(sphere.position);
        Vector3 pointInScreen = InputOutput.WorldToScreenPoint(sphere.position + radius * usedCamera.transform.right);
        return Vector3.Distance(centerPointInScreen, pointInScreen);
    }

    public delegate void OnMause(MouseState mouseState);

    public static event OnMause OnMouseState;

	public static Vector3 touchOrigin;

    public static bool inUsedCameraScreen
    {
        get 
        {
            Vector3 inputMousePosition = Input.mousePosition;
            Vector3 viewportPoint = usedCamera.ScreenToViewportPoint(inputMousePosition);
            Rect rect = usedCamera.rect;
            return viewportPoint.x >= 0.0f && viewportPoint.x <= 1.0f && viewportPoint.y >= 0.0f && viewportPoint.y <= 1.0f;
        }
    }

    void Awake ()
    {
        if (_usedCamera)
        {
            usedCamera = _usedCamera;
        }
        if (Application.isEditor)
        {
            isMobilePlatform = _isMobilePlatform;
        }
        else
        {
            isMobilePlatform = Application.isMobilePlatform;
        }
    }

    void Update()
    {
		if (Input.GetKeyDown ("r")) {
			if (rotationMode != true) {
				rotationMode = true;
				Debug.Log ("R: ON");
			} else 
			{
				rotationMode = false;
				Debug.Log ("R: OFF");
			}
		}

		if (PinchZoom ()) {
			return;
		}

		//if (rotationMode) {
		//	RotateCamera ();
		//} else {
		//	xrotationDir = 0;
		//}
			
        if (Input.GetMouseButtonDown(0))
        {
            if (!inUsedCameraScreen)
            {
                //return;
            }
            Vector3 inputMousePosition = Input.mousePosition;
            Vector3 viewportPoint = usedCamera.ScreenToViewportPoint(inputMousePosition);
           
            _mouseScreenSymmetricalPosition = new Vector3(inputMousePosition.x - 0.5f * Screen.width, inputMousePosition.y - 0.5f * Screen.height, 0.0f);
            _mouseScreenPosition = inputMousePosition;
            Rect rect = usedCamera.rect;
            _mouseScreenRelativePosition = new Vector3(inputMousePosition.x - Screen.width * rect.x, inputMousePosition.y - Screen.height * rect.y, 0.0f);
            _mouseScreenRelativeSymmetricalPosition = new Vector3(inputMousePosition.x - Screen.width * (rect.x + 0.5f * rect.width), inputMousePosition.y - Screen.height * (rect.y + 0.5f * rect.height), 0.0f);
            _mouseViewportPoint = viewportPoint;
            _mouseWordPosition = usedCamera.ScreenToWorldPoint(_mouseScreenPosition);
            _mouseWordRay = usedCamera.ScreenPointToRay(_mouseScreenPosition);
            _mouseScreenSpeed = Vector3.zero;
            _mouseWordSpeed = Vector3.zero;

            if (OnMouseState != null)
            {
                OnMouseState(MouseState.Down);
            }
        }

        bool isPressed = Input.GetMouseButton(0);
        if (!isMobilePlatform || isPressed)
        {
            if (!inUsedCameraScreen)
            {
                //return;
            }
            Vector3 inputMousePosition = Input.mousePosition;
            Vector3 viewportPoint = usedCamera.ScreenToViewportPoint(inputMousePosition);

            Vector3 inputWordPosition = usedCamera.ScreenToWorldPoint(inputMousePosition);

            _mouseScreenSpeed = (inputMousePosition - _mouseScreenPosition) / Time.deltaTime;
            _mouseScreenPosition = inputMousePosition;
            _mouseViewportPoint = viewportPoint;
            _mouseScreenSymmetricalPosition = new Vector3(inputMousePosition.x - 0.5f * Screen.width, inputMousePosition.y - 0.5f * Screen.height, 0.0f);
            Rect rect = usedCamera.rect;
            _mouseScreenRelativePosition = new Vector3(inputMousePosition.x - Screen.width * rect.x, inputMousePosition.y - Screen.height * rect.y, 0.0f);
            _mouseScreenRelativeSymmetricalPosition = new Vector3(inputMousePosition.x - Screen.width * (rect.x + 0.5f * rect.width), inputMousePosition.y - Screen.height * (rect.y + 0.5f * rect.height), 0.0f);
            _mouseWordSpeed = (inputWordPosition - _mouseWordPosition) / Time.deltaTime;
            _mouseWordPosition = usedCamera.ScreenToWorldPoint(_mouseScreenPosition);

            _mouseWordRay = usedCamera.ScreenPointToRay(_mouseScreenPosition);

           
            if (OnMouseState != null)
            {
                if (_mouseScreenSpeed.magnitude == 0.0f)
                {
                    if (isPressed)
                    {
                        OnMouseState(MouseState.Press);
                        OnMouseState(MouseState.PressAndStay);
                    }
                    else
                    {
                        OnMouseState(MouseState.Stay);
                    }
                }
                else
                {
                    if (isPressed)
                    {
                        OnMouseState(MouseState.Press);
                        OnMouseState(MouseState.PressAndMove);
                    }
                    else
                    {
                        OnMouseState(MouseState.Move);
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            Vector3 inputMousePosition = Input.mousePosition;
            _mouseScreenSpeed = Vector3.zero;
            _mouseWordSpeed = Vector3.zero;
            _mouseScreenPosition = inputMousePosition;
            _mouseScreenSymmetricalPosition = new Vector3(inputMousePosition.x - 0.5f * Screen.width, inputMousePosition.y - 0.5f * Screen.height, 0.0f);
            Rect rect = usedCamera.rect;
            _mouseScreenRelativePosition = new Vector3(inputMousePosition.x - Screen.width * rect.x, inputMousePosition.y - Screen.height * rect.y, 0.0f);
            _mouseScreenRelativeSymmetricalPosition = new Vector3(inputMousePosition.x - Screen.width * (rect.x + 0.5f * rect.width), inputMousePosition.y - Screen.height * (rect.y + 0.5f * rect.height), 0.0f);
            Vector3 viewportPoint = usedCamera.ScreenToViewportPoint(inputMousePosition);
            _mouseViewportPoint = viewportPoint;
            _view = Vector3.zero;
            _mouseWordRay = new Ray();
            if(OnMouseState != null)
            {
                OnMouseState(MouseState.Up);
            }
        }
        else
        {
            _view = usedCamera.transform.forward;
        }
    }

	bool PinchZoom(){
		// If there are two touches on the device...
		if (Input.touchCount == 2) {
			// Store both touches.
			Touch touchZero = Input.GetTouch (0);
			Touch touchOne = Input.GetTouch (1);

			// Find the position in the previous frame of each touch.
			Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
			Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

			// Find the magnitude of the vector (the distance) between the touches in each frame.
			float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
			float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

			// Find the difference in the distances between each frame.
			float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

			// Otherwise change the field of view based on the change in distance between the touches.
			usedCamera.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;

			// Clamp the field of view to make sure it's between 0 and 180.
			usedCamera.fieldOfView = Mathf.Clamp (usedCamera.fieldOfView, 0.1f, 179.9f);

			return true;
		} 
		else 
		{
			return false;
		}
	}

	void RotateCamera(){
		// Store the touch.
		Touch touchZero = Input.GetTouch (0);

		// Find the position in the previous frame of each touch.
		float touchZeroPrevPosX = touchZero.position.x - touchZero.deltaPosition.x;

		xrotationDir = touchZeroPrevPosX - touchZero.position.x;

	}
}
