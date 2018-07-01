using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager_Minipool : MonoBehaviour
{
	public Player_Minipool[] m_Players;               // A collection of managers for enabling and disabling different aspects of the tanks.

	private int m_playerOneTurn = 0;
	private int m_playerTwoTurn = 1;
	private int m_playerTurn;

	public bool m_isWinner;
	private Player_Minipool m_GameWinner;           // Reference to the winner of the game.  Used to make an announcement of who won.

	public float m_StartDelay = 3f;             // The delay between the start of RoundStarting and RoundPlaying phases.
	public float m_EndDelay = 3f;               // The delay between the end of RoundPlaying and RoundEnding phases.
	private WaitForSeconds m_StartWait;         // Used to have a delay whilst the round starts.
	private WaitForSeconds m_EndWait;           // Used to have a delay whilst the round or game ends.

	void OnEnable()
	{
		Shooting_Minipool.OnShot += ShotTaken;
	}


	void OnDisable()
	{
		Shooting_Minipool.OnShot -= ShotTaken;
	}

	private void Start()
	{
		// Create the delays so they only have to be made once.
		m_StartWait = new WaitForSeconds(m_StartDelay);
		m_EndWait = new WaitForSeconds(m_EndDelay);

		SpawnPlayers();

		// Once the tanks have been created and the camera is using them as targets, start the game.
		StartCoroutine(GameLoop());
	}


	private void SpawnPlayers()
	{
		Debug.Log("players are spawning");
		// For all the players...
		for (int i = 0; i < m_Players.Length; i++)
		{
			// ... create them, set their player number and references needed for control.
			m_Players[i].Setup();
		}
	}

	// This is called from start and will run each phase of the game one after another.
	private IEnumerator GameLoop()
	{
		// Start off by running the 'RoundStarting' coroutine but don't return until it's finished.
		yield return StartCoroutine(GameStarting());

		// Once the 'RoundStarting' coroutine is finished, run the 'RoundPlaying' coroutine but don't return until it's finished.
		yield return StartCoroutine(GamePlaying());

		// Once execution has returned here, run the 'RoundEnding' coroutine, again don't return until it's finished.
		yield return StartCoroutine(GameEnding());
	}


	private IEnumerator GameStarting()
	{
		// start with player one's turn
		m_playerTurn = m_playerOneTurn;

		// Wait for the specified length of time until yielding control back to the game loop.
		Debug.Log("game is starting");
		yield return m_StartWait;
	}


	private IEnumerator GamePlaying()
	{
		EnablePlayerControl (m_playerTurn);

		while (!isWinner())
		{
			// this code will run continuously until the game ends
			yield return null;
		}

		Debug.Log("game should end now");
	}


	private IEnumerator GameEnding()
	{
		Debug.Log("game is ending");
		yield return m_EndWait;
	}


	private void EnablePlayerControl(int m_PlayerNumber)
	{
		m_Players[m_PlayerNumber].EnableControl();
	}


	private void DisablePlayerControl(int m_PlayerNumber)
	{
		m_Players[m_PlayerNumber].DisableControl();
	}

	private bool isWinner()
	{
		return m_isWinner;
	}

	private void ShotTaken(){
		ChangePlayerTurn ();
	}

	private void ChangePlayerTurn()
	{
		// disable control for the current player
		DisablePlayerControl (m_playerTurn);

		// change the m_playerTurn int
		if (m_playerTurn == m_playerOneTurn) 
		{
			m_playerTurn = m_playerTwoTurn;
		} 
		else 
		{
			m_playerTurn = m_playerOneTurn;
		}

		// enable control for the new player
		EnablePlayerControl (m_playerTurn);
	}
}

