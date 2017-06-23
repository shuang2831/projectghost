using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FlagBehaviour : MonoBehaviour {

    private GameObject player;
    private GameObject[] blobs;

    private bool playerIn;
    private bool[] blobsIn;

    public bool Complete;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        blobs = GameObject.FindGameObjectsWithTag("blob");
        playerIn = false;
        blobsIn = new bool[blobs.Length];

        for (int i = 0; i < blobsIn.Length; i++)
        {
            blobsIn[i] = false;
        }

        Complete = false;

	}
	
	// Update is called once per frame
	void Update () {
        if (blobsIn.All(x => x) && playerIn && Complete == false)
        {
            Complete = true;
        }

        

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player)
        {
            playerIn = true;
        }
        if (other.gameObject.tag == "blob")
        {
            blobsIn[other.gameObject.GetComponent<BlobController>().blobNumber - 1] = true;
        }
    }
}
