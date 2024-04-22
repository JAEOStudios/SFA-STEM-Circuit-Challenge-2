using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDestroy : MonoBehaviour
{
    //timer to delete this object after
    [SerializeField] private float timer;


    //cecreasing timer and deleting object if it's 0
    void Update()
    {
        timer -= Time.deltaTime * 1;
        if(timer < 0)
		{
            GameObject.Destroy(this.gameObject);
		}
    }
}
