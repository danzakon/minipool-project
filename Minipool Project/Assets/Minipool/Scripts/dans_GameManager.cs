using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BallPool;
using BallPool.Mechanics;
using BallPool.AI;
using NetworkManagement;


public class dans_GameManager : MonoBehaviour {
    // public classes
    public Minipool_ShotController shotController;
    public MinipoolGameLogic gameLogic;

    // private classes
    [Header("Managers")]
    [SerializeField] private PhysicsManager physicsManager;
    [SerializeField] private TimeController timeController;
    [SerializeField] private MinipoolGameUI gameUIController;
    [SerializeField] private BallPoolAIManager aiManager;
    private MinipoolGameManager minipoolGameManager;

    // objects
    [Header("Balls Info")]
    public Ball[] balls;
    public int[] activeBallIds;

    [Header("UI Objects")]
    [SerializeField] private Text gameInfo;
    [SerializeField] private Text mainPlayerNameText;
    [SerializeField] private Text mainPlayerCoinsText;
    [SerializeField] private Text otherPlayerNameText;
    [SerializeField] private Text otherPlayerCoinsText;


    // info
    private bool onBallHitPocket = false;
    private bool playAgainMenuIsActive = false;

    // globals
    public static int CUE_BALL_P1 = 0;
    public static int CUE_BALL_P2 = 1;
    public static int BLUE_BALL = 2;
    public static int RED_BALL = 3;
    public static int BLACK_BALL = 4;

    private PhotonView photonView;

    public int turnID;

	private void Awake()
	{
        SetPlayerInfoUI();

        photonView = this.gameObject.AddComponent<PhotonView>();
        photonView.ObservedComponents = new List<Component>(0);
        photonView.ObservedComponents.Add(this);
        photonView.synchronization = ViewSynchronization.ReliableDeltaCompressed;
        photonView.viewID = 1;
        photonView = gameObject.GetComponent<PhotonView>();

        gameInfo.text = "";

        StartCoroutine(SetFirstTurn());

        minipoolGameManager = new MinipoolGameManager();
        minipoolGameManager.Initialize(physicsManager, aiManager, balls);
        minipoolGameManager.gameLogic = new MinipoolGameLogic();
    }

    void Start()
    {
        minipoolGameManager.Start();
    }

    private void SwitchTurn()
    {
        if (turnID == MinipoolPlayerNetwork.Instance.mainPlayer.playerID)
        {
            photonView.RPC("RPC_SetTurn", PhotonTargets.All, MinipoolPlayerNetwork.Instance.otherPlayer.playerID);
        }
        else
        {
            photonView.RPC("RPC_SetTurn", PhotonTargets.All, MinipoolPlayerNetwork.Instance.mainPlayer.playerID);
        }
    }

    private IEnumerator SetFirstTurn()
    {
        yield return new WaitForSeconds(1.0f);
        if (PhotonNetwork.isMasterClient)
        {
            while (turnID != MinipoolPlayerNetwork.Instance.mainPlayer.playerID)
            {
                photonView.RPC("RPC_SetTurn", PhotonTargets.All, MinipoolPlayerNetwork.Instance.mainPlayer.playerID);
            }
        }

    }

    private void Update()
    {
        TurnTimer(Time.deltaTime);
        minipoolGameManager.Update(Time.deltaTime);
        timeController.UpdateTime(minipoolGameManager.playTime);
    }

    public float turnTime = 10.0f;
    private void TurnTimer(float deltaTime)
    {
        turnTime -= deltaTime;
        if (turnTime < 0.0f)
        {
            SwitchTurn();
            turnTime = 10.0f;
        }
    }

    [PunRPC]
    void RPC_SetTurn(int _turnID)
    {
        turnID = _turnID;

        if(shotController.cueBall == shotController.cueBallp1)
        {
            shotController.cueBall = shotController.cueBallp2;
        }
        else
        {
            shotController.cueBall = shotController.cueBallp1;
        }

        gameInfo.text = (turnID == MinipoolPlayerNetwork.Instance.mainPlayer.playerID ? "Your turn" : "Your opponents turn") + "\nGood luck!";
        StartCoroutine(HideGameInfoText(gameInfo.text));
        Debug.Log("turn id:" + turnID);
    }


    private void SetPlayerInfoUI()
    {
        mainPlayerNameText.text = MinipoolPlayerNetwork.Instance.mainPlayer.name;
        mainPlayerCoinsText.text = MinipoolPlayerNetwork.Instance.mainPlayer.coins.ToString();
        otherPlayerNameText.text = MinipoolPlayerNetwork.Instance.otherPlayer.name;
        otherPlayerCoinsText.text = MinipoolPlayerNetwork.Instance.otherPlayer.coins.ToString();
    }




    private bool displayGameInfo = false;
    void OnSetGameInfo(string info)
    {
        StartCoroutine(HideGameInfoText(info));
    }
    IEnumerator HideGameInfoText(string info)
    {
        while(displayGameInfo)
        {
            yield return null;
        }
        displayGameInfo = true;
        gameInfo.text = info;
        yield return new WaitForSeconds(3.0f);
        gameInfo.text = "";
        displayGameInfo = false;
    }













    void PhysicsManager_OnBallHitBall(BallListener ball, BallListener hitBall, bool inMove)
    {
        Debug.Log("PhysicsManager_OnBallHitBall");
        if (!inMove)
        {
            return;
        }
        if (shotController.targetBallListener == hitBall)
        {
            hitBall.body.velocity = hitBall.body.velocity.magnitude * shotController.targetBallSavedDirection;
            shotController.targetBallListener = null;
        }
        balls[ball.id].OnState(BallState.HitBall);
        bool isCueBall = MinipoolGameLogic.isCueBall(ball.id);

        if (isCueBall)
        {
            gameLogic.OnCueBallHitBall(ball.id, hitBall.id);
        }
    }

    void PhysicsManager_OnBallHitBoard(BallListener ball, bool inMove)
    {
        Debug.Log("PhysicsManager_OnBallHitBoard");
        if (!inMove)
        {
            return;
        }
        balls[ball.id].OnState(BallState.HitBoard);
        gameLogic.OnBallHitBoard(ball.id);
    }

    void PhysicsManager_OnBallHitPocket(BallListener ball, PocketListener pocket, bool inMove)
    {
        Debug.Log("PhysicsManager_OnBallHitPocket");
        if (!inMove)
        {
            return;
        }
        balls[ball.id].OnState(BallState.EnterInPocket);
        bool cueBallInPocket = false;
        gameLogic.OnBallInPocket(ball.id, ref cueBallInPocket);
        if (!cueBallInPocket)
        {
            onBallHitPocket = true;
        }
    }

}
