using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    private GameObject player;

    private Vector3 offset;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        offset = transform.position - player.transform.position;
        //player = GameObject.FindGameObjectWithTag("Player");
    }

    void LateUpdate()
    {
        //transform.position = player.transform.position + offset;
    }
    void FixedUpdate()
    {
        rb.MovePosition(player.transform.position + offset);
    }
}