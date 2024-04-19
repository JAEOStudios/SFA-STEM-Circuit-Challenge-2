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
		SceneManager.LoadScene("Level");
	}

}
