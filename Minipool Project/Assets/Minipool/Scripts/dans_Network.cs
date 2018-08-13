using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetworkManagement;
using UnityEngine.SceneManagement;
using BallPool;

public class dans_Network : MonoBehaviour
{
    private PhotonView photonView;
    private string gameVersion = "1.01";

    private bool reachable { get { return (Application.internetReachability != NetworkReachability.NotReachable); } }

    void Awake()
    {
        if (!PhotonNetwork.connected)
        {
            print("Connecting to server...");
            PhotonNetwork.ConnectUsingSettings(gameVersion);
        }

        photonView = this.gameObject.AddComponent<PhotonView>();
        photonView.ObservedComponents = new List<Component>(0);
        photonView.ObservedComponents.Add(this);
        photonView.synchronization = ViewSynchronization.ReliableDeltaCompressed;
        photonView.viewID = 1;
        photonView = gameObject.GetComponent<PhotonView>();
    }

    private void OnConnectedToMaster()
    {
        print("Connected to master");
        PhotonNetwork.automaticallySyncScene = false;
        PhotonNetwork.playerName = MinipoolPlayerNetwork.Instance.mainPlayer.name;
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    private void OnJoinedLobby()
    {
        print("Joined lobby");
    }

    public void OnJoinedRoom()
    {

    }

    void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {

    }

    void OnConnectedToPhoton()
    {

    }

    void OnMasterClientSwitched(PhotonPlayer newMasterClient)
    {

    }

    void OnFailedToConnectToPhoton(object parameters)
    {

    }

    void OnDisconnectedFromPhoton()
    {

    }

    public void CreateRoom()
    {

    }


    public void Connect()
    {

    }


    public void Disconnect()
    {

    }

}
