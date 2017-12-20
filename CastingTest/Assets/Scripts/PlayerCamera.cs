using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {
    //Inspector serialized
    [SerializeField]
    float distanceAway = 6;
    [SerializeField]
    float distanceUp = 2;
    [SerializeField]
    Transform followThisPlayer;

    //smoothing and damping
    Vector3 velocityCamSmooth = Vector3.zero;
    [SerializeField]
    float camSmoothDampTime = 0.1f;

    //private global only
    Vector3 lookDir;
    Vector3 targetPosition;




	void Start ()
    {
        //followThisPlayer = GameObject.FindWithTag("NewPlayer").transform;
        lookDir = followThisPlayer.forward;
	}
	
	void LateUpdate ()
    {
        Vector3 characterOffset = followThisPlayer.position + new Vector3(0f, distanceUp, 0f);

        //Calculate direction from camera to player, kill Y, and normalize to give a valid direction with unit magnitude
        lookDir = characterOffset - this.transform.position;
        lookDir.y = 0;
        lookDir.Normalize();
        Debug.DrawRay(this.transform.position, lookDir, Color.green);

        targetPosition = characterOffset + followThisPlayer.up * distanceUp - lookDir * distanceAway;
        Debug.DrawLine(followThisPlayer.position, targetPosition, Color.magenta);

        CompensateForWalls(characterOffset, ref targetPosition);

        smoothPosition(this.transform.position, targetPosition);

        transform.LookAt(characterOffset);
	}

    private void smoothPosition(Vector3 fromPos, Vector3 toPos)
    {
        //Making a smooth transition between camera's current position and the position it wants to be in
        this.transform.position = Vector3.SmoothDamp(fromPos, toPos, ref velocityCamSmooth, camSmoothDampTime);
    }

    private void CompensateForWalls(Vector3 fromObject, ref Vector3 toTarget)
    {
        Debug.DrawLine(fromObject, toTarget, Color.cyan);
        //Compensate for walls between camera
        RaycastHit wallHit = new RaycastHit();
        if(Physics.Linecast(fromObject, toTarget, out wallHit))
        {
            Debug.DrawRay(wallHit.point, Vector3.left, Color.red);
            toTarget = new Vector3(wallHit.point.x, toTarget.y, wallHit.point.z);
        }
    }
}


