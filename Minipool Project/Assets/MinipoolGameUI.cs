using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;
using NetworkManagement;
using BallPool;
using BallPool.Mechanics;


public class MinipoolGameUI : MonoBehaviour
{

    public event Action<bool> OnShot;
    public event Action OnForceGoHome;
    [SerializeField] private PhysicsManager physicsManager;
    [SerializeField] private dans_GameManager gameManager;

    [SerializeField] private string homeScene;
    [SerializeField] private string playScene;


    void Awake()
    {
        if (!NetworkManager.initialized)
        {
            enabled = false;
            return;
        }
    }

    public void GoHome()
    {
        physicsManager.Disable();
        if (NetworkManager.mainPlayer != null)
        {
            NetworkManager.mainPlayer.state = PlayerState.Online;
        }
        SceneManager.LoadScene(homeScene);
    }

}
