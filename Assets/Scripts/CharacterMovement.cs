using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    GameObject character;
    Vector2 position;
    int xPos = 0;
    int yPos = 0;
    int direction = 1;
    SpriteRenderer spriteRenderer;
    [SerializeField] Sprite[] directionSprites;

    [SerializeField] private GameObject laser;
    // Start is called before the first frame update

    void Start()
    {
        character = this.gameObject;
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        position = new Vector2(gameObject.GetComponent<Transform>().position.x, gameObject.GetComponent<Transform>().position.y);
    }

    // Update is called once per frame
    void Update()
	{
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			if (ValidMove(Vector2.down))
			{
				MoveCharacter(xPos, yPos - 1);
			}
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


		else if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject l = GameObject.Instantiate(laser, this.transform.position, Quaternion.identity);
            l.GetComponent<LaserMove>().Initialize(direction);
        }
        
    }

    

    private bool ValidMove(Vector2 direction)
	{
        RaycastHit2D[] hits = Physics2D.RaycastAll(this.transform.position, direction, 1f);
        Debug.DrawRay(transform.position, direction * 1f, Color.red);
        foreach(RaycastHit2D hit in hits)
		{
            if (hit.collider.tag == "Wall" || hit.collider.tag == "Breakable")
            {
                return false;
            }
            else if (hit.collider.tag == "Chip")
            {
                ChipMovement cm = hit.collider.gameObject.GetComponent<ChipMovement>();
                return (cm.TryChipMove(direction));
            }
            else if(hit.collider.tag == "Bot" && hit.collider.gameObject != this.gameObject)
			{
                return false;
			}
            

        }
            
        
        return true;
    }

    private void MoveCharacter(int newX, int newY)
	{
        xPos = newX;
        yPos = newY;
        gameObject.transform.position = new Vector2(xPos + 0.5f, yPos + 0.5f);
	}

    public void InitializeCharacter(int newX, int newY)
	{
        MoveCharacter(newX, newY);
        Debug.Log("Robot Initialized");

    }
}
