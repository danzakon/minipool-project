using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Minipool : MonoBehaviour
{
	public int m_PlayerNumber;            // This specifies which player this the manager for.

	private Shooting_Minipool m_Shooting;   // Reference to tank's shooting script, used to disable and enable control.


	public void Setup()
	{
		// Get references to the components.
		Debug.Log("Player" + m_PlayerNumber.ToString() + " has been setup");

		m_Shooting = gameObject.GetComponent<Shooting_Minipool>();
		m_Shooting.m_playerNumber = m_PlayerNumber;
	}


	// Used during the phases of the game where the player shouldn't be able to control their tank.
	public void DisableControl()
	{
		m_Shooting.enabled = false;
	}


	// Used during the phases of the game where the player should be able to control their tank.
	public void EnableControl()
	{
		Debug.Log("Player" + m_PlayerNumber.ToString() + " has been enabled to shoot");
		m_Shooting.enabled = true;
	}
}


