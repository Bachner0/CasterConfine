using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[RequireComponent(typeof(BoxCollider))]
//do I need other required components?

public class PlayerMovement : Photon.MonoBehaviour              //added photon. (important)
{
    /*
    public Rigidbody playerRigidBody;           // add the rigid body to this field
    public Transform target;                    // from old script... for player selecting a target. does it go here?
    Camera playerCam;
    */

    public float Health;

    
    private float incSpeed = 12.0f;
    private float rotSpeed = 120.0f;
    private float redSpeed = 0.45f;
    private float slidePower = 20.0f;

    LinkedList<char> mList = new LinkedList<char>();
    Dictionary<string, string> mHash = new Dictionary<string, string>();

    //variable for playerstate switch statement
    private int stateInt = 0;


    //rotating variables
    //private Transform myTransform;      //cache to improve performance
    private Quaternion targetRotation;


    //16:49 in #7       smooth move -- variables so that other objects move smoothly. not sure if i want
                        // additionally, we put this component into the photonview focus
    private Vector3 TargetPosition;
    private Quaternion TargetRotation;




    private PhotonView PhotonView;    

    // Use this for initialization
    void Awake()
    {
        PhotonView = GetComponent<PhotonView>();


        // Powerslide combos
        mHash.Add("wW", "Idle");
        mHash.Add("xX", "Idle");
        mHash.Add("W", "Idle");
        mHash.Add("X", "Idle");
        mHash.Add("wWX", "Idle");
        mHash.Add("WX", "Idle");
        mHash.Add("wXW", "Idle");
        mHash.Add("WxX", "Idle");
        mHash.Add("XW", "Idle");
        mHash.Add("XwW", "Idle");
        mHash.Add("xWX", "Idle");
        mHash.Add("xXW", "Idle");
        mHash.Add("xXwW", "Idle");
        mHash.Add("wWxX", "Idle");
        mHash.Add("wxWX", "Idle");
        mHash.Add("xwXW", "Idle");
        mHash.Add("zax", "sslideright");
        mHash.Add("zdx", "sslideright");
        mHash.Add("cax", "sslideleft");
        mHash.Add("cdx", "sslideleft");
        mHash.Add("cwdW", "arcoutright");
        mHash.Add("cxdX", "arcoutright");
        mHash.Add("cwaW", "arcbackright");
        mHash.Add("cxaX", "arcbackright");
        mHash.Add("zwdW", "arcoutleft");
        mHash.Add("zxdX", "arcoutleft");
        mHash.Add("zwaW", "arcbackleft");
        mHash.Add("zxaX", "arcbackleft");
        mHash.Add("wd", "interruptright");
        mHash.Add("wa", "interruptleft");

    }


