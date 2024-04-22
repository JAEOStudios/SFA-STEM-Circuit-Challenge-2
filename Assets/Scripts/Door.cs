using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
	//audio source for when door is opened
	[SerializeField] private AudioSource openNoise;

	// Start is called before the first frame update
	private int x = 0;
	private int y = 0;

	private bool isOpen = false;

	public void initializeDoor(int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	public bool MatchCoords(int x, int y)
	{
		if (this.x == x && this.y == y)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public void Open()
	{
		if(isOpen == false)
		{
			//playing the door open noise
			openNoise.Play();

			isOpen = true;
			this.gameObject.tag = "Untagged";
			this.gameObject.GetComponent<Animator>().SetBool("IsOpen", true);
		}
		//this.GetComponent<SpriteRenderer>().enabled = false;
	}

	public void Close()
	{
		if(isOpen == true)
		{
			isOpen = false;
			this.gameObject.tag = "Wall";
			this.gameObject.GetComponent<Animator>().SetBool("IsOpen", false);
		}
		//this.GetComponent<SpriteRenderer>().enabled = true;
	}
}
