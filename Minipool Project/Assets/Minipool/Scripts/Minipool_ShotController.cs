using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using NetworkManagement;
using BallPool.Mechanics;
using BallPool.AI;
using BallPool;

public class Minipool_ShotController : MonoBehaviour
{
    // important classes
    public PhysicsManager physicsManager;
    [SerializeField] private dans_GameManager gameManager;
    [SerializeField] private MinipoolGameUI uiController;

    // balls
    public Ball cueBall;
    public Ball cueBallp1;
    public Ball cueBallp2;

    [SerializeField] private LineRenderer cueBallSimpleLine;
    [SerializeField] private LineRenderer targetBallSimpleLine;
    public Vector3 targetBallSavedDirection { get; private set; }
    public BallListener targetBallListener { get; set; }

    public GameObject clickStartPlaceIndicator;
    public GameObject clickCurrentPlaceIndicator;
    public GameObject dragBallHitTargetArea;
    public LineRenderer interClickPlaceLine;
    private Vector3 mouseStartPosition;
    private Vector3 forceVector;

    private MouseState mouseState;
    private float force;
    public bool inShot;
    public bool inMove;
    public float cueBallMaxVelocity;

    // AI DEPENDENCIES
    public Transform firstMoveSpace;

    private float cueBallRadius;
    public int clothLayer { get; set; }
    public int boardLayer { get; set; }
    public int ballLayer { get; set; }
    public int cueBallLayer { get; set; }
    public int shotPlaneLayer { get; set; }

    public bool devTesting;

	private void Awake()
	{
        cueBallRadius = 0.5f * cueBall.transform.lossyScale.x;
        clothLayer = 1 << LayerMask.NameToLayer("Cloth");
        boardLayer = 1 << LayerMask.NameToLayer("Board");
        ballLayer = 1 << LayerMask.NameToLayer("Ball");
        cueBallLayer = 1 << LayerMask.NameToLayer("CueBall");
        shotPlaneLayer = 1 << LayerMask.NameToLayer("ShotPlane");

	}

	private void OnEnable()
	{
        InputOutput.OnMouseState += InputOutput_OnMouseState;
	}

	private void Update()
	{
        if (!devTesting)
            OnEnableControl(iHaveControl());
        else
            OnEnableControl(true);
	}

	private bool iHaveControl()
    {
        if (devTesting)
            return true;
        if (gameManager.turnID == MinipoolPlayerNetwork.Instance.mainPlayer.playerID)
        {
            return true;
        }
        else
        {
            return false;
        }
    }



	public void OnEnableControl(bool iHaveControl)
    {
        if (iHaveControl)
        {
            //Debug.Log("I have control");
        }
        else
        {
            //Debug.Log("My oppenent has control");
        }
    }

    void DisableAimers()
    {
        //clickStartPlaceIndicator.SetActive(false);
        //clickCurrentPlaceIndicator.SetActive(false);
        //interClickPlaceLine.enabled = false;
    }

    void EnableAimers()
    {
        clickStartPlaceIndicator.SetActive(true);
        clickCurrentPlaceIndicator.SetActive(true);
        interClickPlaceLine.enabled = true;
    }

    RaycastHit mousePosOnCloth()
    {
        RaycastHit clothHit;
        Vector3 origin = InputOutput.mouseWordPosition;
        Vector3 direction = InputOutput.mouseWordRay.direction;
        Physics.Raycast(origin, direction, out clothHit, 20.0f, shotPlaneLayer);
        return clothHit;
    }

    void TakeShot(Vector3 _shotPoint, Vector3 _forceVector)
    {
        physicsManager.moveTime = 0.0f;
        physicsManager.endFromNetwork = false;
        //shotNumber++;
        inShot = true;
        inMove = true;

        Impulse impulse = new Impulse(_shotPoint, -(_forceVector));

        physicsManager.SetImpulse(impulse);


        physicsManager.StartShot(cueBall.listener);


        inShot = false;

    }


    void ControlIn3D(MouseState mouseState)
    {
        if (mouseState == MouseState.Down)
        {
            Debug.Log("mouse is down");

            Vector3 mouseScreenPosition = InputOutput.mouseScreenPosition;

            //if (Vector3.Distance(mouseScreenPosition, cueBall.transform.position) < 3.0f)
            //{
                // set the start position of the mouse on the cloth
                clickStartPlaceIndicator.transform.position = mousePosOnCloth().point;
                InputOutput.rotationMode = false;
            //}
            //else
            //{
            //    InputOutput.rotationMode = true;
            //}

            if (InputOutput.rotationMode == true)
            {
                DisableAimers();
            }
            else
            {
                EnableAimers();
            }
        }
        else if (mouseState == MouseState.Up)
        {
            DisableAimers();

        }
        else if (mouseState == MouseState.Press || (mouseState == MouseState.Move))
        {

            //Debug.Log("dragging mouse");
            //// rotate the camera
            //if (InputOutput.rotationMode == true)
            //{
            //    //DisableAimers();

            //    Vector3 screenRotateSpeed = -(InputOutput.mouseScreenSpeed * Time.deltaTime);
            //    Vector3 screenRotateSpeedNormalized = screenRotateSpeed.normalized;

            //    float rSpeed = screenRotateSpeed.x;

            //    //cuePivot.Rotate(cuePivot.up, (InputOutput.isMobilePlatform ? rotationSpeed3D : mouseRotationSpeed3D) * 50.0f * rotationSpeedCurve.Evaluate(rSpeed / 50.0f));
            //}
            //else if (InputOutput.rotationMode == false)
            //{
                clickCurrentPlaceIndicator.transform.position = mousePosOnCloth().point;

                interClickPlaceLine.positionCount = 2;
                interClickPlaceLine.SetPosition(0, clickStartPlaceIndicator.transform.position);
                interClickPlaceLine.SetPosition(1, clickCurrentPlaceIndicator.transform.position);

                float dragDist = Vector3.Distance(clickCurrentPlaceIndicator.transform.position, clickStartPlaceIndicator.transform.position);

                forceVector = Vector3.Normalize(clickCurrentPlaceIndicator.transform.position - clickStartPlaceIndicator.transform.position);
                forceVector = new Vector3(forceVector.x, 0, forceVector.z);
                forceVector = forceVector * cueBallMaxVelocity * physicsManager.ballMass;
            //}
        }
    }

    void InputOutput_OnMouseState(MouseState _mouseState)
    {
        if(!iHaveControl())
        {
            return;
        }
        if (!InputOutput.inUsedCameraScreen && _mouseState != MouseState.Up)
        {
            return;
        }
        this.mouseState = _mouseState;

        if (!(mouseState == MouseState.Down || mouseState == MouseState.Up || mouseState == MouseState.Press || mouseState == MouseState.Move))
        {
            return;
        }

        if (!InputOutput.rotationMode && InputOutput.isMobilePlatform && mouseState == MouseState.Up)
        {
            // take the shot
            inShot = true;

            TakeShot(cueBall.transform.position, forceVector);
            Debug.Log("Shooting from " + cueBall.transform.position.ToString() + " with " + forceVector.ToString() + " force");

            DisableAimers();

            //StartCoroutine("WaitAndStartShot");
        }
        if ((!InputOutput.inUsedCameraScreen && mouseState != MouseState.Up))
        {
            return;
        }
        if (mouseState == MouseState.Down)
        {
            InputOutput.touchOrigin = Input.mousePosition;
        }
        if (!inShot)
        {
            ControlIn3D(mouseState);
        }
    }

}