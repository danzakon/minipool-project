using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinipoolSocial
{

	public void SaveMainPlayerName(string playerName)
    {
        PlayerPrefs.SetString("MainPlayerName", playerName);
    }

    public string GetMainPlayerName()
    {
        string savedName = PlayerPrefs.GetString("MainPlayerName");
        if (string.IsNullOrEmpty(savedName))
        {
            savedName = "Dan" + Random.Range(1, 1000);
            SaveMainPlayerName(savedName);
        }
        return savedName;
    }


    public void SaveMainPlayerCoins(int playerCoins)
    {
        PlayerPrefs.SetInt("MainPlayerCoins", playerCoins);
    }


    public int GetMainPlayerCoins()
    {
        int savedCoins = PlayerPrefs.GetInt("MainPlayerCoins");
        if (savedCoins < 5)
        {
            savedCoins = 50;
            SaveMainPlayerCoins(savedCoins);
        }
        return savedCoins;
    }


    public void SaveMainPlayerID(int playerID)
    {
        PlayerPrefs.SetInt("MainPlayerID", playerID);
    }


    public int GetMainPlayerID()
    {
        int savedID = PlayerPrefs.GetInt("MainPlayerID");
        if (savedID == 0 || savedID == null)
        {
            savedID = Random.Range(1, 100000);;
            SaveMainPlayerID(savedID);
        }
        return savedID;
    }

}
