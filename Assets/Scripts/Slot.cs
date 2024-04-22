using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
	// Start is called before the first frame update
	bool isMatching = false;
	int id = 0;
	int x = 0;
	int y = 0;

	//reference to spark when chip is connected
	[SerializeField] private GameObject spark;
	[SerializeField] private AudioSource connectedSound;

	public void initializeSlot(int x, int y, int id)
	{
		this.x = x;
		this.y = y;
		this.id = id;
	}

	//checking if the chip corresponds and setting variable
	private void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log("It has hit!");
		if (other.CompareTag("Chip"))
		{
			if(other.gameObject.GetComponent<ChipMovement>().GetChipID() == this.id)
			{
				//creating the spark
				GameObject.Instantiate(spark, this.transform);
				//playing the connected sound
				connectedSound.Play();

				Debug.Log("And it is match!");
				isMatching = true;
			}
		}
	}

	//turning off variable if chip is taken away
	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Chip"))
		{
			if (other.gameObject.GetComponent<ChipMovement>().GetChipID() == this.id)
			{
				isMatching = false;
			}
		}
	}

	public bool GetIsMatching()
	{
		return isMatching;
	}

	//checks if the chip matches to the coords passed in
	public bool MatchCoords(int x, int y)
	{
		if(this.x == x && this.y == y)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}
