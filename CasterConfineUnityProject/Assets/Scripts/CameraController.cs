using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform targetToFollow;

    public Vector3 offsetFromPlayer;

    //zoom variables
    public float zoomSpeed = 4f;
    public float minZoom = 2f;
    public float maxZoom = 15f;
    private float currentZoom = 10f;

    public float pitch = 2f;

    //NEED camera rotation

    // Use this for initialization
    void Start()
    {

    }

    void Update()
    {
        currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed; //registers what the scroll wheel does
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);   //use the min and max by clamping

        //NEED camera rotation when holding right mouse button down

    }

	// Update is called once per frame - Late means it called right after the frame
	void LateUpdate ()
    {
        //follow target at specified place
        transform.position = targetToFollow.position - offsetFromPlayer * currentZoom;

        //look at target location and target height
        transform.LookAt(targetToFollow.position + Vector3.up * pitch);

        //NEED camera rotation
	}
}
