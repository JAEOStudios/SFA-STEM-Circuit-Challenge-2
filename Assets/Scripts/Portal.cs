using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
	//checking if the player is overlapping the portal, and starting the next level if so
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.CompareTag("Bot"))
		{
			GameObject.Find("Grid").GetComponent<WorldGenerator>().startNextLevel();
		}
	}
}
