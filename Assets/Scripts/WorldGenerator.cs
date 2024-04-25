using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using TMPro;

public class WorldGenerator : MonoBehaviour
{
    //info about the files and file loading
    private string fileName = "testLayout.txt";
    LevelManager levelManager;
    private FileInfo file;

    //size parameters populated from file
    private int sizeX = 0;
    private int sizeY = 0;

    //serialized objects of tiles to populate
    [SerializeField] private RuleTile tile;
    [SerializeField] private Tilemap tileMap;
    private List<char[]> world = new List<char[]>();
    [SerializeField] private RuleTile circuitTile;
    [SerializeField] private Tilemap circuitMap;
    [SerializeField] private RuleTile activeTile;

    //struct for battery location
    public struct BatteryLocation
	{
        public int x;
        public int y;
        public BatteryLocation(int x, int y)
		{
            this.x = x;
            this.y = y;
		}
	}
    //lists of batteries, slots, and doors
    private List<BatteryLocation> batteryLocations = new List<BatteryLocation>();

    private List<Slot> slots = new List<Slot>();

    private List<Door> doors = new List<Door>();

    //jumpable characters for the wires
    private List<char> jump = new List<char>();
    
    //gameojbect prefabs for instantiation
    [SerializeField] private GameObject robot;
    [SerializeField] private GameObject[] chip;
    [SerializeField] private GameObject[] slot;
    [SerializeField] private GameObject battery;
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject breakableBlock;
    [SerializeField] private GameObject exitPortal;
    [SerializeField] private GameObject hazard;

    //references to often used character and chip movement classes
    private CharacterMovement characterMovement;
    private ChipMovement chipMovement;

    //health variables
    private int health = 3;
    [SerializeField] private GameObject[] hearts = new GameObject[3];

    // Start is called before the first frame update
    void Start()
    {
        //pulling level manager
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        fileName = levelManager.GetCurrentLevel();
        file = new FileInfo(Application.dataPath+ " /Levels/" + fileName);

        //adding the jumpable characters
        jump.Add('W');
        jump.Add('w');
        jump.Add('D');
        jump.Add(')');
        jump.Add(']');
        jump.Add('}');
        jump.Add('>');

        GenerateMain();

        //pulling the hearts
        hearts = GameObject.FindGameObjectsWithTag("Heart");

    }

    // Update is called once per frame
    void Update()
    {
        //validating circuit completion repeatedly
        CircuitCheckSetup();
    }

