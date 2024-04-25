using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectManager : MonoBehaviour
{
    public int selection = 0;
    [SerializeField] Button[] buttons;

    [SerializeField] GameObject selector;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.DownArrow))
		{
            selection++;
		}
        else if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.UpArrow))
		{
            selection--;
		}

        if(selection >= buttons.Length)
		{
            selection = 0;
		}
        else if(selection < 0)
		{
            selection = buttons.Length - 1;
		}

        selector.transform.position = new Vector2(buttons[selection].transform.position.x, buttons[selection].transform.position.y + 50);

        if(Input.GetKeyDown(KeyCode.Space))
		{
            buttons[selection].onClick.Invoke();
		}
        
        
    }
}
