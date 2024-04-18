using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    float currScroll = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currScroll += 1 * Time.deltaTime;
        this.transform.position = new Vector2(currScroll, currScroll);
        if(currScroll >= 3)
		{
            currScroll = 0;
		}
    }
}
