using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
        transform.rotation = Camera.main.transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetButtonDown("Fire1"))
        {
            if (Input.GetMouseButtonDown(0))
            {
                GetComponent<SpriteRenderer>().enabled = true;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z + 1f) ;
                    
                    //cube.transform.position = targetPosition;
                }
            }
        }
        else if (Input.GetButtonDown("Fire2"))
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }

    }
}
