using System.Collections;
using System.Collections.Generic;

using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using NetworkManagement;

public class MinipoolHomeManager : MonoBehaviour
{
    [Header("Managers")]
    //[SerializeField] private LoginMenuManager loginManager;
    //[SerializeField] private RoomsListManager roomsListManager;
    [SerializeField] private NetworkGameAdapter networkGameAdapter;

    [Header("Player UI")]
    [SerializeField] private MinipoolPlayerProfileUI mainPlayerUI;
    [SerializeField] private MinipoolPlayerProfileUI opponentUI;
    [SerializeField] private InputField nameInput;


    [Header("Buttons")]
    [SerializeField] private Button createRoomButton;
    [SerializeField] private Button leftRoomButton;

    [Header("Info")]
    private bool roomCreated;
    [SerializeField] private string playScene;


    void Awake()
    {
        DataManager.SaveGameData();
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        NetworkManager.initialized = true;

        roomCreated = false;

        NetworkManager.network.SetAdapter(networkGameAdapter);

        // events
        NetworkManager.OnMainPlayerLoaded += NetworkManager_OnMainPlayerLoaded;
        NetworkManager.OnRandomPlayerLoaded += NetworkManager_OnRandomPlayerLoaded;
        NetworkManager.OnFriendsAndRandomPlayersLoaded += NetworkManager_OnFriendsAndRandomPlayersLoaded;
        NetworkManager.network.OnNetwork += NetworkManager_network_OnNetwork;


        // if the main player edits their name
        nameInput.onEndEdit.AddListener((string playerName) =>
        {
            if (playerName != "Guest")
            {
                networkGameAdapter.OnUpdateMainPlayerName(playerName);
                NetworkManager.mainPlayer.UpdateName(playerName);
                mainPlayerUI.SetPlayer(NetworkManager.mainPlayer);
            }
        });

        NetworkManager.network.Reset();
        UpdatePlayersList();
    }

    void Start()
    {
        Resources.UnloadUnusedAssets();
    }

    void OnDisable()
    {
        NetworkManager.Disable();
    }

    private void NetworkManager_network_OnNetwork(NetworkState state)
    {
        if (state == NetworkState.Connected)
        {

        }
        else if (state == NetworkState.OpponentReadyToPlay)
        {

        }
        else if (state == NetworkState.JoinedToRoom || state == NetworkState.CreatedRoom)
        {

        }
    }


    void NetworkManager_OnFriendsAndRandomPlayersLoaded(PlayerProfile[] players)
    {
        FindPlayersByNamePrizeIsOnlineAndIsFriend();
    }


    void NetworkManager_OnRandomPlayerLoaded(NetworkManagement.PlayerProfile player)
    {

    }

    void NetworkManager_OnMainPlayerLoaded(NetworkManagement.PlayerProfile player)
    {
        networkGameAdapter.OnMainPlayerLoaded(0, player.userName, player.coins, player.image, player.imageURL);
        mainPlayerUI.SetPlayer(player);
        nameInput.text = player.userName;
        Debug.Log("main player loaded");
    }

    public void GoToPlayWithAI()
    {
        string opponentName = "penelope";
        int opponentCoins = 50;


        networkGameAdapter.OnGoToPlayWithAI(1, opponentName, opponentCoins, null, "");
    }


    public void GoToPlayWithPlayer(PlayerProfileUI playerUI)
    {
        networkGameAdapter.OnGoToPlayWithPlayer(playerUI.player);
    }

    public void GoToPlay()
    {
        if (NetworkManager.mainPlayer != null)
        {
            NetworkManager.mainPlayer.state = PlayerState.Playing;
        }
        SceneManager.LoadScene(playScene);
    }

    public void CancelGoToPlay(PlayerState opponentState, string playerId)
    {
        NetworkManager.FindPlayer(playerId).state = opponentState;
        NetworkManager.UpdatePlayers();
        StartCoroutine(NetworkManager.LoadRandomPlayer());
    }

    public void CreateRoom()
    {
        NetworkManager.network.CreateRoom();
    }



    public void FindPlayersByNamePrizeIsOnlineAndIsFriend()
    {
        //NetworkManagement.PlayerProfile[] playerProfiles = NetworkManager.FindPlayers();
        //NetworkManagement.Room[] rooms = new NetworkManagement.Room[playerProfiles.Length];
        //for (int i = 0; i < rooms.Length; i++)
        //{
        //    List<PlayerProfile> players = new List<PlayerProfile>(0);
        //    players.Add(playerProfiles[i]);
        //    CheckOpponentImageByName(playerProfiles[i]);
        //    rooms[i] = new NetworkManagement.Room(i, prize, players);
        //}

        //roomsListManager.UpdateRooms(rooms);
    }

    public void UpdatePlayersList()
    {
        NetworkManager.UpdatePlayers();
        StartCoroutine(NetworkManager.LoadRandomPlayer());
        StartCoroutine(NetworkManager.LoadFriendsAndRandomPlayers(50));
    }
}
