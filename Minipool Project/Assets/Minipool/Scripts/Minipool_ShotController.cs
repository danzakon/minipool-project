using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using NetworkManagement;
using BallPool.Mechanics;
using BallPool.AI;
using BallPool;

public class Minipool_ShotController : MonoBehaviour
{

    public event System.Action OnEndCalculateAI;
    public event System.Action OnSelectBall;
    public event System.Action OnUnselectBall;

    // important classes
    public PhysicsManager physicsManager;
    [SerializeField] private dans_GameManager gameManager;
    [SerializeField] private AightBallPoolAIManager aiManager;
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

    private MouseState touchState;

    // AI DEPENDENCIES
    public Transform firstMoveSpace;

    private float cueBallRadius;
    public int clothLayer { get; set; }
    public int boardLayer { get; set; }
    public int ballLayer { get; set; }
    public int cueBallLayer { get; set; }
    public int shotPlaneLayer { get; set; }

	private void Awake()
	{
        cueBallRadius = 0.5f * cueBall.transform.lossyScale.x;
        clothLayer = 1 << LayerMask.NameToLayer("Cloth");
        boardLayer = 1 << LayerMask.NameToLayer("Board");
        ballLayer = 1 << LayerMask.NameToLayer("Ball");
        cueBallLayer = 1 << LayerMask.NameToLayer("CueBall");
        shotPlaneLayer = 1 << LayerMask.NameToLayer("ShotPlane");
	}

	public void OnEnableControl(bool value)
    {
        
    }

}