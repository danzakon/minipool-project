using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetworkManagement;

public class LocalNetwork : NetworkEngine
{
    public override void Initialize()
    {

    }
    public override void Disconnect()
    {
        
    }
    public override void SendRemoteMessage(string message, params object[] args)
    {
        
    }
    public override void OnGoToPlayWithPlayer(NetworkManagement.PlayerProfile player)
    {
       
    }

    protected override void Update()
    {
        
    }
    public override void OnSendTime(float time01)
    {

    }
    public override void Connect()
    {
        
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
        players = null;
    }
    public override bool CheckIsFriend(string id)
    {
        return false;
    }
}
