#if PUN
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetworkManagement;

public class PunNetwork : NetworkEngine, AightBallPoolMessenger
{
    private PhotonView photonView;
    private string gameVersion = "0.01";
    private PhotonPlayer opponentPlayer;
    private AightBallPoolNetworkMessenger messenger;

    public override void Initialize()
    {
        if (!messenger)
        {
            messenger = gameObject.AddComponent<AightBallPoolNetworkMessenger>();
        }
    }

    protected override void Awake()
    {
  //      base.Awake();
        
  //      sendRate = 10;
  //      PhotonNetwork.sendRate = sendRate;
  //      PhotonNetwork.sendRateOnSerialize = sendRate;
  //      PhotonNetwork.autoJoinLobby = true;
  //      PhotonNetwork.EnableLobbyStatistics = true;
  //      photonView = gameObject.AddComponent<PhotonView>();
  //      photonView.ObservedComponents = new List<Component>(0);
  //      photonView.ObservedComponents.Add(this);
  //      //photonView.synchronization = ViewSynchronization.ReliableDeltaCompressed;
  //      //photonView.viewID = 1;
  //      PhotonNetwork.automaticallySyncScene = true;
  //      PhotonNetwork.BackgroundTimeout = 5.0f;
		//Debug.Log("Connect ");
        //Connect();
    }

    public override void Disable()
    {
        base.Disable();
    }
    protected override void Update()
    {
        base.Update();
    }

    public override void SendRemoteMessage(string message, params object[] args)
    {
        photonView.RPC(message, opponentPlayer, args);
    }
 
    public override void OnGoToPlayWithPlayer(NetworkManagement.PlayerProfile player)
    {
        if (PhotonNetwork.player != PhotonNetwork.masterClient)
        {
//            adapter.homeMenuManager.UpdatePrize(player.prize);

//            Debug.LogWarning("SetPrize " + player.prize);
            NetworkManager.opponentPlayer = player;
            PhotonNetwork.JoinRoom(NetworkManager.PlayerToString(NetworkManager.opponentPlayer));
        }
        else
        {
            //The client who first created the room
            photonView.RPC("OnOpponenReadToPlay", opponentPlayer, NetworkManager.PlayerToString(NetworkManager.mainPlayer), AightBallPoolNetworkGameAdapter.is3DGraphics);
        }
    }


   
    public void OnJoinedRoom()
    {
        if (PhotonNetwork.player != PhotonNetwork.masterClient)
        {
            opponentPlayer = PhotonNetwork.masterClient;
            photonView.RPC("OnOpponenReadToPlay", opponentPlayer, NetworkManager.PlayerToString(NetworkManager.mainPlayer), AightBallPoolNetworkGameAdapter.is3DGraphics);
        }
        CallNetworkState(NetworkState.JoinedToRoom);
    }

