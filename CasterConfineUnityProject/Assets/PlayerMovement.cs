using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider))]
//do I need other required components?

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody playerRigidBody;           // add the rigid body to this field
    public BoxCollider playerBoxCollider;       // add the BoxCollider to this field
    public GameObject player;                   // add the player asset to this field
    public Transform target;                    // from old script... for player selecting a target. does it go here?


    // Use this for initialization
    void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
