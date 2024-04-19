using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class VictoryManager : MonoBehaviour
{
    //the text on the victory screen to update with the beaten level pack
    [SerializeField] private TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        //instantiating the level manager
        LevelManager l = GameObject.Find("LevelManager").gameObject.GetComponent<LevelManager>();

        //changing the text and destroying the level manager object to be recreated in the level select scene
        text.text = "Congratulations \n\n you have beaten the " + l.GetLevelPack();
        GameObject.Destroy(l.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //checking if the user has pressed space
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("LevelSelect");
        }
    }
}