    void GenerateMain()
	{
        //initializing contents of file for usage
        ReadFile();

        
        for(int i = 0; i < sizeY; i++)
		{
            for(int j = 0; j < sizeX; j++)
			{
                //building a wall
                if (world[i][j] == '1')
				{
                    tileMap.SetTile(new Vector3Int(j, -i, 0), tile);
                }
                //building a robot
                else if(world[i][j] == 'R')
				{
                    characterMovement = GameObject.Instantiate(robot).GetComponent<CharacterMovement>();
                    characterMovement.InitializeCharacter(j, -i);
                    Debug.Log("robot found at" + j + ", " + i);
				}
                //chip 1
                else if(world[i][j] == '(')
				{
                    chipMovement = GameObject.Instantiate(chip[0]).GetComponent<ChipMovement>();
                    chipMovement.InitializeChip(j, -i, 1);
                    Debug.Log("chip found at" + j + ", " + i);
				}
                //chip 2
                else if (world[i][j] == '[')
                {
                    chipMovement = GameObject.Instantiate(chip[1]).GetComponent<ChipMovement>();
                    chipMovement.InitializeChip(j, -i, 2);
                    Debug.Log("chip found at" + j + ", " + i);
                }
                //chip 3
                else if (world[i][j] == '{')
                {
                    chipMovement = GameObject.Instantiate(chip[2]).GetComponent<ChipMovement>();
                    chipMovement.InitializeChip(j, -i, 3);
                    Debug.Log("chip found at" + j + ", " + i);
                }
                //chip 4
                else if (world[i][j] == '<')
                {
                    chipMovement = GameObject.Instantiate(chip[3]).GetComponent<ChipMovement>();
                    chipMovement.InitializeChip(j, -i, 4);
                    Debug.Log("chip found at" + j + ", " + i);
                }
                //wires w wall
                else if(world[i][j] == 'W')
				{
                    tileMap.SetTile(new Vector3Int(j, -i, 0), tile);
                    circuitMap.SetTile(new Vector3Int(j, -i, 0), circuitTile);

                }
                //wires w/o wall
                else if (world[i][j] == 'w')
                {
                    circuitMap.SetTile(new Vector3Int(j, -i, 0), circuitTile);

                }
                //battery
                else if(world[i][j] == 'B')
				{
                    tileMap.SetTile(new Vector3Int(j, -i, 0), tile);
                    GameObject b = GameObject.Instantiate(battery);
                    b.transform.position = new Vector2(j + 0.5f, -i + 0.5f);
                    //adding battery location to list of structs
                    batteryLocations.Add(new BatteryLocation(j, i));
                }
                //door
                else if (world[i][j] == 'D')
                {
                    GameObject d = GameObject.Instantiate(door);
                    d.transform.position = new Vector2(j + 0.5f, -i + 0.5f);
                    d.GetComponent<Door>().initializeDoor(j, i);
                    //adding door to list of doors
                    doors.Add(d.GetComponent<Door>());
                }
                //breakable walls
                else if (world[i][j] == '2')
                {
                    GameObject b = GameObject.Instantiate(breakableBlock);
                    b.transform.position = new Vector2(j + 0.5f, -i + 0.5f);
                }
                //portal for level
                else if (world[i][j] == 'E')
                {
                    GameObject e = GameObject.Instantiate(exitPortal);
                    e.transform.position = new Vector2(j + 0.5f, -i + 0.5f);
                }
                else if (world[i][j] == 'X')
                {
                    GameObject x = GameObject.Instantiate(hazard);
                    x.transform.position = new Vector2(j + 0.5f, -i + 0.5f);
                }
                //chip slot 1
                else if (world[i][j] == ')')
				{
                    GameObject s = GameObject.Instantiate(slot[0]);
                    s.transform.position = new Vector2(j + 0.5f, -i + 0.5f);
                    s.GetComponent<Slot>().initializeSlot(j, i, 1);
                    slots.Add(s.GetComponent<Slot>());
                }
                //chip slot 2
                else if (world[i][j] == ']')
                {
                    GameObject s = GameObject.Instantiate(slot[1]);
                    s.transform.position = new Vector2(j + 0.5f, -i + 0.5f);
                    s.GetComponent<Slot>().initializeSlot(j, i, 2);
                    slots.Add(s.GetComponent<Slot>());
                }
                //chip slot 3
                else if (world[i][j] == '}')
                {
                    GameObject s = GameObject.Instantiate(slot[2]);
                    s.transform.position = new Vector2(j + 0.5f, -i + 0.5f);
                    s.GetComponent<Slot>().initializeSlot(j, i, 3);
                    slots.Add(s.GetComponent<Slot>());
                }
                //chip slot 4
                else if (world[i][j] == '>')
                {
                    GameObject s = GameObject.Instantiate(slot[3]);
                    s.transform.position = new Vector2(j + 0.5f, -i + 0.5f);
                    s.GetComponent<Slot>().initializeSlot(j, i, 4);
                    slots.Add(s.GetComponent<Slot>());
                }

            }
		}
        

    }

