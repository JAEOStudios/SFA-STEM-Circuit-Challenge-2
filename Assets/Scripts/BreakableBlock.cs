using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBlock : MonoBehaviour
{
	// Start is called before the first frame update
	public void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.CompareTag("Laser"))
		{
			//GameObject.Destroy(collision.gameObject);
			GameObject.Destroy(this.gameObject);
		}
	}
}
