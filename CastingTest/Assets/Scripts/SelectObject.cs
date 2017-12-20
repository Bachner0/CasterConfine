using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectObject : MonoBehaviour
{

    public LayerMask selectionMask; //for defining what layers to register selection on
    Camera playerCamera;

    // Use this for initialization
    void Start()
    {
        playerCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {


        //Clicking and ray cast to see what was clicked on
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            //
            if (Physics.Raycast(ray, out hit, 50, selectionMask))  //50 is a distance limit
            {
                Debug.Log("We hit " + hit.collider.name + " " + hit.point);
                //(whatever is in here will be executed)

                // check to see if we hit an interactable
                //stop focusing any objects and focus on this one, maybe throw a graphic up


            }

            //if the raycast doesn't hit, then unfocus. Or maybe make another layermask and if it is hit, it unfocuses
        }

    }
}
