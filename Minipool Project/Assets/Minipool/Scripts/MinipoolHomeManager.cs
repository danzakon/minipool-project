using System.Collections;
using System.Collections.Generic;

using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using NetworkManagement;

public class MinipoolHomeManager : MonoBehaviour
{
    [SerializeField] private InputField nameField;
    [SerializeField] private Text name;
    [SerializeField] private Text coins;
    [SerializeField] private string playScene;

	private void Awake()
	{
        name.text = MinipoolPlayerNetwork.Instance.mainPlayer.name;
        coins.text = MinipoolPlayerNetwork.Instance.mainPlayer.coins.ToString();
	}

    public void MainPlayedChangedName()
    {
        MinipoolPlayerNetwork.Instance.social.SaveMainPlayerName(nameField.text);
        name.text = MinipoolPlayerNetwork.Instance.social.GetMainPlayerName();
    }

	public void OnClick_Play()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    private void OnJoinedRoom()
    {
        print("joined room " + PhotonNetwork.room.Name);
        if (PhotonNetwork.playerList.Length == 2)
        {
            MinipoolPlayerNetwork.Instance.SetUpGame();
        }
    }

    private void OnPhotonRandomJoinFailed()
    {
        RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 2 };
        string randRoomName = Random.Range(10000, 999999999).ToString();

        if (PhotonNetwork.CreateRoom(randRoomName, roomOptions, TypedLobby.Default))
        {
            print("created room " + randRoomName);
        }
        else
        {
            print("create room failed to send");
        }
    }

    private void OnPhotonCreateRoomFailed(object[] codeAndMessage)
    {
        print("create room failed: " + codeAndMessage[1]);
    }

    private void OnCreatedRoom()
    {
        print("room created successfully");
    }
}
