using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectManager : MonoBehaviour
{
    public int selection = 0;
    [SerializeField] Button[] buttons;

    [SerializeField] GameObject selector;

    //input variables
    private bool moveUp = true;
    private bool moveDown = true;
    private bool moveLeft = true;
    private bool moveRight = true;
    private bool shoot = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //updating input booleans
        if (Input.GetAxisRaw("Horizontal") < 0.1f)
        {
            moveRight = true;
        }
        if (Input.GetAxisRaw("Horizontal") > -0.1f)
        {
            moveLeft = true;
        }
        if (Input.GetAxisRaw("Vertical") < 0.1f)
        {
            moveUp = true;
        }
        if (Input.GetAxisRaw("Vertical") > -0.1f)
        {
            moveDown = true;
        }
        if (Input.GetAxisRaw("Jump") < 0.1f)
        {
            shoot = true;
        }

        if ((Input.GetAxisRaw("Horizontal") > 0.5f && moveRight) || (Input.GetAxisRaw("Vertical") < -0.5f && moveDown))
		{
            moveDown = false;
            moveRight = false;
            selection++;
            this.GetComponent<AudioSource>().Play();
		}
        else if((Input.GetAxisRaw("Horizontal") < -0.5f && moveLeft) || (Input.GetAxisRaw("Vertical") > 0.5f && moveUp))
		{
            moveUp = false;
            moveLeft = false;
            selection--;
            this.GetComponent<AudioSource>().Play();
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

        if(Input.GetAxisRaw("Jump") > 0 && shoot)
		{
            shoot = false;
            buttons[selection].onClick.Invoke();
		}
        
        
    }
}