    private void RegenDeadCircuit()
	{
        //setting all circuit tiles to off to ensure that when they get unmatched they turn off
        for (int i = 0; i < sizeY; i++)
        {
            for (int j = 0; j < sizeX; j++)
            {
                //world 
                if (world[i][j] == 'W' || world[i][j] == 'w')
                {
                    circuitMap.SetTile(new Vector3Int(j, -i, 0), circuitTile);
                }
                //ensuring chip slots don't have circuits visible
                else if (world[i][j] == ')' || world[i][j] == ']' || world[i][j] == '}' || world[i][j] == '>')
				{
                    circuitMap.SetTile(new Vector3Int(j, -i, 0), null);
                }

            }
        }
    }

    private void ReadFile()
	{
        if(file.Exists)
		{
            //opening the file
            StreamReader streamReader = file.OpenText();

            //reading the x and y coords
            sizeX = int.Parse(streamReader.ReadLine());
            sizeY = int.Parse(streamReader.ReadLine());

            SetCamera();

            //assigning the level name object to have the text
            GameObject.Find("LevelName").GetComponent<TextMeshProUGUI>().text = streamReader.ReadLine();

            //reading in the file line by line into the array
            for (int i = 0; i < sizeY; i++)
            {
                string line = streamReader.ReadLine();
                world.Add(line.ToCharArray());
            }
            //reading in the next level from the file 
            if (!streamReader.EndOfStream)
            {
                levelManager.UpdateNextLevel(streamReader.ReadLine());
            }
            else
            {
                //if a level isn't provided, change to ls for level select indicator to be read in startNextLevel()
                levelManager.UpdateNextLevel("ls");
            }
        }
        

    }

    //changes camera size based on level size
    private void SetCamera()
	{
        Camera cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        cam.transform.position = new Vector3(sizeX / 2f, 1 - sizeY / 2f, -10f);
        cam.orthographicSize = Mathf.Max(sizeX/3.8f, sizeY/2);
	}

    //reloading the level with no changes
    public void restartLevel()
	{
        SceneManager.LoadScene("Level");
	}

    public void startNextLevel()
	{
        //getting the next level
        levelManager.UpdateCurrentLevel(levelManager.GetNextLevel());

        //if it doesn't exist, delete the level manager and load the level select
        if(levelManager.GetCurrentLevel() == "ls" || levelManager.GetNextLevel() == "ls")
		{
            SceneManager.LoadScene("Victory");
		}
		else
		{
            //if it does, reload current level to start new level
            SceneManager.LoadScene("Level");
        }
        
    }

    private void CircuitCheckSetup()
	{
        //ensuring the circuit is reset 
        RegenDeadCircuit();
        for(int i = 0; i < batteryLocations.Count; i++)
		{
            //setting up the up direction check
            int currY = batteryLocations[i].y - 1;
            int currX = batteryLocations[i].x;

            //1 = down, 2 = right, 3 = up, 4 = left
            int prevDirection = 1;

            Door d1 = CircuitCheck(currY, currX, prevDirection);

            //setting up the down direction check
            currY = batteryLocations[i].y + 1;
            currX = batteryLocations[i].x;

            //1 = down, 2 = right, 3 = up, 4 = left
            prevDirection = 3;

            Door d2 = CircuitCheck(currY, currX, prevDirection);

            //if both ends of the cirucuits are complete, open the door
            if ( d1==d2 && d1 != null && d2 != null )
			{
                d1.Open();

            }
            //else, close the door
            else if (d1 != null)
			{
                d1.Close();
                
			}
            else if(d2 != null)
			{
                d2.Close();
            }

        }
    }

