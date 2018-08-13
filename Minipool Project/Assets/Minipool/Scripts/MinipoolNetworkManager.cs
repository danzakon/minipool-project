using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Xml;


public enum PlayerState
{
    Online = 0,
    Away,
    Busy,
    Offline,
    Playing
}

public class MinipoolRoom
{

    public int roomID
    {
        get;
        private set;
    }

    public int roomPrize
    {
        get;
        set;
    }

    public MinipoolRoom(int _roomID, int _roomPrize, List<MinipoolProfile> _roomPlayers)
    {
        this.roomID = _roomID;
        this.roomPrize = _roomPrize;
        this.roomPlayers = _roomPlayers;

        mainPlayer = (roomPlayers == null || roomPlayers.Count == 0) ? null : roomPlayers[0];

        foreach (MinipoolProfile player in roomPlayers)
        {
            player.roomID = roomID;
            Debug.Log("Player " + player.name + "is in the house");
        }
    }

    public MinipoolProfile mainPlayer
    {
        get;
        private set;
    }

    public List<MinipoolProfile> roomPlayers
    {
        get;
        private set;
    }
}


public class MinipoolProfile
{
    public int playerID
    {
        get;
        private set;
    }

    public void SetPlayerID(int _playerID)
    {
        playerID = _playerID;
    }

    public int roomID
    {
        get;
        set;
    }

    public bool isMain
    {
        get;
        private set;
    }

    public string name
    {
        get;
        private set;
    }

    public void SetPlayerName(string _name)
    {
        name = _name;
    }

    public int coins
    {
        get;
        private set;
    }

    public void SetPlayerCoins(int _coins)
    {
        coins = _coins;
    }

    public PlayerState state
    {
        get;
        set;
    }

    public MinipoolProfile(int _playerID, string _name, int _coins, bool _isMain)
    {
        this.playerID = _playerID;
        this.isMain = _isMain;
        this.name = _name;
        this.coins = _coins;
    }
}


public class MinipoolNetworkManager
{
    public static bool initialized = false;

    public static MinipoolProfile mainPlayer
    {
        get;
        private set;
    }

    public static MinipoolProfile opponentPlayer
    {
        get;
        private set;
    }

    private static MinipoolProfile[] _players;
    public static MinipoolProfile[] players
    {
        get
        {
            if (_players == null)
            {
                LoadPlayers();
            }
            return _players;
        }
    }


    public static void UpdatePlayers()
    {
        _players = null;
        LoadPlayers();
    }

    private static void LoadPlayers()
    {

    }

   
}

