using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointBehaviour : MonoBehaviour {

    private bool visited;


	// Use this for initialization
	void Start () {
        visited = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !visited)
        {
           visited = true;
            other.gameObject.SendMessage("setCheckpoint");
            transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.red;
        }

    }
}
