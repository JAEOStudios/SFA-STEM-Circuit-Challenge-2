using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
	//strings populated when switching levels and when selecting a level pack
	[SerializeField] private string currentLevel;
    [SerializeField] private string nextLevel;
	[SerializeField] private string levelPack;

	[SerializeField] private AudioSource confirm;

	private float timer = 0;
	public void Start()
	{
		DontDestroyOnLoad(this.transform);
	}

	//setters and getters
	public void UpdateNextLevel(string nextLevel)
	{
        this.nextLevel = nextLevel;
	}

    public string GetNextLevel()
	{
        return this.nextLevel;
	}

	public void UpdateCurrentLevel(string currentLevel)
	{
		this.currentLevel = currentLevel;
	}

	public string GetCurrentLevel()
	{
		return this.currentLevel;
	}

	public void UpdateLevelPack(string levelPack)
	{
		this.levelPack = levelPack;
	}

	public string GetLevelPack()
	{
		return this.levelPack;
	}

	//method to move forward to the first level of the level pack
	public void startLevelPack()
	{
		timer = 0.5f;
		confirm.Play();
	}

	private void Update()
	{
		if (timer > 0)
		{
			GameObject.Find("Sliders").GetComponent<Animator>().SetTrigger("Close");
			timer -= 1 * Time.deltaTime;
			//if the timer is less than 0, jump to level select
			if (timer <= 0)
			{
				SceneManager.LoadScene("Level");
			}
		}
	}

}
