using UnityEngine;
using System.Collections;

public class CameraLookAt : MonoBehaviour

{

    public Camera cameraToLookAt;

    void Start()
    {
        //transform.rotation = (cameraToLookAt.transform.rotation);
        cameraToLookAt = Camera.main;
    }

    void Update()
    {
        Vector3 v = cameraToLookAt.transform.position - transform.position;
        //v.x = v.z = 0.0f;
        //transform.LookAt(cameraToLookAt.transform.position - v);
        //transform.Rotate(0, 180, 0);
        //transform.position = new Vector3(v.x, transform.position.y, v.z);
        transform.rotation = (cameraToLookAt.transform.rotation);
    }
}
