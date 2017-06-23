using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
        transform.GetChild(0).gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

}

