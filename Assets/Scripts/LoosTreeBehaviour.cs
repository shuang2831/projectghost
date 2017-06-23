using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoosTreeBehaviour : MonoBehaviour {

    private Vector3 startPosition;
    private Quaternion startRotation;

	// Use this for initialization
	void Start () {
        startPosition = transform.position;
        startRotation = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y < -10)
        {
            transform.rotation = startRotation;
            transform.position = startPosition;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
	}
}
