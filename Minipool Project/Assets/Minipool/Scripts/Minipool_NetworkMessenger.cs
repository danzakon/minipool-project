using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BallPool;
using NetworkManagement;

    //public interface MinipoolMessenger
    //{
    //    void OnSendCueControl(float cuePivotLocalRotationY, float cueVerticalLocalRotationX, Vector2 cueDisplacementLocalPositionXY, float cueSliderLocalPositionZ, float force);
    //}


public class MinipoolNetworkMessenger : MonoBehaviour {
    private Minipool_ShotController _shotController;
    private Minipool_ShotController shotController
    {
        get
        {
            if (!_shotController)
            {
                _shotController = Minipool_ShotController.FindObjectOfType<Minipool_ShotController>();
            }
            return _shotController;
        }
    }


    private dans_GameManager _gameManager;
    private dans_GameManager gameManager
    {
        get
        {
            if (!_gameManager)
            {
                _gameManager = dans_GameManager.FindObjectOfType<dans_GameManager>();
            }
            return _gameManager;
        }
    }

    #region sended from network
    public void SetTime(float time01)
    {

    }

    public void SetOpponentCueURL(string url)
    {

    }
    public void SetOpponentTableURLs(string boardURL, string clothURL, string clothColor)
    {

    }

    public IEnumerator OnOpponentInGameScene()
    {
        yield return null;
    }

    public void OnOpponentForceGoHome()
    {

    }

    public void OnSendCueControl(float cuePivotLocalRotationY, float cueVerticalLocalRotationX, Vector2 cueDisplacementLocalPositionXY, float cueSliderLocalPositionZ, float force)
    {

    }
    public void OnForceSendCueControl(float cuePivotLocalRotationY, float cueVerticalLocalRotationX, Vector2 cueDisplacementLocalPositionXY, float cueSliderLocalPositionZ, float force)
    {

    }

    public void OnMoveBall(Vector3 ballPosition)
    {

    }

    public void SelectBallPosition(Vector3 ballPosition)
    {

    }

    public void SetBallPosition(Vector3 ballPosition)
    {

    }

    public void SetMechanicalStatesFromNetwork(int ballId, string mechanicalStateData)
    {

    }

    public void WaitAndStopMoveFromNetwork(float time)
    {

    }

    public void StartSimulate(string impulse)
    {

    }

    public void EndSimulate(string data)
    {

    }
    #endregion
}
