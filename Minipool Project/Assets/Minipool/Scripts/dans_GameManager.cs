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

    // private classes
    [SerializeField] private PhysicsManager physicsManager;
    [SerializeField] private TimeController timeController;
    [SerializeField] private MinipoolGameLogic gameLogic;
    [SerializeField] private MinipoolGameUI gameUIController;
    [SerializeField] private BallPoolAIManager aiManager;

    // instances
    private MinipoolGameManager minipool_GameManager;


    // objects
    public Ball[] balls;
    [SerializeField] private Text gameInfo;


    // info
    public int[] activeBallIds;
    private bool onBallHitPocket = false;
    private bool playAgainMenuIsActive = false;

    // globals
    public static int CUE_BALL_P1 = 0;
    public static int CUE_BALL_P2 = 1;
    public static int BLUE_BALL = 2;
    public static int RED_BALL = 3;
    public static int BLACK_BALL = 4;


	private void Awake()
	{
        // save data at start
        DataManager.SaveGameData();

        // TODO: fix
        // NetworkManager.network.OnNetwork += NetworkManager_network_OnNetwork;

        // initialize game manager instance
        minipool_GameManager = new MinipoolGameManager();
        minipool_GameManager.Initialize(physicsManager, aiManager, balls);
        minipool_GameManager.maxPlayTime = timeController.maxPlayTime;

        // initialize game logic instance
        gameLogic = new MinipoolGameLogic();

        // events -> functions
        minipool_GameManager.OnEndTime += OnEndTime;
        minipool_GameManager.OnSetGameInfo += OnSetGameInfo;
        gameUIController.OnForceGoHome += OnForceGoHome;

        // check if network is initialized
        if(!NetworkManager.initialized)
        {
            Debug.LogWarning("Network manager is not initialized, going home...");
            gameUIController.GoHome();
            enabled = false;
            return;
        }
	}

	private void Start()
	{
        // TODO
        // minipool_GameManager.OnSetPrize += (int prize) => { this.prize.text = prize + ""; };


        // events -> functions
        minipool_GameManager.OnCalculateAI += OnCalculateAI;
        minipool_GameManager.OnSetPlayer += OnSetPlayer;
        minipool_GameManager.OnSetAvatar += OnSetAvatar;
        minipool_GameManager.OnSetActivePlayer += OnSetActivePlayer;
        minipool_GameManager.OnGameComplete += OnGameComplete;
        minipool_GameManager.OnSetActiveBallsIds += OnSetActiveBallIds;
        minipool_GameManager.OnEnableControl += (bool value) =>
        {
            shotController.OnEnableControl(value);
        };

        // shot controller events -> functions
        shotController.OnEndCalculateAI += OnEndCalculateAI;
        shotController.OnSelectBall += OnSelectBall;
        shotController.OnUnselectBall += OnUnselectBall;

        minipool_GameManager.Start();

        // turn change -> function
        BallPoolPlayer.OnTurnChanged += Player_OnTurnChanged;

        if (!MinipoolGameLogic.isOnLine)
        {
            // user goes first (for testing)
            // TODO use random
            BallPoolPlayer.turnId = 1;
        }
        else if (MinipoolGameLogic.isOnLine)
        {
            NetworkManager.network.SendRemoteMessage("OnOpponenWaitingForYourTurn");
            NetworkManager.network.SendRemoteMessage("OnOppenenInGameScene");
            StartCoroutine(UpdateNetworkTime());

            // display whose turn it is in UI
            gameInfo.text = (BallPoolPlayer.mainPlayer.myTurn ? "Your turn" : "Your opponents turn") + "\nGood luck!";

            // 
            StartCoroutine(HideGameInfoText(gameInfo.text));
            Resources.UnloadUnusedAssets();
        }
 	}

	private void Update()
	{
        minipool_GameManager.Update(Time.deltaTime);
        timeController.UpdateTime(minipool_GameManager.playTime);
	}

    public void SetBallsState(int number)
    {
        
    }

    private int GetTimeInSeconds()
    {
        return 0;
    }

    void GameManager_TestFunction()
    {
        
    }

    void OnEndTime()
    {
        
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

    void OnCalculateAI ()
    {
        
    }

    // TODO: combine with AightBallPoolManager CallOnSetPlayer
    void OnSetPlayer(BallPoolPlayer player)
    {
        
    }

    void OnSetActiveBallIds(BallPoolPlayer player)
    {
        
    }

    void OnSetActivePlayer(BallPoolPlayer player, bool value)
    {
        
    }

    void OnGameComplete()
    {
        
    }

    void UpdateActiveBalls()
    {
        
    }

    void OnStartShot(string data)
    {
        
    }

    void OnEndShot(string data)
    {
        
    }

    void PhysicsManager_OnBallHitBall(BallListener ball, BallListener hitBall, bool inMove)
    {
        
    }

    void PhysicsManager_OnBallHitBoard(BallListener ball, bool inMove)
    {
        
    }

    void PhysicsManager_OnBallHitPocket(BallListener ball, PocketListener pocket, bool inMove)
    {
        
    }

    void Player_OnTurnChanged()
    {
        
    }

    void OnForceGoHome()
    {
        
    }

    void TriggerPlayAgainMenu(bool isOn)
    {
        // TODO: Implement
        Debug.LogWarning("TriggerPlayAgainMenu not implemented");
    }

    private IEnumerator DownloadAndSetAvatar(BallPoolPlayer player)
    {
        yield return null;
    }


    // TODO: combine with AightBallPoolManager CallOnSetAvatar
    void OnSetAvatar(BallPoolPlayer player)
    {
        StartCoroutine(DownloadAndSetAvatar(player));
    }

    void OnEndCalculateAI()
    {
        
    }

    void OnSelectBall()
    {

    }

    void OnUnselectBall()
    {

    }

    // CallOnSetPrize from AightBallPoolManager
    void OnSetPrize()
    {
        
    }

    IEnumerator UpdateNetworkTime()
    {
        yield return null;
    }

}