    void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        Debug.Log("OnPhotonPlayerConnected " + newPlayer.ID);
        opponentPlayer = newPlayer;
    }

    void OnConnectedToPhoton()
    {
        CallNetworkState(NetworkState.Connected);
        Debug.Log("state " + state);
    }
        
    void OnMasterClientSwitched (   PhotonPlayer    newMasterClient )
    {
        Debug.LogWarning("OnMasterClientSwitched");
    }

    void OnFailedToConnectToPhoton(object parameters)
    {
        CallNetworkState(NetworkState.FailedToConnect);
        Debug.LogWarning(state);
    }

    void OnDisconnectedFromPhoton()
    {
        CallNetworkState(NetworkState.LostConnection);
        Debug.LogWarning(state);
    }
        
    public override void CreateRoom()
    {
        if (PhotonNetwork.connectionStateDetailed == ClientState.Joined)
        {
            PhotonNetwork.LeaveRoom();
        }
        if (PhotonNetwork.connectedAndReady && PhotonNetwork.connectionStateDetailed == ClientState.JoinedLobby)
        {
            PhotonNetwork.CreateRoom(NetworkManager.PlayerToString(NetworkManager.mainPlayer)/* + PhotonNetwork.GetRoomList().Length*/, new RoomOptions() { MaxPlayers = 2 }, null);
        }
    }

    public override void Reset()
    {
        LeftRoom();
    }

    public override void LeftRoom()
    {
        if (PhotonNetwork.connectionStateDetailed == ClientState.Joined)
        {
            PhotonNetwork.LeaveRoom();
        }
    }

    public override void Connect()
    {
        if (photonView && reachable)
        {
            Debug.Log("photonView " + photonView + "  reachable " + reachable +   "   PhotonNetwork.connectionStateDetailed " + PhotonNetwork.connectionState);
            if(PhotonNetwork.connectionState == ConnectionState.Disconnected)
            {
                PhotonNetwork.ConnectUsingSettings(gameVersion);
				Debug.Log("photonView " + photonView + "  reachable " + reachable +   "   PhotonNetwork.connectionStateDetailed " + PhotonNetwork.connectionState);
            }
            else if(PhotonNetwork.connectionState != ConnectionState.Connecting)
            {
                PhotonNetwork.Disconnect();
            }
        }
    }
        
        
    public override void Disconnect()
    {
        if (PhotonNetwork.connectionStateDetailed != ClientState.Disconnected)
        {
            PhotonNetwork.Disconnect();
        }
    }

    void OnJoinedLobby ()
    {
        
    }
    void OnReceivedRoomListUpdate()
    {
        StartUpdatePlayers();
    }
   
    public override void StartUpdatePlayers()
    {
        StartCoroutine(UpdatePlayers());
    }
    private IEnumerator UpdatePlayers()
    {
        NetworkManager.social.UpdateFriendsList();
        while (!NetworkManager.social.friendsListIsUpdated)
        {
            yield return null;
        }
        NetworkManager.UpdatePlayers();
        StartCoroutine(NetworkManager.LoadRandomPlayer());
        StartCoroutine(NetworkManager.LoadFriendsAndRandomPlayers(50));
    }

    void OnConnectedToMaster()
    {
        
    }

    void OnPhotonCreateRoomFailed(object[] codeAndMsg)
    { 
        
    }

    public void OnCreatedRoom()
    {
        StartUpdatePlayers();
        CallNetworkState(NetworkState.CreatedRoom);
    }

    void OnLeftRoom()
    {
        opponentPlayer = null;
        PhotonNetwork.RemoveRPCs(PhotonNetwork.player);
        CallNetworkState(NetworkState.LeftRoom);
    }

    void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        Debug.LogWarning("OnPhotonPlayerDisconnected");
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.RemoveRPCs(otherPlayer);
        PhotonNetwork.DestroyPlayerObjects(otherPlayer);
    }

   
    public override void LoadPlayers(ref NetworkManagement.PlayerProfile[] players)
    {
        RoomInfo[] rooms = PhotonNetwork.GetRoomList();
		List<NetworkManagement.PlayerProfile> playersList = new List<NetworkManagement.PlayerProfile>(0);

        for (int i = 0; i < rooms.Length; i++)
        {
            RoomInfo room = rooms[i];
            if (room.PlayerCount < room.MaxPlayers)
            {
                playersList.Add(NetworkManager.PlayerFromString(rooms[i].Name, CheckIsFriend));
            }
        }
        players = playersList.ToArray();
    }

    public override bool CheckIsFriend(string id)
    {
        string[] friendsId = NetworkManager.social.GetFriendsId();
        if (friendsId != null)
        {
            foreach (string friendId in friendsId)
            {
                if (id == friendId)
                {
                    return true;
                }
            }
        }
        return false;
    }

    [PunRPC]
    public override void OnOpponentReadyToPlay(string playerData)
    {
        NetworkManager.opponentPlayer = NetworkManager.PlayerFromString(playerData, CheckIsFriend);

        Debug.Log("OnOpponenReadToPlay " + playerData);
        CallNetworkState(NetworkState.OpponentReadyToPlay);
        if (PhotonNetwork.player != PhotonNetwork.masterClient)
        {
            int turnId = Random.Range(0, 2);
            int turnIdForSend = turnId == 1 ? 0 : 1;
            OnOpponentStartToPlay(turnId);
            photonView.RPC("OnOpponenStartToPlay", opponentPlayer, turnIdForSend);
        }
    }

    [PunRPC]
    public override void OnOpponentStartToPlay(int turnId)
    {
        Debug.Log(" OnOpponenStartToPlay " + turnId);
        adapter.SetTurn(turnId);
        adapter.homeMenuManager.GoToPlay();
    }

    [PunRPC]
    public override void OnSendTime(float time01)
    {
        messenger.SetTime(time01);
    }
    [PunRPC]
    public override void StartSimulate(string impulse)
    {
        messenger.StartSimulate(impulse);
    }
    [PunRPC]
    public override void EndSimulate(string ballsState)
    {
        messenger.EndSimulate(ballsState);
    }
    [PunRPC]
    public override void OnOpponentWaitingForYourTurn()
    {
        base.OnOpponentWaitingForYourTurn();
    }
    [PunRPC]
    public override void OnOpponentInGameScene()
    {
        StartCoroutine(messenger.OnOpponenInGameScene());
    }
    [PunRPC]
    public override void OnOpponentForceGoHome()
    {
        messenger.OnOpponentForceGoHome();
    }
    #region AightBallPool Interface
    [PunRPC]
    public void OnSendCueControl(float cuePivotLocalRotationY, float cueVerticalLocalRotationX, Vector2 cueDisplacementLocalPositionXY, float cueSliderLocalPositionZ, float force)
    {
        messenger.OnSendCueControl(cuePivotLocalRotationY, cueVerticalLocalRotationX, cueDisplacementLocalPositionXY, cueSliderLocalPositionZ, force);
    }
    [PunRPC]
    public void OnForceSendCueControl(float cuePivotLocalRotationY, float cueVerticalLocalRotationX, Vector2 cueDisplacementLocalPositionXY, float cueSliderLocalPositionZ, float force)
    {
        messenger.OnForceSendCueControl(cuePivotLocalRotationY, cueVerticalLocalRotationX, cueDisplacementLocalPositionXY, cueSliderLocalPositionZ, force);
    }
    [PunRPC]
    public void OnMoveBall(Vector3 ballPosition)
    {
        messenger.OnMoveBall(ballPosition);
    }
    [PunRPC]
    public void SelectBallPosition(Vector3 ballPosition)
    {
        messenger.SelectBallPosition(ballPosition);
    }
    [PunRPC]
    public void SetBallPosition(Vector3 ballPosition)
    {
        messenger.SetBallPosition(ballPosition);
    }

    [PunRPC]
    public void SetMechanicalStatesFromNetwork(int ballId, string mechanicalStateData)
    {
        messenger.SetMechanicalStatesFromNetwork(ballId, mechanicalStateData);
    }
    [PunRPC]
    public void WaitAndStopMoveFromNetwork(float time)
    {
        messenger.WaitAndStopMoveFromNetwork(time);
    }
    [PunRPC]
    public void SendOpponentCueURL(string url)
    {
        messenger.SetOpponentCueURL(url);
    }
    [PunRPC]
    public void SendOpponentTableURLs(string boardURL, string clothURL, string clothColor)
    {
        messenger.SetOpponentTableURLs(boardURL, clothURL, clothColor);
    }
    #endregion
}
#endif
