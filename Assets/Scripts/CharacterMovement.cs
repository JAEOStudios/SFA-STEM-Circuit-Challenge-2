using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    //reference to this gameobject
    private GameObject character;

    //current position and coordinates
    private Vector2 position;
    private int xPos = 0;
    private int yPos = 0;

    //current direction and accompanying sprites
    private int direction = 1;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] directionSprites;

    //reference to laser prefab
    [SerializeField] private GameObject laser;

    //health variables
    private int health = 3;
    [SerializeField] private GameObject[] hearts = new GameObject[3];
    // Start is called before the first frame update

    void Start()
    {
        //setting variables and position of character
        character = this.gameObject;
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        position = new Vector2(gameObject.GetComponent<Transform>().position.x, gameObject.GetComponent<Transform>().position.y);

        hearts = GameObject.FindGameObjectsWithTag("Heart");
    }

    // Update is called once per frame
    void Update()
	{
        //checking inputs and if the movements are valid
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			if (ValidMove(Vector2.down))
			{
				MoveCharacter(xPos, yPos - 1);
			}
            //updating sprites
			spriteRenderer.sprite = directionSprites[0];
            direction = 1;
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			if (ValidMove(Vector2.right))
			{
				MoveCharacter(xPos + 1, yPos);
			}
			spriteRenderer.sprite = directionSprites[1];
            direction = 2;
		}
		else if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			if (ValidMove(Vector2.up))
			{
				MoveCharacter(xPos, yPos + 1);
			}
			spriteRenderer.sprite = directionSprites[2];
            direction = 3;
		}

		else if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			if (ValidMove(Vector2.left))
			{
				MoveCharacter(xPos - 1, yPos);
			}
			spriteRenderer.sprite = directionSprites[3];
            direction = 4;
		}

        //creating a laser option in the right direction when space is hit
		else if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject l = GameObject.Instantiate(laser, this.transform.position, Quaternion.identity);
            l.GetComponent<LaserMove>().Initialize(direction);
        }

        //checking if the player is out of health
        if(health <= 0 || Input.GetKeyDown(KeyCode.R))
		{
            GameObject.Find("Grid").GetComponent<WorldGenerator>().restartLevel();
		}
        
    }

    

    //checks if the robot can move this direction
    private bool ValidMove(Vector2 direction)
	{
        RaycastHit2D[] hits = Physics2D.RaycastAll(this.transform.position, direction, 1f);
        Debug.DrawRay(transform.position, direction * 1f, Color.red);
        foreach(RaycastHit2D hit in hits)
		{
            //if wall, no
            if (hit.collider.tag == "Wall" || hit.collider.tag == "Breakable")
            {
                return false;
            }
            //if chip, dependent on if chip can move
            else if (hit.collider.tag == "Chip")
            {
                ChipMovement cm = hit.collider.gameObject.GetComponent<ChipMovement>();
                return (cm.TryChipMove(direction));
            }
            //if another robot, no
            else if(hit.collider.tag == "Bot" && hit.collider.gameObject != this.gameObject)
			{
                return false;
			}
            

        }
            
        
        return true;
    }

    //updating the position of the robot's coords and transform
    private void MoveCharacter(int newX, int newY)
	{
        xPos = newX;
        yPos = newY;
        gameObject.transform.position = new Vector2(xPos + 0.5f, yPos + 0.5f);
	}

    //pulls x and y coords from the world generator
    public void InitializeCharacter(int newX, int newY)
	{
        MoveCharacter(newX, newY);
        Debug.Log("Robot Initialized");

    }

    //taking damage if hitting a hazard
	private void OnTriggerEnter2D(Collider2D collision)
	{

		if(collision.CompareTag("Hazard"))
		{
            health--;
            hearts[health].SetActive(false);
		}
	}
}
