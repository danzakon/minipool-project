using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Shooting_Minipool : MonoBehaviour
{
	public int m_playerNumber;

	public delegate void ShotAction();
	public static event ShotAction OnShot;

	void Update()
	{
		if (Input.GetMouseButtonDown (0)) 
		{
			Debug.Log ("Player " + m_playerNumber + " just clicked");
			OnShot ();
		}
	}
}


