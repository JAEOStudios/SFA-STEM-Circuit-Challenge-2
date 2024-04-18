using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] private int nextLevel = 1;
	// Start is called before the first frame update

	public void Initialize(int nextLevel)
	{
		this.nextLevel = nextLevel;
	}


	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.CompareTag("Bot"))
		{
			SceneManager.LoadScene(nextLevel);
		}
	}
}
