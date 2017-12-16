using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    static public Transform target;
    public float distance = 4.0f;
    public float height = 1.0f;
    public float smoothLag = 0.2f;
    public float maxSpeed = 10.0f;
    public float snapLag = 0.3f;
    public float clampHeadPositionScreenSpace = 0.75f;
    LayerMask lineOfSightMask = 0;
    Vector3 headOffset = Vector3.zero;
    Vector3 centerOffset = Vector3.zero;

    //bool isSnapping = false;
    Vector3 velocity = Vector3.zero;
    float targetHeight = 100000.0f;

    //void Apply(Transform dummyTarget, Vector3 dummyCenter)
    //{
    //    Vector3 targetCenter = target.position + centerOffset;
    //    Vector3 targetHead = target.position + headOffset;

    //    targetHeight = targetCenter.y + height;

    //    if (Input.GetButton("Fire2") && !isSnapping)
    //    {
    //        velocity = Vector3.zero;
    //        isSnapping = true;
    //    }

    //    if (isSnapping)
    //    {
    //        ApplySnapping(targetCenter);
    //    }
    //    else
    //    {
    //        ApplyPositionDamping(new Vector3(targetCenter.x, targetHeight, targetCenter.z));
    //    }

    //    SetUpRotation(targetCenter, targetHead);
    //}

    // Update is called once per frame
    //void LateUpdate()
    //{
    //    if (target)
    //        Apply(null, Vector3.zero);
    //}

    //void ApplySnapping(Vector3 targetCenter)
    //{
    //    Vector3 position = transform.position;
    //    Vector3 offset = position - targetCenter;
    //    offset.y = 0;
    //    float currentDistance = offset.magnitude;

    //    float targetAngle = target.eulerAngles.y;
    //    float currentAngle = transform.eulerAngles.y;

    //    currentAngle = Mathf.SmoothDampAngle(currentAngle, targetAngle, ref velocity.x, snapLag);
    //    currentDistance = Mathf.SmoothDamp(currentDistance, distance, ref velocity.z, snapLag);

    //    Vector3 newPosition = targetCenter;
    //    newPosition += Quaternion.Euler(0, currentAngle, 0) * Vector3.back * currentDistance;

    //    newPosition.y = Mathf.SmoothDamp(position.y, targetCenter.y + height, ref velocity.y, smoothLag, maxSpeed);

    //    newPosition = AdjustLineOfSight(newPosition, targetCenter);

    //    transform.position = newPosition;

    //    if (AngleDistance(currentAngle, targetAngle) < 3.0)
    //    {
    //        isSnapping = false;
    //        velocity = Vector3.zero;
    //    }
    //}

    Vector3 AdjustLineOfSight(Vector3 newPosition, Vector3 target)
    {
        RaycastHit hit;
        if (Physics.Linecast(target, newPosition, out hit, lineOfSightMask.value))
        {
            velocity = Vector3.zero;
            return hit.point;
        }

        return newPosition;
    }

    void ApplyPositionDamping(Vector3 targetCenter)
    {
        Vector3 position = transform.position;
        Vector3 offset = position - targetCenter;
        offset.y = 0;
        Vector3 newTargetPos = offset.normalized * distance + targetCenter;

        Vector3 newPosition;
        newPosition.x = Mathf.SmoothDamp(position.x, newTargetPos.x, ref velocity.x, smoothLag, maxSpeed);
        newPosition.z = Mathf.SmoothDamp(position.z, newTargetPos.z, ref velocity.z, smoothLag, maxSpeed);
        newPosition.y = Mathf.SmoothDamp(position.y, targetCenter.y, ref velocity.y, smoothLag, maxSpeed);

        newPosition = AdjustLineOfSight(newPosition, targetCenter);

        transform.position = newPosition;
    }

    void SetUpRotation(Vector3 centerPos, Vector3 headPos)
    {
        Vector3 cameraPos = transform.position;
        Vector3 offsetToCenter = centerPos - cameraPos;

        //Generate base rotation only around y-axis
        Quaternion yRotation = Quaternion.LookRotation(new Vector3(offsetToCenter.x, 0.0f, offsetToCenter.z));

        Vector3 relativeOffset = Vector3.forward * distance + Vector3.down * height;
        transform.rotation = yRotation * Quaternion.LookRotation(relativeOffset);

        //Calculate the projected center position and top position in world space
        Ray centerRay = this.GetComponent<Camera>().ViewportPointToRay(new Vector3(.5f, 0.5f, 1f));
        Ray topRay = this.GetComponent<Camera>().ViewportPointToRay(new Vector3(.5f, clampHeadPositionScreenSpace, 1.0f));

        Vector3 centerRayPos = centerRay.GetPoint(distance);
        Vector3 topRayPos = topRay.GetPoint(distance);

        float centerToTopAngle = Vector3.Angle(centerRay.direction, topRay.direction);

        float heightToAngle = centerToTopAngle / (centerRayPos.y - topRayPos.y);

        float extraLookAngle = heightToAngle * (centerRayPos.y - centerPos.y);
        if (extraLookAngle < centerToTopAngle)
        {
            extraLookAngle = 0;
        }
        else
        {
            extraLookAngle = extraLookAngle - centerToTopAngle;
            transform.rotation *= Quaternion.Euler(-extraLookAngle, 0, 0);
        }
    }

    float AngleDistance(float a, float b)
    {
        a = Mathf.Repeat(a, 360);
        b = Mathf.Repeat(b, 360);

        return Mathf.Abs(b - a);
    }

    Vector3 GetCenterOffset()
    {
        return centerOffset;
    }

    void SetTarget(Transform t)
    {
        target = t;
    }


}

    /* from https://youtu.be/HFP8dvDXPyk?list=PL1bPKmY0c-wnvW9F4BDJMwDHmq2SQ8m3L
     * 
    public bool holdCamera;
    public bool addDefaultAsNormal;
    public Transform target;

    #region Variables
    public string activeStateID;
    [SerializeField]
    float moveSpeed = 5;
    [SerializeField]
    float turnSpeed = 1.5f;
    [SerializeField]
    float turnSpeedController = 5.5f;
    [SerializeField]
    float turnSmoothing = .1f;
    [SerializeField]
    bool isController;
    public bool lockCursor;
    #endregion

    #region References
    [HideInInspector]
    public Transform pivot;
    [HideInInspector]
    public Transform camTrans;
    #endregion

    static public CameraManager singleton;

    Vector3 targetPosition;
    [HideInInspector]
    public Vector3 targetPositionOffset;

    #region Internal Variables
    float x;
    float y;
    float lookAngle;
    float tiltAngle;
    float offsetX;
    float offsetY;
    float smoothX = 0;
    float smoothY = 0;
    float smoothXvelocity = 0;
    float smoothYvelocity = 0;
    #endregion

    [SerializeField]
    List<CameraState> cameraState = new List<CameraState>();
    CameraState activeState;
    CameraState defaultState;

    private void Awake()
    {
        singleton = this;
    }

    private void Start()
    {
        if(Camera.main.transform == null)
        {
            Debug.Log("You haven't assigned a camera with the tag 'MainCamera' !");
        }

        camTrans = Camera.main.transform.parent;
        pivot = camTrans.parent;

        //Create Default State
        CameraState cs = new CameraState();
        cs.id = "default";
        cs.minAngle = 35;
        cs.maxAngle = 35;
        cs.cameraFOV = Camera.main.fieldOfView;
        cs.cameraZ = camTrans.localPosition.z;
        cs.pivotPosition = pivot.localPosition;
        defaultState = cs;

        if (addDefaultAsNormal)
        {
            cameraState.Add(defaultState);
            defaultState.id = "normal";
        }

        activeState = defaultState;
        activeStateID = activeState.id;
        FixPositions();

        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

    }

    private void FixedUpdate()
    {
        if (target)
        {
            targetPosition = target.position + targetPositionOffset;
        }

        CameraFollow();

        if (!holdCamera)
            HandleRotation();

        FixPositions();
    }

    private void CameraFollow()
    {
        Vector3 camPosition = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);
        transform.position = camPosition;
    }

    private void HandleRotation()
    {
        HandleOffsets();

        x = Input.GetAxis("Mouse X") + offsetX;
        y = Input.GetAxis("Mouse Y") + offsetY;

        float targetTurnSpeed = turnSpeed;

        if (isController)
        {
            targetTurnSpeed = turnSpeedController;
        }

        if (turnSmoothing > 0)
        {
            smoothX = Mathf.SmoothDamp(smoothX, x, ref smoothXvelocity, turnSmoothing);
            smoothY = Mathf.SmoothDamp(smoothY, y, ref smoothYvelocity, turnSmoothing);
        }
        else
        {
            smoothX = x;
            smoothY = y;
        }

        lookAngle += smoothX * targetTurnSpeed;

        //reset the look angle when it does a full circle
        if (lookAngle > 360)
            lookAngle = 0;
        if (lookAngle < -360)
            lookAngle = 0;

        transform.rotation = Quaternion.Euler(0f, lookAngle, 0);

        tiltAngle -= smoothY * targetTurnSpeed;
        tiltAngle = Mathf.Clamp(tiltAngle, -activeState.minAngle, activeState.maxAngle);

        pivot.localRotation = Quaternion.Euler(tiltAngle, 0, 0);


    }


    //for gun recoil....
    private void HandleOffsets()
    {
        if (offsetX != 0)
        {
            offsetX = Mathf.MoveTowards(offsetX, 0, Time.deltaTime);
        }

        if(offsetY != 0)
        {
            offsetY = Mathf.MoveTowards(offsetY, 0, Time.deltaTime);
        }
    }


    CameraState GetState(string id)
    {
        CameraState r = null;
        for (int i = 0; i < cameraState.Count; i++)
        {
            if (cameraState[i].id == id)
            {
                r = cameraState[i];
                break;
            }
        }

        return r;
    }


    public void ChangeState(string id)
    {
        if (activeState.id != id)
        {
            CameraState targetState = GetState(id);
            if(targetState == null)
            {
                Debug.Log("Camera state ' " + id + " ' not found! Using previous");
                return;
            }

            activeState = targetState;
            activeStateID = activeState.id;
        }
    }

    private void FixPositions()
    {
        Vector3 targetPivotPosition = (activeState.useDefaultPosition) ? defaultState.pivotPosition : activeState.pivotPosition;
        pivot.localPosition = Vector3.Lerp(pivot.localPosition, targetPivotPosition, Time.deltaTime * 5);

        float targetZ = (activeState.useDefaultCameraZ) ? defaultState.cameraZ : activeState.cameraZ;
        Vector3 targetP = camTrans.localPosition;
        targetP.z = Mathf.Lerp(targetP.z, targetZ, Time.deltaTime * 5);
        camTrans.localPosition = targetP;

        float targetFov = (activeState.useDefaultFOV) ? defaultState.cameraFOV : activeState.cameraFOV;
        if(targetFov < 1)
        {
            targetFov = 2;
        }
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, targetFov, Time.deltaTime * 5);

    }

//removed his IEnum stuff


    [System.Serializable]
    public class CameraState
    {
        [Header("Name of state")]
        public string id;
        [Header("Limits")]
        public float minAngle;
        public float maxAngle;
        [Header("Pivot Position")]
        public bool useDefaultPosition;
        public Vector3 pivotPosition;
        [Header("Camera Position")]
        public bool useDefaultCameraZ;
        public float cameraZ;
        [Header("Camera FOV")]
        public bool useDefaultFOV;
        public float cameraFOV;
    }


}


    */

    /*
    [SerializeField]
    public GameObject target;
    public float lookSmooth = 0.09f;
    public Vector3 offsetFromTarget = new Vector3(0, 6, -8);
    public float xTilt = 10;

    private Vector3 destination = Vector3.zero;
    //private CharacterController charController;         //not sure why i need this for camera. maybe try some other component

    //private Transform charTransform;
    float rotateVel = 0;

    private void Start()
    {
        //SetCameraTarget(target);
    }


    private void LateUpdate()
    {
        //moving
        MoveToTarget();
        //rotating
        LookAtTarget();

    }
    */
    /*
    private void SetCameraTarget(Transform t)
    {
        target = t;

        // not sure why i need this, my player does not have a charactercontroller component...
        if (target != null)
        {
            if (target.GetComponent<Transform>())
            {
                charTransform = target.GetComponent<Transform>();
            }
            else
            {
                Debug.LogError("Your camera's target needs a charactercontroller");
            }
        }
        else
        {
            Debug.LogError("Your camera needs a target");
        }
    }
    */
    /*
    private void MoveToTarget()
    {
        //destination = target.rotation * offsetFromTarget;
        //destination += target.position;
        //transform.position = destination;

    }


    private void LookAtTarget()
    {

    }





    */




// another video i watched
/*

}

    */





// this is from a video i watched
/*
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
void LateUpdate()
{
    //follow target at specified place
    transform.position = targetToFollow.position - offsetFromPlayer * currentZoom;

    //look at target location and target height
    transform.LookAt(targetToFollow.position + Vector3.up * pitch);

    //NEED camera rotation
}
}

*/
