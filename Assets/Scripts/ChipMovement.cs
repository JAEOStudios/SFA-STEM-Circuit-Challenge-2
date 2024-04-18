using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipMovement : MonoBehaviour
{
    GameObject chip;
    Vector2 position;
    int xPos = 0;
    int yPos = 0;

    int id = 0;
    // Start is called before the first frame update

    void Start()
    {
        chip = this.gameObject;
        position = new Vector2(gameObject.GetComponent<Transform>().position.x, gameObject.GetComponent<Transform>().position.y);
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    public bool TryChipMove(Vector2 direction)
	{
        if (direction == Vector2.down)
        {
            if (ValidMove(Vector2.down))
            {
                MoveChip(xPos, yPos - 1);
                return true;
            }
        }
        else if (direction == Vector2.right)
        {
            if (ValidMove(Vector2.right))
            {
                MoveChip(xPos+1, yPos);
                return true;
            }
        }
        else if (direction == Vector2.up)
        {
            if (ValidMove(Vector2.up))
            {
                MoveChip(xPos, yPos + 1);
                return true;
            }
        }

        else if (direction == Vector2.left)
        {
            if (ValidMove(Vector2.left))
            {
                MoveChip(xPos - 1, yPos);
                return true;
            }
        }
        return false;
    }

    private bool ValidMove(Vector2 direction)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(this.transform.position, direction, 1f);
        Debug.DrawRay(transform.position, direction * 1f, Color.red);
        foreach (RaycastHit2D hit in hits)
            if (hit.collider.tag == "Wall" || hit.collider.tag == "Breakable")
            {
                return false;
            }
            else if (hit.collider.gameObject != this.gameObject && hit.collider.tag == "Chip")
            {
                ChipMovement cm = hit.collider.gameObject.GetComponent<ChipMovement>();
                return (cm.TryChipMove(direction));
            }
            else if (hit.collider.tag == "Bot")
            {
                return false;
            }
        return true;
    }

    private void MoveChip(int newX, int newY)
    {
        xPos = newX;
        yPos = newY;
        gameObject.transform.position = new Vector2(xPos + 0.5f, yPos + 0.5f);
    }

    public void InitializeChip(int newX, int newY, int id)
    {
        this.id = id;
        MoveChip(newX, newY);
        Debug.Log("Chip Initialized");

    }

    public int GetChipID()
	{
        return id;
	}
}