    private void Start()
    {
        //anim = GetComponent<Animator>();
        //myTransform = transform;

        // GameObject.Find("CastCollider").GetComponent<CastCollider>().okToCast    -  need to get access to the bool in the cast circle prefab

    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonView.isMine)
        {
            CheckInput();
        }
        else
        {
            //16:49 in #7       smooth move -- not sure if i want
            SmoothMove();
        }
    }

    //called every time you receive a packet for this object, whether it is yours or someone else's. that is why this component is added to photonview focus
    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting) //true is we are sending the data
        {
            //the order of data sent
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            //must be the same order as above
            TargetPosition = (Vector3)stream.ReceiveNext();
            TargetRotation = (Quaternion)stream.ReceiveNext();
        }
    }


    //16:49 in #7       smooth move -- not sure if i want
    private void SmoothMove()
    {
        transform.position = Vector3.Lerp(transform.position, TargetPosition, 0.25f);   //the float is between 0 and 1. higher means less accurate
        transform.rotation = Quaternion.RotateTowards(transform.rotation, TargetRotation, 500 * Time.deltaTime);
    }

    private void CheckInput()
    { 

        #region Movement input

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (mList.Count > 2) mList.Remove('w');
            if (mList.Count > 2) mList.Remove('x');
            if (mList.Count > 3) mList.Remove('W');
            mList.Remove('w');
            mList.AddLast('w');
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            if (mList.Count > 3) mList.Remove('w');
            if (mList.Count > 3) mList.Remove('x');
            mList.Remove('W');
            mList.AddLast('W');
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (mList.Count > 2) mList.Remove('w');
            if (mList.Count > 2) mList.Remove('x');
            if (mList.Count > 3) mList.Remove('X');
            mList.Remove('x');
            mList.AddLast('x');
        }
        if (Input.GetKeyUp(KeyCode.X))
        {
            if (mList.Count > 3) mList.Remove('w');
            if (mList.Count > 3) mList.Remove('x');
            mList.Remove('X');
            mList.AddLast('X');
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (mList.Count > 2) mList.Remove('w');
            if (mList.Count > 2) mList.Remove('x');
            if (mList.Count > 3) mList.Remove('W');
            mList.Remove('w');
            mList.AddLast('w');
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            if (mList.Count > 3) mList.Remove('w');
            if (mList.Count > 3) mList.Remove('x');
            mList.Remove('W');
            mList.AddLast('W');
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (mList.Count > 2) mList.Remove('w');
            if (mList.Count > 2) mList.Remove('x');
            if (mList.Count > 3) mList.Remove('X');
            mList.Remove('x');
            mList.AddLast('x');
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            if (mList.Count > 3) mList.Remove('w');
            if (mList.Count > 3) mList.Remove('x');
            mList.Remove('X');
            mList.AddLast('X');
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (mList.Count > 2) mList.Remove('w');
            if (mList.Count > 2) mList.Remove('x');     // revist, maybe remove
            mList.Remove('z');
            mList.AddLast('z');
        }
        if (Input.GetKeyUp(KeyCode.Z))
        {
            mList.Remove('z');
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (mList.Count > 3) mList.Remove('w');
            if (mList.Count > 2) mList.Remove('x');     // revist, maybe remove
            mList.Remove('c');
            mList.AddLast('c');
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            mList.Remove('c');
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (mList.Count > 3) mList.Remove('W');
            if (mList.Count > 2) mList.Remove('X');     // revist, maybe remove
            mList.Remove('a');
            mList.AddLast('a');
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            mList.Remove('a');
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (mList.Count > 3) mList.Remove('W');
            if (mList.Count > 2) mList.Remove('X');     // revist, maybe remove
            mList.Remove('d');
            mList.AddLast('d');
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            mList.Remove('d');
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (mList.Count > 3) mList.Remove('W');     // revist, maybe remove
            if (mList.Count > 2) mList.Remove('X');
            mList.Remove('a');
            mList.AddLast('a');
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            mList.Remove('a');
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (mList.Count > 3) mList.Remove('W');     // revist, maybe remove
            if (mList.Count > 2) mList.Remove('X');
            mList.Remove('d');
            mList.AddLast('d');
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            mList.Remove('d');
        }
        #endregion

        ControlMethod(stateInt);                        // this has to do with defining the state of movement.
                                                        // was going to use this to define what can be done when starting to cast

    }

    
    void ControlMethod(int currentState)                //state gets changed when casting etc... to be called when doing so
    {                                                   //I removed all the casting code from this script
        switch (currentState)
        {
            case 0: //running
                    //all keys work normally
                runningPls();
                break;
            case 1: //castnormal
                    // z, c, x are ignored in non-combo processing
                    // w changes bool to take 1 step forward when called
         //       castnormalPls();
                break;
            case 2: //breakanimation
                    // w, z, c, x are ignored in non-combo processing
                    // new combinations change bool to break animation + 1 step forward
         //       breakAnimationPls();
                break;
            case 3: //hiccup
                    //all keys and combos ignored in processing
         //       hiccupPls();
                break;
        }
    }


    void runningPls()
    {
        string m = "";

        foreach (char i in mList)
        {
            m = m + i.ToString();
        }

        //if (m != null)
        //{ Debug.Log(m); }

        string items = "";
        if (mHash.TryGetValue(m, out items))                                            //the stored list or hash looks for these combos
        {
            if (items == "interruptleft")
            {
                transform.position += transform.forward * incSpeed * Time.deltaTime;
                transform.Rotate(Vector3.down * rotSpeed * Time.deltaTime);
            }
            else if (items == "interruptright")
            {
                transform.position += transform.forward * incSpeed * Time.deltaTime;
                transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime);
            }
            else if (items == "sslideright")
            {
                transform.position += -transform.forward * redSpeed * Time.deltaTime;
                transform.position += -transform.right * slidePower * Time.deltaTime;
            }
            else if (items == "sslideleft")
            {
                transform.position += -transform.forward * redSpeed * Time.deltaTime;
                transform.position += transform.right * slidePower * Time.deltaTime;
            }
            else if (items == "arcoutleft")
            {
                transform.position += -transform.right * slidePower * Time.deltaTime;
                transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime);
            }
            else if (items == "arcbackleft")
            {
                transform.position += -transform.right * slidePower * Time.deltaTime;
                transform.Rotate(Vector3.down * rotSpeed * Time.deltaTime);
            }
            else if (items == "arcoutright")
            {
                transform.position += transform.right * slidePower * Time.deltaTime;
                transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime);
            }
            else if (items == "arcbackright")
            {
                transform.position += transform.right * slidePower * Time.deltaTime;
                transform.Rotate(Vector3.down * rotSpeed * Time.deltaTime);
            }
            else if (items == "Idle") { mList.Clear(); }
            else if (items == "dump") { mList.Clear(); }
        }
        else
        {
            var w = Move.off;
            var x = Move.off;
            var a = Move.off;
            var z = Move.off;
            string move = "";

            foreach (char i in m)
            {
                if (i == 'w')
                {
                    w = Move.wxaz;
                    x = Move.off;
                }
                else if (i == 'W') w = Move.off;
                else if (i == 'x')
                {
                    x = Move.wxaz;
                    w = Move.off;
                }
                else if (i == 'X') x = Move.off;
                else if (i == 'a') a = Move.wxaz;
                else if (i == 'd') a = Move.dc;
                else if (i == 'z') z = Move.wxaz;
                else if (i == 'c') z = Move.dc;
            }
            if (w == Move.wxaz) { move = move + 'w'; transform.position += transform.forward * incSpeed * Time.deltaTime; }
            if (x == Move.wxaz) { move = move + 'x'; transform.position += -transform.forward * incSpeed * redSpeed * Time.deltaTime; }
            if (a == Move.wxaz) { move = move + 'a'; transform.Rotate(Vector3.down * rotSpeed * Time.deltaTime); }
            if (a == Move.dc) { move = move + 'd'; transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime); }
            if (z == Move.wxaz) { move = move + 'z'; transform.position += -transform.right * incSpeed * redSpeed * Time.deltaTime; }
            if (z == Move.dc) { move = move + 'c'; transform.position += transform.right * incSpeed * redSpeed * Time.deltaTime; }
        }
    }
    
    enum Move { off, wxaz, dc };

}

