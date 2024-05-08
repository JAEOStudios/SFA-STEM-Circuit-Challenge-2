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

    //reference to smoke prefab
    [SerializeField] private GameObject smoke;
    [SerializeField] private AudioSource movementSound;
    [SerializeField] private AudioSource donk;

    [SerializeField] private GameObject explosion;

    //tracks animation for portal
    private bool hasPortaled = false;
    private float countDown = 0;

    //health variable
    private int health = 3;

    private WorldGenerator worldGenerator;

    //input variables
    private bool moveUp = true;
    private bool moveDown = true;
    private bool moveLeft = true;
    private bool moveRight = true;
    private bool shoot = true;

    // Start is called before the first frame update

    void Start()
    {
        //setting variables and position of character
        character = this.gameObject;
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        position = new Vector2(gameObject.GetComponent<Transform>().position.x, gameObject.GetComponent<Transform>().position.y);

        //pulling world generator
        worldGenerator = GameObject.Find("Grid").GetComponent<WorldGenerator>();
    }

    // Update is called once per frame
    void Update()
	{
        //process inputs
        if(Input.GetAxisRaw("Horizontal") < 0.1f)
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
        if (!hasPortaled && countDown <= 0)
		{
            //checking inputs and if the movements are valid
            if (Input.GetAxisRaw("Vertical") < -0.5f && moveDown)
            {
                moveDown = false;
                if (ValidMove(Vector2.down))
                {
                    MoveCharacter(xPos, yPos - 1);
                }
                //updating sprites
                spriteRenderer.sprite = directionSprites[0];
                direction = 1;
            }
            else if (Input.GetAxisRaw("Horizontal") > 0.5f && moveRight)
            {
                moveRight = false;
                if (ValidMove(Vector2.right))
                {
                    MoveCharacter(xPos + 1, yPos);
                }
                spriteRenderer.sprite = directionSprites[1];
                direction = 2;
            }
            else if (Input.GetAxisRaw("Vertical") > 0.5f && moveUp)
            {
                moveUp = false;
                if (ValidMove(Vector2.up))
                {
                    MoveCharacter(xPos, yPos + 1);
                }
                spriteRenderer.sprite = directionSprites[2];
                direction = 3;
            }
            else if (Input.GetAxisRaw("Horizontal") < -0.5f && moveLeft)
            {
                moveLeft = false;
                if (ValidMove(Vector2.left))
                {
                    MoveCharacter(xPos - 1, yPos);
                }
                spriteRenderer.sprite = directionSprites[3];
                direction = 4;
            }

            //creating a laser option in the right direction when jump binding is hit
            else if (Input.GetAxisRaw("Jump") > 0 && shoot)
            {
                shoot = false;
                GameObject l = GameObject.Instantiate(laser, this.transform.position, Quaternion.identity);
                l.GetComponent<LaserMove>().Initialize(direction);
            }

            //health setting while game is valid
            health = worldGenerator.GetHealth();

        }
		else if(hasPortaled)
		{
            //animating the player warping into the portal
            this.transform.localScale = new Vector3(this.transform.localScale.x - (0.5f * Time.deltaTime), this.transform.localScale.x - (0.5f * Time.deltaTime), this.transform.localScale.x - (0.5f * Time.deltaTime));
            this.transform.eulerAngles = new Vector3(0, 0, this.transform.eulerAngles.z + 100 * Time.deltaTime);
        }
        
        //things to do if the countdown is on
        if (countDown > 0)
		{
            //decreasing the timer if it's been set
            countDown -= Time.deltaTime * 1;
            
            if (countDown < 0.5f)
            {
                GameObject.Find("Sliders").GetComponent<Animator>().SetTrigger("Close");
            }

            //starting the next level if the countdown is over and the player isn't dead
            if (countDown <= 0 && health > 0)
            {
                GameObject.Find("Grid").GetComponent<WorldGenerator>().startNextLevel();
            }
            //restarting the current one if they are dead
            else if (countDown <= 0)
            {
                GameObject.Find("Grid").GetComponent<WorldGenerator>().restartLevel();
            }
			//else
			//{
            //    Debug.Log("Why am I here?");
			//}
        }

        //checking if the player is out of health or restarting
        if ((health <= 0 || Input.GetButtonDown("Fire2")) && countDown <= 0)
        {
            //setting the countdown and health to 0 to trigger the restart
            countDown = 2f;
            health = 0;

            //creating the explosion and turning off the robot sprite
            this.GetComponent<SpriteRenderer>().enabled = false;
            GameObject.Instantiate(explosion, this.transform.position, this.transform.rotation);
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
                //playing donk
                donk.Play();
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
        //creating the smoke prefab 
        GameObject.Instantiate(smoke, new Vector3(xPos + 0.5f, yPos + 0.5f, 0), Quaternion.identity);
        //playing the sound
        movementSound.Play();
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
            worldGenerator.TakeDamage();
		}
	}

    //warps the player to the portal and changes the portal tracker boolean to true
    public void TouchPortal(Vector2 position)
	{
        //setting the sprite to front facing
        spriteRenderer.sprite = directionSprites[0];

        //setting variable and position
        hasPortaled = true;
        countDown = 2f;
        this.transform.position = position;
	}
}
