using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.IO;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField] string fileName = "testLayout.txt";
    FileInfo file;
    [SerializeField] private int nextLevel;

    private int sizeX = 0;
    private int sizeY = 0;
    [SerializeField] RuleTile tile;
    [SerializeField] Tilemap tileMap;
    List<char[]> world = new List<char[]>();
    [SerializeField] RuleTile circuitTile;
    [SerializeField] Tilemap circuitMap;
    [SerializeField] RuleTile activeTile;

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
    List<BatteryLocation> batteryLocations = new List<BatteryLocation>();

    List<Slot> slots = new List<Slot>();

    List<Door> doors = new List<Door>();

    List<char> jump = new List<char>();
    

    [SerializeField] GameObject robot;
    [SerializeField] GameObject[] chip;
    [SerializeField] GameObject[] slot;
    [SerializeField] GameObject battery;
    [SerializeField] GameObject door;
    [SerializeField] GameObject breakableBlock;
    [SerializeField] GameObject exitPortal;

    CharacterMovement characterMovement;
    ChipMovement chipMovement;
    // Start is called before the first frame update
    void Start()
    {
        file = new FileInfo("Assets\\Layouts\\" + fileName);

        //adding the jumpable characters
        jump.Add('L');
        jump.Add('D');
        jump.Add(')');
        jump.Add(']');
        jump.Add('}');
        jump.Add('>');

        GenerateMain();


    }

    // Update is called once per frame
    void Update()
    {
        CircuitCheckSetup();
    }

    void GenerateMain()
	{
        ReadFile();
        for(int i = 0; i < sizeY; i++)
		{
            for(int j = 0; j < sizeX; j++)
			{
                //world 
                if (world[i][j] == '1')
				{
                    tileMap.SetTile(new Vector3Int(j, -i, 0), tile);
                }
                else if(world[i][j] == 'R')
				{
                    characterMovement = GameObject.Instantiate(robot).GetComponent<CharacterMovement>();
                    characterMovement.InitializeCharacter(j, -i);
                    Debug.Log("robot found at" + j + ", " + i);
				}
                else if(world[i][j] == '(')
				{
                    chipMovement = GameObject.Instantiate(chip[0]).GetComponent<ChipMovement>();
                    chipMovement.InitializeChip(j, -i, 1);
                    Debug.Log("chip found at" + j + ", " + i);
				}
                else if (world[i][j] == '[')
                {
                    chipMovement = GameObject.Instantiate(chip[1]).GetComponent<ChipMovement>();
                    chipMovement.InitializeChip(j, -i, 2);
                    Debug.Log("chip found at" + j + ", " + i);
                }
                else if (world[i][j] == '{')
                {
                    chipMovement = GameObject.Instantiate(chip[2]).GetComponent<ChipMovement>();
                    chipMovement.InitializeChip(j, -i, 3);
                    Debug.Log("chip found at" + j + ", " + i);
                }
                else if (world[i][j] == '<')
                {
                    chipMovement = GameObject.Instantiate(chip[3]).GetComponent<ChipMovement>();
                    chipMovement.InitializeChip(j, -i, 4);
                    Debug.Log("chip found at" + j + ", " + i);
                }
                else if(world[i][j] == 'L')
				{
                    tileMap.SetTile(new Vector3Int(j, -i, 0), tile);
                    circuitMap.SetTile(new Vector3Int(j, -i, 0), circuitTile);

                }
                else if(world[i][j] == 'B')
				{
                    tileMap.SetTile(new Vector3Int(j, -i, 0), tile);
                    GameObject b = GameObject.Instantiate(battery);
                    b.transform.position = new Vector2(j + 0.5f, -i + 0.5f);

                    batteryLocations.Add(new BatteryLocation(j, i));
                }
                else if (world[i][j] == 'D')
                {
                    GameObject d = GameObject.Instantiate(door);
                    d.transform.position = new Vector2(j + 0.5f, -i + 0.5f);
                    d.GetComponent<Door>().initializeDoor(j, i);
                    doors.Add(d.GetComponent<Door>());
                }
                else if (world[i][j] == '2')
                {
                    GameObject b = GameObject.Instantiate(breakableBlock);
                    b.transform.position = new Vector2(j + 0.5f, -i + 0.5f);
                }
                else if (world[i][j] == 'E')
                {
                    GameObject e = GameObject.Instantiate(exitPortal);
                    e.transform.position = new Vector2(j + 0.5f, -i + 0.5f);
                    e.GetComponent<Portal>().Initialize(nextLevel);
                }
                else if (world[i][j] == ')')
				{
                    GameObject s = GameObject.Instantiate(slot[0]);
                    s.transform.position = new Vector2(j + 0.5f, -i + 0.5f);
                    s.GetComponent<Slot>().initializeSlot(j, i, 1);
                    slots.Add(s.GetComponent<Slot>());
                }
                else if (world[i][j] == ']')
                {
                    GameObject s = GameObject.Instantiate(slot[1]);
                    s.transform.position = new Vector2(j + 0.5f, -i + 0.5f);
                    s.GetComponent<Slot>().initializeSlot(j, i, 2);
                    slots.Add(s.GetComponent<Slot>());
                }
                else if (world[i][j] == '}')
                {
                    GameObject s = GameObject.Instantiate(slot[2]);
                    s.transform.position = new Vector2(j + 0.5f, -i + 0.5f);
                    s.GetComponent<Slot>().initializeSlot(j, i, 3);
                    slots.Add(s.GetComponent<Slot>());
                }
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
        for (int i = 0; i < sizeY; i++)
        {
            for (int j = 0; j < sizeX; j++)
            {
                //world 
                if (world[i][j] == 'L')
                {
                    circuitMap.SetTile(new Vector3Int(j, -i, 0), circuitTile);
                }
                else if (world[i][j] == ')' || world[i][j] == ']' || world[i][j] == '}' || world[i][j] == '>')
				{
                    circuitMap.SetTile(new Vector3Int(j, -i, 0), null);
                }

            }
        }
    }

    void ReadFile()
	{
        StreamReader streamReader = file.OpenText();
        sizeX = int.Parse(streamReader.ReadLine());
        sizeY = int.Parse(streamReader.ReadLine());
        for(int i = 0; i < sizeY; i++)
		{
            string line = streamReader.ReadLine();
            world.Add(line.ToCharArray());
		}


    }

    private void CircuitCheckSetup()
	{
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

            if ( d1==d2 && d1 != null && d2 != null )
			{
                d1.Open();

            }
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
            //setting the tiles to be visibly active
            if(world[currY][currX] != ')' && world[currY][currX] != ']' && world[currY][currX] != '}' && world[currY][currX] != '>')
			{
                circuitMap.SetTile(new Vector3Int(currX, -currY, 0), activeTile);
            }
            Debug.Log(currX + ", " + currY + ", " + world[currY][currX]);
            if (prevDirection != 1 && currY < sizeY - 1 && jump.Contains(world[currY + 1][currX]))
            {
                char c = world[currY + 1][currX];
                if (c == ')' || c==']' || c=='}' || c=='>')
				{
                    Debug.Log("HERE, " + c);
                    if (ChipSlotCheck(currY + 1, currX, c))
					{
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
                    prevDirection = 3;
                    currY++;
                }
            }
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
        Debug.Log("We are in the chip slot check");
        foreach(Slot slot in slots)
		{
            
            if(slot.MatchCoords(x, y))
			{
                Debug.Log("Chip is found!");
                return slot.GetIsMatching();
			}
		}
        return false;
	}

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
}
