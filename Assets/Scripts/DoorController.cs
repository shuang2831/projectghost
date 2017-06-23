using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour {

    bool open;
    Vector3 startPosition;
    float doorTimer;
	// Use this for initialization
	void Start () {
        open = false;
        startPosition = transform.position;
        doorTimer = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate()
    {
        if (open)
        {
            if (doorTimer <= 1) {
                doorTimer += Time.deltaTime * 3.0f;
                transform.position = Vector3.Lerp(startPosition, new Vector3(startPosition.x, -5f, startPosition.z), doorTimer);
                
            }
        }
        else
        {
            if (doorTimer >= 0)
            {
                doorTimer -= Time.deltaTime * 3.0f;
                transform.position = Vector3.Lerp(startPosition, new Vector3(startPosition.x, -5f, startPosition.z), doorTimer);
               
            }
        }
    }

    void Activate()
    {
        if (!open)
        {
            //transform.Translate(new Vector3(0, -10, 0));
            //transform.position = Vector3.Lerp(startPosition, new Vector3(startPosition.x, -10, startPosition.z, Time.deltaTime * 5.0f);
            open = true;
        }
    }

    void Close()
    {
        if (open)
        {
            //transform.Translate(new Vector3(0, 10, 0));
            //  transform.position = startPosition;
            open = false;
            //doorTimer = 0;
        }
    }
}
