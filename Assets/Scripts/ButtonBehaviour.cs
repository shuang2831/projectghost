using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBehaviour : MonoBehaviour {

    public GameObject connectedItem; 
	// Use this for initialization
	void Start () {
        Physics.IgnoreCollision(GetComponent<BoxCollider>(), GameObject.FindGameObjectWithTag("ground").GetComponent<BoxCollider>());
        Physics.IgnoreCollision(GetComponent<CapsuleCollider>(), GameObject.FindGameObjectWithTag("ground").GetComponent<BoxCollider>());
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "ground")
        {
            if (other.gameObject.GetComponent<Rigidbody>().mass > 0.9)
            {
                transform.Translate(new Vector3(0, -0.2f, 0));
                connectedItem.SendMessage("Activate");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "ground")
        {
            if (other.gameObject.GetComponent<Rigidbody>().mass > 0.9)
            {
                //transform.localPosition = new Vector3(0, -0.1f, 0);
                transform.Translate(new Vector3(0, 0.2f, 0));
                connectedItem.SendMessage("Close");
            }
        }
    }
}
