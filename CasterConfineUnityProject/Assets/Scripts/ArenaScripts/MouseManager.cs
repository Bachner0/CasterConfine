using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour {

    public LayerMask selectionMask; //for defining what layers to register selection on
    Camera playerCamera;
    public GameObject selectedObject;   //to store what was selected

    // Use this for initialization
    void Start ()
    {
        playerCamera = Camera.main;
        selectedObject = null;
	}
	
	// Update is called once per frame
	void Update ()
    {
        GetInputAndRayCast();
    }

    void GetInputAndRayCast()
    {
        //Clicking and ray cast to see what was clicked on
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // check to see if we hit the interactable layer (will only be other players in this game)
            if (Physics.Raycast(ray, out hit, 50, selectionMask))  //50 is a distance limit
            {
                Debug.Log("We hit " + hit.collider.name + " " + hit.point);

                GameObject hitObject = hit.transform.root.gameObject;

                SelectThisObject(hitObject);
            }
            else
            {
                //ClearSelection();
                //we don't really want to clear selection here. 


                //what if it is a UI element being clicked like skill or escape menu?
                //HANDLE THAT HERE


            }

        }
    }


    void SelectThisObject(GameObject obj)
    {
        if (selectedObject != null)
        {
            if (obj == selectedObject)
                return;

            ClearSelection();
        }
        selectedObject = obj;
    }

    //If we wanted to deselect in some way. maybe use this when gameobject (player) is destroyed?
    void ClearSelection()
    {
        if (selectedObject == null)
        {
            return;
        }
        else
        {
            selectedObject = null;
        }
    }
}
