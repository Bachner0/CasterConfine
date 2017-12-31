using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Make it so you can't select self
//
//I also want a UI box that has name and health bar updated
//  -the name will be grabbed by the photon name for a text box
//  -the health bar will update with a listener on when the target health changes
//



public class MouseManager : Photon.PunBehaviour
{

    public LayerMask selectionMask; //for defining what layers to register selection on
    Camera playerCamera;
    public GameObject selectedObject;   //to store what was selected
    public Renderer selectedObjectRenderer; //stored object renderer
    public PhotonView selectionPhotonView; //stored photonview
    private PhotonView MyPhotonView;
    private PhotonView tempSelectionPhotonView;

    // Use this for initialization
    void Start ()
    {
        playerCamera = Camera.main;
        selectedObject = null;
        selectedObjectRenderer = null;
        MyPhotonView = GetComponent<PhotonView>();
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
                try
                {
                    PhotonView objPhotonView = hitObject.GetComponent<PhotonView>();

                    if (!objPhotonView.isMine)
                    {
                        SelectThisObject(hitObject);
                    }
                }
                catch
                {
                    Debug.Log("Selected the target drudge. He has no PhotonView because I don't know how to get it to work.");
                }
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
        selectedObjectRenderer = obj.GetComponentInChildren<Renderer>();

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
