using System.Collections;
using System.Collections.Generic;
using System;
using NetworkManagement;
using BallPool;


public class MinipoolNetworkGameAdapter 
{

    private HomeMenuManager _homeMenuManager;
    public HomeMenuManager homeMenuManager
    {
        get { return _homeMenuManager; }
    }

    private MinipoolHomeManager _homeManager;
    public MinipoolNetworkGameAdapter(MinipoolHomeManager homeManager)
    {
        _homeManager = homeManager;
    }

    public void SetTurn(int turnId)
    {
        MinipoolPlayer.turnId = turnId;
    }

    public void OnMainPlayerLoaded(int playerId, string name, int coins, object avatar, string avatarURL)
    {
        if (!MinipoolPlayer.initialized)
        {
            MinipoolPlayer.players = new BallPoolPlayer[2];
            MinipoolPlayer.playersCount = 2;
        }
        MinipoolPlayer.players[0] = new AightBallPoolPlayer(0, name, coins, avatar, avatarURL);
    }

    public void OnUpdateMainPlayerName(string name)
    {
        MinipoolPlayer.mainPlayer.name = name;
    }

    public void OnUpdatePrize(int prize)
    {
//        MinipoolPlayer.prize = prize;
    }

    public void GoToReplay()
    {
        MinipoolGameLogic.playMode = GamePlayMode.Replay;
        homeMenuManager.GoToPlay();
    }

    public void GoToReplayFromSharedData()
    {
        MinipoolGameLogic.playMode = GamePlayMode.Replay;
        if (!BallPoolPlayer.initialized)
        {
            BallPoolPlayer.players = new BallPoolPlayer[2];
            BallPoolPlayer.playersCount = 2;
        }
        homeMenuManager.GoToPlay();
    }

    public void OnGoToPlayWithAI(int playerId, string name, int coins, object avatar, string avatarURL)
    {
        MinipoolGameLogic.playMode = GamePlayMode.PlayerAI;
        MinipoolPlayer.players[0].SetCoins(NetworkManager.mainPlayer.coins);
        MinipoolPlayer.players[1] = new MinipoolPlayer(1, "Robot Dan", coins, avatar, avatarURL);
        homeMenuManager.GoToPlay();
    }
    public void OnGoToPlayHotSeatMode(int playerId, string name, int coins, object avatar, string avatarURL)
    {
        MinipoolGameLogic.playMode = GamePlayMode.HotSeat;
        MinipoolPlayer.players[0].SetCoins(NetworkManager.mainPlayer.coins);
        MinipoolPlayer.players[1] = new MinipoolPlayer(1, name, coins, avatar, avatarURL);
        homeMenuManager.GoToPlay();
    }
    public void OnGoToPlayWithPlayer(MinipoolProfile _mainPlayer, MinipoolProfile _otherPlayer)
    {
        //MinipoolGameLogic.playMode = GamePlayMode.Online;
        //MinipoolPlayer.players[0] = new MinipoolPlayer(_mainPlayer.playerID, _mainPlayer.name, _mainPlayer.coins, null, null);
        //MinipoolPlayer.players[1] = new MinipoolPlayer(_otherPlayer.playerID, _otherPlayer.name, _otherPlayer.coins, null, null);
    }
}
