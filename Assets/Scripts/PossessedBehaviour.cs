using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PossessedBehaviour : MonoBehaviour {

    private bool poss;
    private GameObject player;
    private Vector3 target;
    private float Distance;
    private Rigidbody rb;
    private GameObject blob;
    private Renderer rend;
    private Image marker;
    private Behaviour halo; 

    // Use this for initialization
    void Start () {

        poss = false;
        target = transform.position;
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        rend = GetComponent<Renderer>();
        marker = GetComponentInChildren<Image>();
        halo = (Behaviour)GetComponent("Halo");

        halo.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {


    }
    void FixedUpdate()
    {

        if (poss && ScoreBehavior.currentBlob == blob.GetComponent<BlobController>().blobNumber)
        {
            marker.enabled = true;
            if (rend.material.color == Color.green)
            {
                greenLogic();
            }
            else if (rend.material.color == new Color(0.3F, 0.1F, 0.6F, 0.5F))
            {
                purpleLogic();
            }
        } else
        {
            marker.enabled = false;
            target = transform.position;
            moveTo();
        }

        

    }

    void lookAt()
    {
        // Rotate to look at player.
        Vector3 mousePos = Vector3.zero;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            mousePos = hit.point;

            //cube.transform.position = targetPosition;
        }
        Quaternion rotation = Quaternion.LookRotation((mousePos) - transform.position);
        rotation.z = 0;
        rotation.x = 0;

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5.0f);
        Distance = Vector3.Distance(target, transform.position);
      
        //transform.LookAt(Target); alternate way to track player replaces both lines above.
    }
    void moveTo()
    {
        if (Distance > 0.5f)
        {
            Vector3 temp = transform.forward;
            temp.y = 0;
            //transform.position += temp * 5.0f * Time.deltaTime;
            rb.MovePosition( transform.position + (target-transform.position).normalized * Time.deltaTime * (2.5f + (100/rb.mass)));
            
        }
        if (!poss)
        {
            target = transform.position;
        }
    }

    private void greenLogic()
    {

        if (Input.GetButtonDown("Fire1"))
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    target = hit.point;

                    //cube.transform.position = targetPosition;
                }
            }
        }
        else if (Input.GetButtonDown("Fire2"))
        {

            blob.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
            blob.GetComponentInChildren<Rigidbody>().isKinematic = false;
            blob.GetComponentInChildren<Rigidbody>().useGravity = true;
            blob.transform.position = (transform.position + ((player.transform.position - transform.position).normalized * 2.0f));
            rend.material.color = Color.white;
            transform.position = Vector3.MoveTowards(transform.position, transform.position, 10f);
            poss = false;

        }
        lookAt();
        moveTo();
    }

    private void purpleLogic()
    {
        if (Input.GetButton("Fire1"))
        {
            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.point.z > transform.position.z)
                    {
                        transform.localScale += new Vector3(0.01F, 0.01F, 0.01F);
                        rb.mass += 0.15f;

                    }
                    else
                    {
                        if (transform.localScale.x > 0.2f)
                        {
                            transform.localScale -= new Vector3(0.01F, 0.01F, 0.01F);
                            rb.mass -= 0.15f;
                        }
                    }
                }
            }
        }
        else if (Input.GetButtonDown("Fire2"))
        {

            blob.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
            blob.GetComponentInChildren<Rigidbody>().isKinematic = false;
            blob.GetComponentInChildren<Rigidbody>().useGravity = true;
            blob.transform.position = (transform.position + ((player.transform.position - transform.position).normalized * 2.0f));
            rend.material.color = Color.white;
            transform.position = Vector3.MoveTowards(transform.position, transform.position, 10f);
            poss = false;

        }
        lookAt();
        moveTo();
    }


    void OnTriggerEnter(Collider other)
    {
       if (other.gameObject.tag == "blob" && !poss)
        {
            blob = other.gameObject;
            if (!blob.GetComponent<BlobController>().following)
            {
               
                other.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
                other.gameObject.GetComponentInChildren<Rigidbody>().isKinematic = true;
                other.gameObject.GetComponentInChildren<Rigidbody>().useGravity = false;
                blob.transform.position = new Vector3(0, -5, 0);
                rend.material.color = blob.GetComponent<BlobController>().chosenColor;

                poss = true; 
            }
        }
    }

    private void OnMouseEnter()
    {
        halo.enabled = true;

    }

    private void OnMouseExit()
    {
        halo.enabled = false;
    }
}
