using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    private float timer = 0;

    private string destinationLevel = "LevelSelect";

    [SerializeField] private AudioSource confirm;
    // Update is called once per frame
    void Update()
    {
        //sends the user to the level select
        if(Input.GetKeyDown(KeyCode.Space) && timer <= 0)
		{
            timer = 0.5f;
            destinationLevel = "LevelSelect";
            confirm.Play();
		}

        //if the timer is initialized
        if(timer > 0)
		{
            GameObject.Find("Sliders").GetComponent<Animator>().SetTrigger("Close");
            timer -= 1 * Time.deltaTime;
            //if the timer is less than 0, jump to level select
            if(timer <= 0)
			{
                SceneManager.LoadScene(destinationLevel);
            }
		}
    }

    public void Credits()
	{
        timer = 0.5f;
        destinationLevel = "Credits";
        confirm.Play();
	}
}
