using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NetworkManagement;

public class MinipoolPlayerProfileUI : MonoBehaviour
{
    [SerializeField] private Text userName;
    [SerializeField] private Text coins;

    void Update()
    {

    }

    public NetworkManagement.PlayerProfile player
    {
        get;
        private set;
    }

    public void UpdateCoinsFromPlayer()
    {
        if (player != null)
        {
            coins.text = player.coins + "";
        }
    }

    public void SetPlayer(NetworkManagement.PlayerProfile player)
    {
        this.player = player;
        if (player == null)
        {
            enabled = true;
            userName.text = "";
            coins.text = "";
            return;
        }

        userName.text = player.userName;
        coins.text = player.coins + "";

    }
}
