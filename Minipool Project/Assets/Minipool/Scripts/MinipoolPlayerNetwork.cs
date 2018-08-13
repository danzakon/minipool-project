using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using BallPool;

public class MinipoolPlayerNetwork : MonoBehaviour
{
    public static MinipoolPlayerNetwork Instance;
    public MinipoolSocial social;

    public MinipoolProfile mainPlayer
    {
        get;
        private set;
    }

    public MinipoolProfile otherPlayer
    {
        get;
        private set;
    }

    private PhotonView PhotonView;

    void Awake()
    {
        Instance = this;
        social = new MinipoolSocial();

        PhotonView = GetComponent<PhotonView>();


        mainPlayer = new MinipoolProfile(social.GetMainPlayerID(), social.GetMainPlayerName(), social.GetMainPlayerCoins(), true);
        otherPlayer = new MinipoolProfile(-1, "", -1, false);


        // increase photon sendRate, smoother online functionality at the cost
        // of increased bandwidth use
        PhotonNetwork.sendRate = 60;
        PhotonNetwork.sendRateOnSerialize = 30;

        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }


    void Update()
    {

    }


    private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        //if (scene.name == "Minipool_Gameplay")
        //{
        //    if (PhotonNetwork.isMasterClient)
        //    {
        //        MasterLoadedGame();
        //    }
        //    else
        //    {
        //        NonMasterLoadedGame();
        //    }
        //}
    }

    public void SetUpGame()
    {
        if (PhotonNetwork.isMasterClient)
        {
            MasterLoadedGame();
        }
        else
        {
            NonMasterLoadedGame();
        }
    }


    // you are the master client and you just loaded the game
    private void MasterLoadedGame()
    {
        // send your MinipoolProfile to the other player
        PhotonView.RPC("RPC_LoadOtherPlayer", PhotonTargets.Others, mainPlayer.playerID, mainPlayer.name, mainPlayer.coins);
    }

    // you are not the master client and you just loaded the game
    // call RPC method for other player to load game
    private void NonMasterLoadedGame()
    {
        PhotonView.RPC("RPC_LoadGameOthers", PhotonTargets.Others);

        // send your MinipoolProfile to the other player
        PhotonView.RPC("RPC_LoadOtherPlayer", PhotonTargets.Others, mainPlayer.playerID, mainPlayer.name, mainPlayer.coins);
    }

    // receive call from other player to load the game
    [PunRPC]
    private void RPC_LoadGameOthers()
    {
        SetUpGame();
    }

    // receive call from other player to load their profile
    [PunRPC]
    private void RPC_LoadOtherPlayer(int _otherPlayerID, string _otherPlayerName, int _otherPlayerCoins)
    {
        otherPlayer.SetPlayerID(_otherPlayerID);
        otherPlayer.SetPlayerName(_otherPlayerName);
        otherPlayer.SetPlayerCoins(_otherPlayerCoins);

        SceneManager.LoadScene(1);
    }
}
