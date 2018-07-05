using System.Collections;
using System.Collections.Generic;
using System;
using NetworkManagement;
using BallPool;


public class AightBallPoolNetworkGameAdapter : NetworkGameAdapter
{
    public static bool is3DGraphics = true;
    public static bool isSameGraphicsMode;
    private HomeMenuManager _homeMenuManager;
    public HomeMenuManager homeMenuManager
    {
        get{ return _homeMenuManager; }
    }
    public AightBallPoolNetworkGameAdapter( HomeMenuManager homeMenuManager)
    {
        _homeMenuManager = homeMenuManager;
    }
    public void SetTurn(int turnId)
    {
        BallPoolPlayer.turnId = turnId;
    }
    public void OnMainPlayerLoaded(int playerId, string name, int coins, object avatar, string avatarURL)
	{
        if(!BallPoolPlayer.initialized)
		{
            BallPoolPlayer.players = new BallPoolPlayer[2];
            BallPoolPlayer.playersCount = 2;
		}
        BallPoolPlayer.players[0] = new AightBallPoolPlayer(0, name, coins, avatar, avatarURL);
		//AightBallPoolPlayer.prize = prize;
	}
	public void OnUpdateMainPlayerName (string name)
	{
		AightBallPoolPlayer.mainPlayer.name = name;
	}
	public void OnUpdatePrize (int prize)
	{
		//AightBallPoolPlayer.prize = prize;
	}
    public void GoToReplay()
    {
        BallPoolGameLogic.playMode = GamePlayMode.Replay;
        homeMenuManager.GoToPlay();
    }
    public void GoToReplayFromSharedData()
    {
        BallPoolGameLogic.playMode = GamePlayMode.Replay;
        if(!BallPoolPlayer.initialized)
        {
            BallPoolPlayer.players = new BallPoolPlayer[2];
            BallPoolPlayer.playersCount = 2;
        }
        homeMenuManager.GoToPlay();
    }
	public void OnGoToPlayWithAI(int playerId, string name, int coins, object avatar, string avatarURL)
	{
        //AightBallPoolPlayer.prize = NetworkManager.mainPlayer.prize;
        BallPoolGameLogic.playMode = GamePlayMode.PlayerAI;
        BallPoolPlayer.players[0].SetCoins(NetworkManager.mainPlayer.coins);
        BallPoolPlayer.players[1] = new MinipoolPlayer(1, "sharon", coins, avatar, avatarURL);
        homeMenuManager.GoToPlay();
	}
	public void OnGoToPlayHotSeatMode (int playerId, string name, int coins, object avatar, string avatarURL)
	{
        //AightBallPoolPlayer.prize = NetworkManager.mainPlayer.prize;
        BallPoolGameLogic.playMode = GamePlayMode.HotSeat;
        BallPoolPlayer.players[0].SetCoins(NetworkManager.mainPlayer.coins);
        BallPoolPlayer.players[1] = new AightBallPoolPlayer(1, name, coins, avatar, avatarURL);
        homeMenuManager.GoToPlay();
	}
    public void OnGoToPlayWithPlayer (PlayerProfile player)
	{
        //AightBallPoolPlayer.prize = NetworkManager.mainPlayer.prize;
        BallPoolGameLogic.playMode = GamePlayMode.Online;
        BallPoolPlayer.players[0].SetCoins(NetworkManager.mainPlayer.coins);
        BallPoolPlayer.players[1] = new AightBallPoolPlayer(1, player.userName, player.coins, player.image, player.imageURL);
        NetworkManager.network.OnGoToPlayWithPlayer(player);
	}
}
