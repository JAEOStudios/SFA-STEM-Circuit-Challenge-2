using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
	[SerializeField] private AudioSource sound;
	//checking if the player is overlapping the portal, and calling the bot's portal animation if so
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.CompareTag("Bot"))
		{
			sound.Play();
			collision.gameObject.GetComponent<CharacterMovement>().TouchPortal(this.transform.position);
		}
	}
}
