using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetworkManagement;

public class NetworkExample : NetworkEngine
{
    [SerializeField] private NetworkState oldState;
    [SerializeField] private float time;

    public override void Initialize()
    {

    }

    public override void Disconnect()
    {

    }
    public override void SendRemoteMessage(string message, params object[] args)
    {
        //        System.Reflection.MethodInfo methodInfo = messenger.GetType().GetMethod(message);
        //        methodInfo.Invoke(messenger, args);
    }
    public override void OnGoToPlayWithPlayer(NetworkManagement.PlayerProfile player)
    {
        Debug.LogWarning("OnGoToPLayWithPlayer " + player);
        adapter.SetTurn(Random.Range(0, 2));
        adapter.OnGoToPlayWithAI(1, player.userName, player.coins, player.image, player.imageURL);
    }

    protected override void Update()
    {
        base.Update();

        NetworkManager.network.SendRemoteMessage("SetTime", 0, time);
        if (state != oldState)
        {
            CallNetworkState(oldState);
        }
    }
    public override void OnSendTime(float time01)
    {

    }
    public override void Connect()
    {
        oldState = NetworkState.Connected;
    }

    public override void CreateRoom()
    {

    }
    public override void LeftRoom()
    {

    }
    public override void Reset()
    {

    }
    public override void StartSimulate(string ballsState)
    {

    }
    public override void EndSimulate(string ballsState)
    {

    }

    public override void OnOpponentReadyToPlay(string playerData)
    {

    }
    public override void OnOpponentStartToPlay(int turnId)
    {

    }
    public override void OnOpponentInGameScene()
    {

    }
    public override void OnOpponentForceGoHome()
    {

    }
    public override void StartUpdatePlayers()
    {

    }
    public override void LoadPlayers(ref NetworkManagement.PlayerProfile[] players)
    {
        //Imitating players loading
        HomeMenuManager homeMenuManager = HomeMenuManager.FindObjectOfType<HomeMenuManager>();
        players = new NetworkManagement.PlayerProfile[homeMenuManager.defaultAvatars.Length];
        int randomOnline = Random.Range(0, players.Length);

        for (int i = 0; i < homeMenuManager.defaultAvatars.Length; i++)
        {
            bool isFriend = (i == randomOnline? true : Random.Range(0, 3) != 0) && (LoginManager.logined || LoginManager.loginedFacebook);
            PlayerState state = Random.Range(0, 5) != 0 ? PlayerState.Online : (PlayerState)(Random.Range((int)PlayerState.Online, (int)PlayerState.Playing + 1));
            if (i == randomOnline)
            {
                state = PlayerState.Online;
            }
            int coins = Random.Range(30, 10000);
            int mainPlayerSavedPrize = NetworkManager.social.GetMainPlayerCoins();
//            players[i] = new PlayerProfile(i + "", false, homeMenuManager.defaultAvatars[i], "", homeMenuManager.defaultAvatars[i].name, isFriend, homeMenuManager.defaultAvatars[i].name, state, coins);
        }

    }
    public override bool CheckIsFriend(string id)
    {
        return false;
    }
}
