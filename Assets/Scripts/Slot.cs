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

	public void initializeSlot(int x, int y, int id)
	{
		this.x = x;
		this.y = y;
		this.id = id;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log("It has hit!");
		if (other.CompareTag("Chip"))
		{
			if(other.gameObject.GetComponent<ChipMovement>().GetChipID() == this.id)
			{
				Debug.Log("And it is match!");
				isMatching = true;
			}
		}
	}

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
