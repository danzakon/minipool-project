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

    }

    public void GoHome()
    {
        physicsManager.Disable();
        SceneManager.LoadScene(homeScene);
    }

}
