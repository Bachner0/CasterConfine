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
    private float redSpeed = 0.45f;     //slide speed modifier
    private float slidePower = 20.0f;   //slide speed modifier
    private float rotateToTargetSpeed = 60.0f;


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

    #region Sequence bools
    bool isBusyCasting;
    bool hasSelection;
    bool isEnemy;
    bool hasEnoughMana;
    bool lookingAtTarget;
    bool armsGoingDown;
    bool breakAnimation;
    bool isInCastCircle;
    bool cancelCast;

    #endregion

    #region Timers
    float maxRotateTimer = 10f;
    float armsMovingTimer = 2f;
    float breakTimer = 2f;

    #endregion


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
        mHash.Add("cwdW", "arcoutright");   //these aren't the same because of the forward step
        mHash.Add("cxdX", "arcoutright");
        mHash.Add("cwaW", "arcbackright");  //these aren't the same because of the forward step
        mHash.Add("cxaX", "arcbackright");
        mHash.Add("zwdW", "arcoutleft");    //these aren't the same because of the forward step
        mHash.Add("zxdX", "arcoutleft");
        mHash.Add("zwaW", "arcbackleft");   //these aren't the same because of the forward step
        mHash.Add("zxaX", "arcbackleft");
        mHash.Add("zawW", "extremeturnleft");
        mHash.Add("cdwW", "extremeturnright");
        mHash.Add("zaw", "wideanglediagforwardleft");   //straight slide when casting
        mHash.Add("cdw", "wideanglediagforwardright");  //straight slide when casting
        mHash.Add("zawd", "wsslideleftrotateright"); //these aren't the same because of the forward step
        mHash.Add("zaxd", "backsslideleftrotateright");
        mHash.Add("cdwa", "wssliderightrotateleft"); //these aren't the same because of the forward step
        mHash.Add("cdxa", "backssliderightrotateleft");

        //could add some more where a or d are tapping at the end of 3 (ex: zwxd - would add a little rotate on it)

    }


    private void Start()
    {
        //anim = GetComponent<Animator>();
        //myTransform = transform;

        // GameObject.Find("CastCollider").GetComponent<CastCollider>().okToCast    -  need to get access to the bool in the cast circle prefab

        stateInt = 0;

    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonView.isMine)
        {
            CheckInput();                   //this receives input and puts it in the list

            ControlMethod(stateInt);        //changes the state of the movement restrictions (movement executed in here)

        }
        else
        {
            //this is where other players' movements are updated.
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
            if (mList.Count > 3) mList.Remove('X');
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
            if (mList.Count > 3) mList.Remove('X');
            mList.Remove('X');
            mList.AddLast('X');
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (mList.Count > 2) mList.Remove('w');
            if (mList.Count > 2) mList.Remove('x');     // was causing backwards movement
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
            if (mList.Count > 2) mList.Remove('x');     // was causing backwards movement
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
            //if (mList.Count > 2) mList.Remove('X');     // was causing backwards movement
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
            //if (mList.Count > 2) mList.Remove('X');     // was causing backwards movement
            mList.Remove('d');
            mList.AddLast('d');
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            mList.Remove('d');
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (mList.Count > 3) mList.Remove('W');
            //if (mList.Count > 2) mList.Remove('X');   // was causing backwards movement
            mList.Remove('a');
            mList.AddLast('a');
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            mList.Remove('a');
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (mList.Count > 3) mList.Remove('W');
            //if (mList.Count > 2) mList.Remove('X');     // was causing backwards movement
            mList.Remove('d');
            mList.AddLast('d');
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            mList.Remove('d');
        }
        #endregion

        
    }

    
    void ControlMethod(int currentState)                //state gets changed when casting etc... to be called when doing so
    {                                                   //I removed all the casting code from this script
        switch (currentState)
        {
            case 0: //unrestricted running
                    //all keys work normally
                FreeRun();

                break;
            case 1: //rotate movement for rotating to target
                    // a, d rotate speed reduced by amount auto rotate is
                    // w, z, c changes bool to take 1 step/animation when called
                TargetRotationRun();

                break;
            case 2: //slide movement during actual casting
                    // w, z, c, x are ignored in non-combo processing
                    // slides work only
                SlideRun();

                break;
            case 3: //movement after the cast break
                    //no z, c, x (strafe and backward)
                    // this only lasts for a moment
                BreakRun();
                    //always begins with a list reset or pause or something
                break;
            case 4: //no movement
                    //all movement is suspended
                NoMoveRun();

                break;
        }
    }


    void FreeRun()
    {
        string m = "";

        foreach (char i in mList)
        {
            m = m + i.ToString();
        }
        //displays contents of input list to console.
        //if (m != null)
        //{ Debug.Log(m); }

        string items = "";
        if (mHash.TryGetValue(m, out items))            //the stored list or hash looks for these combos
        {
            if (items == "sslideright")
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
            else if (items == "extremeturnleft")
            {
                transform.position += -transform.right * slidePower / 2 * Time.deltaTime;
                transform.Rotate(Vector3.down * rotSpeed * Time.deltaTime * 2);
            }
            else if (items == "extremeturnright")
            {
                transform.position += transform.right * slidePower / 2 * Time.deltaTime;
                transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime * 2);
            }

            else if (items == "wideanglediagforwardleft")
            {
                transform.position += transform.forward * incSpeed * Time.deltaTime;
                transform.position += (-transform.right * 2) * incSpeed * redSpeed * Time.deltaTime;
            }
            else if (items == "wideanglediagforwardright")
            {
                transform.position += transform.forward * incSpeed * Time.deltaTime;
                transform.position += (transform.right * 2) * incSpeed * redSpeed * Time.deltaTime;
            }
            else if (items == "backsslideleftrotateright")
            {
                transform.position += -transform.forward * redSpeed * Time.deltaTime;
                transform.position += -transform.right * slidePower * Time.deltaTime;
                transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime);
            }
            else if (items == "backssliderightrotateleft")
            {
                transform.position += -transform.forward * redSpeed * Time.deltaTime;
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

    void TargetRotationRun()        //needs work on the one steps
    {
        string m = "";

        foreach (char i in mList)
        {
            m = m + i.ToString();
        }
        //displays contents of input list to console.
        //if (m != null)
        //{ Debug.Log(m); }

        string items = "";
        if (mHash.TryGetValue(m, out items))            //the stored list or hash looks for these combos
        {
            //new 12/26/17
            if (items == "extremeturnleft")
            {
                transform.position += -transform.right * slidePower / 2 * Time.deltaTime;
                transform.Rotate(Vector3.down * ((rotSpeed * 2) - rotateToTargetSpeed) * Time.deltaTime);             //reduced rotate speed by auto turn speed
            }
            else if (items == "extremeturnright")
            {
                transform.position += transform.right * slidePower / 2 * Time.deltaTime;
                transform.Rotate(Vector3.up * ((rotSpeed * 2) - rotateToTargetSpeed) * Time.deltaTime);             //reduced rotate speed by auto turn speed
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
            if (w == Move.wxaz) {  }    //should take one step and animation only
            if (x == Move.wxaz) {  }
            if (a == Move.wxaz) { move = move + 'a'; transform.Rotate(Vector3.down * (rotSpeed - rotateToTargetSpeed) * Time.deltaTime); }  //reduced rotate speed by auto turn speed
            if (a == Move.dc) { move = move + 'd'; transform.Rotate(Vector3.up * (rotSpeed - rotateToTargetSpeed) * Time.deltaTime); }  //reduced rotate speed by auto turn speed
            if (z == Move.wxaz) { }     //should take one step and animation only
            if (z == Move.dc) { }       //should take one step and animation only
        }
    }

    void SlideRun()
    {
        string m = "";

        foreach (char i in mList)
        {
            m = m + i.ToString();
        }
        //displays contents of input list to console.
        //if (m != null)
        //{ Debug.Log(m); }

        string items = "";
        if (mHash.TryGetValue(m, out items))            //the stored list or hash looks for these combos
        {
            if (items == "sslideright")
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
            else if (items == "extremeturnleft")
            {
                transform.position += -transform.forward * redSpeed * Time.deltaTime;
                transform.position += transform.right * slidePower * Time.deltaTime;
            }
            else if (items == "extremeturnright")
            {
                transform.position += -transform.forward * redSpeed * Time.deltaTime;
                transform.position += -transform.right * slidePower * Time.deltaTime;
            }

            else if (items == "wideanglediagforwardleft")
            {
                transform.position += -transform.forward * redSpeed * Time.deltaTime;
                transform.position += transform.right * slidePower * Time.deltaTime;
            }
            else if (items == "wideanglediagforwardright")
            {
                transform.position += -transform.forward * redSpeed * Time.deltaTime;
                transform.position += -transform.right * slidePower * Time.deltaTime;
            }
            else if (items == "backsslideleftrotateright")
            {
                transform.position += -transform.forward * redSpeed * Time.deltaTime;
                transform.position += -transform.right * slidePower * Time.deltaTime;
                transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime);
            }
            else if (items == "backssliderightrotateleft")
            {
                transform.position += -transform.forward * redSpeed * Time.deltaTime;
                transform.position += transform.right * slidePower * Time.deltaTime;
                transform.Rotate(Vector3.down * rotSpeed * Time.deltaTime);
            }
            else if (items == "wsslideleftrotateright")
            {
                transform.position += -transform.forward * redSpeed * Time.deltaTime;
                transform.position += -transform.right * slidePower * Time.deltaTime;
                transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime);
            }
            else if (items == "wssliderightrotateleft")
            {
                transform.position += -transform.forward * redSpeed * Time.deltaTime;
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
            if (w == Move.wxaz) { }
            if (x == Move.wxaz) {  }
            if (a == Move.wxaz) { move = move + 'a'; transform.Rotate(Vector3.down * rotSpeed * Time.deltaTime); }
            if (a == Move.dc) { move = move + 'd'; transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime); }
            if (z == Move.wxaz) {  }
            if (z == Move.dc) {  }
        }
    }

    void BreakRun()
    {
        string m = "";

        foreach (char i in mList)
        {
            m = m + i.ToString();
        }
        //displays contents of input list to console.
        //if (m != null)
        //{ Debug.Log(m); }

        string items = "";
        if (mHash.TryGetValue(m, out items))            //the stored list or hash looks for these combos
        {
            if (items == "sslideright")
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
            else if (items == "extremeturnleft")
            {
                transform.position += -transform.forward * redSpeed * Time.deltaTime;
                transform.position += transform.right * slidePower * Time.deltaTime;
            }
            else if (items == "extremeturnright")
            {
                transform.position += -transform.forward * redSpeed * Time.deltaTime;
                transform.position += -transform.right * slidePower * Time.deltaTime;
            }

            else if (items == "wideanglediagforwardleft")
            {
                transform.position += -transform.forward * redSpeed * Time.deltaTime;
                transform.position += transform.right * slidePower * Time.deltaTime;
            }
            else if (items == "wideanglediagforwardright")
            {
                transform.position += -transform.forward * redSpeed * Time.deltaTime;
                transform.position += -transform.right * slidePower * Time.deltaTime;
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
            if (x == Move.wxaz) { }
            if (a == Move.wxaz) { move = move + 'a'; transform.Rotate(Vector3.down * rotSpeed * Time.deltaTime); }
            if (a == Move.dc) { move = move + 'd'; transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime); }
            if (z == Move.wxaz) { }
            if (z == Move.dc) { }
        }
    }

    void NoMoveRun()
    {
        mList.Clear();
    }

    enum Move { off, wxaz, dc };

}

