using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserMove : MonoBehaviour
{
    private int direction;

    //prefab for sparks on hit
    [SerializeField] private GameObject spark;
    [SerializeField] private GameObject laserSound;
    [SerializeField] private AudioSource doink;
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Instantiate(laserSound, this.transform.position, this.transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        //moving the laser based on direction
        if(direction == 1)
		{
            this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y - 10 * Time.deltaTime);
            this.transform.eulerAngles = new Vector3(0f, 0f, -180f);
        }
        else if(direction == 2)
		{
            this.transform.position = new Vector2(this.transform.position.x + 10 * Time.deltaTime, this.transform.position.y);
            this.transform.eulerAngles = new Vector3(0f, 0f, -90f);
        }
        else if(direction == 3)
		{
            this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + 10 * Time.deltaTime);
        }
		else
		{
            this.transform.position = new Vector2(this.transform.position.x - 10 * Time.deltaTime, this.transform.position.y);
            this.transform.eulerAngles = new Vector3(0f, 0f, 90f);
        }
    }

    public void Initialize(int direction)
	{
        this.direction = direction;
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
        if(!collision.gameObject.CompareTag("Bot") && !collision.gameObject.CompareTag("Untagged"))// && !collision.gameObject.CompareTag("Breakable"))
		{
            GameObject.Instantiate(spark, this.transform.position, this.transform.rotation);
            GameObject.Destroy(this.gameObject);

        }
    }
}