	private Door CircuitCheck(int currY, int currX, int prevDirection)
	{
        while (world[currY][currX] != 'D')
        {
            //setting the tiles to be visibly active if they aren't chip slots
            if(world[currY][currX] != ')' && world[currY][currX] != ']' && world[currY][currX] != '}' && world[currY][currX] != '>')
			{
                circuitMap.SetTile(new Vector3Int(currX, -currY, 0), activeTile);
            }
            Debug.Log(currX + ", " + currY + ", " + world[currY][currX]);
            //checking the down direction if it wasn't used last, if the screen isn't at the bottom, and if the tile is a jumpable tile
            if (prevDirection != 1 && currY < sizeY - 1 && jump.Contains(world[currY + 1][currX]))
            {
                //if it's a chip slot...
                char c = world[currY + 1][currX];
                if (c == ')' || c==']' || c=='}' || c=='>')
				{
                    Debug.Log("HERE, " + c);
                    //check the chip slot
                    if (ChipSlotCheck(currY + 1, currX, c))
					{
                        //setting direction and updating position
                        prevDirection = 3;
                        currY++;
                    }
					else
					{
                        Debug.Log("Door not found!");
                        return null;
					}
				}
				else
				{
                    //setting direction and updating position
                    prevDirection = 3;
                    currY++;
                }
            }
            //checking the right direction if it wasn't used last, if the screen isn't at the right, and if the tile is a jumpable tile
            else if (prevDirection != 2 && currX < sizeX - 1 && jump.Contains(world[currY][currX + 1]))
            {
                char c = world[currY][currX+1];
                if (c == ')' || c == ']' || c == '}' || c == '>')
                {
                    Debug.Log("HERE, " + c);
                    if (ChipSlotCheck(currY, currX + 1, c))
                    {
                        prevDirection = 4;
                        currX++;
                    }
                    else
                    {
                        Debug.Log("Door not found!");
                        return null;
                    }
                }
                else
                {
                    prevDirection = 4;
                    currX++;
                }
            }
            //checking the up direction if it wasn't used last, if the screen isn't at the top, and if the tile is a jumpable tile
            else if (prevDirection != 3 && currY > 0 && jump.Contains(world[currY - 1][currX]))
            {
                char c = world[currY - 1][currX];
                if (c == ')' || c == ']' || c == '}' || c == '>')
                {
                    Debug.Log("HERE, " + c);
                    if (ChipSlotCheck(currY - 1, currX, c))
                    {
                        prevDirection = 1;
                        currY--;
                    }
					else
					{
                        Debug.Log("Door not found!");
                        return null;
					}
                }
                else
                {
                    prevDirection = 1;
                    currY--;
                }
            }
            //checking the left direction if it wasn't used last, if the screen isn't at the left, and if the tile is a jumpable tile
            else if (prevDirection != 4 && currX > 0 && jump.Contains(world[currY][currX - 1]))
            {
                char c = world[currY][currX-1];
                if (c == ')' || c == ']' || c == '}' || c == '>')
                {
                    Debug.Log("HERE, " + c);
                    if (ChipSlotCheck(currY, currX-1, c))
                    {
                        prevDirection = 2;
                        currX--;
                    }
					else
					{
                        Debug.Log("Door not found!");
                        return null;
					}
                }
                else
                {
                    prevDirection = 2;
                    currX--;
                }
            }
			else
			{
                Debug.Log("Door not found!");
                return null;
			}

        }
        Debug.Log("Door found!");
        return DoorCheck(currY, currX);
    }

    private bool ChipSlotCheck(int y, int x, char c)
	{
        //for each chip slot, check if the corresponding chip is overlapping it
        Debug.Log("We are in the chip slot check");
        foreach(Slot slot in slots)
		{
            //calling the chip slot's match method
            if(slot.MatchCoords(x, y))
			{
                Debug.Log("Chip is found!");
                return slot.GetIsMatching();
			}
		}
        return false;
	}

    //checking if the door matches the current coords to open
    private Door DoorCheck(int y, int x)
	{
        foreach(Door door in doors)
		{
            if(door.MatchCoords(x,y))
			{
                return door;
			}
		}
        return null;
	}

    public void TakeDamage()
	{
        health--;
        if(health >= 0)
		{
            hearts[health].SetActive(false);
        }
	}

    public int GetHealth()
	{
        return this.health;
	}
}
