using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoManager : MonoBehaviour
{
    private float timer = 5f;

    // Update is called once per frame
    void Update()
    {
        timer -= 1 * Time.deltaTime;

        if(Input.GetAxisRaw("Jump") > 0 && timer >= 0.5f)
		{
            timer = 0.5f;
		}

        //if the timer is initialized
        if (timer < 0.5f)
        {
            GameObject.Find("Sliders").GetComponent<Animator>().SetTrigger("Close");
            //if the timer is less than 0, jump to level select
            if (timer <= 0)
            {
                SceneManager.LoadScene("Title");
            }
        }
    }
}
