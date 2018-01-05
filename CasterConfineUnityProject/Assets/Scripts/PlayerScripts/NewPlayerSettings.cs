using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerSettings : MonoBehaviour
{
    private PhotonView photonView;

    public Spell[] selectedSpellsLoadout;       //drop these in .... later will need to figure out how to get this list of spells from a player made list

    public GameObject spellBar;

    public GameObject spellInstantiationLocation;   //this is for locating all the spell triggerables. put all the spell triggerables on this
                                                    //attached gameobject


    void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }


    /// <summary>
    /// First, it gets the CameraWork component, we expect this, so if we don't find it, we log an error. 
    /// Then, if photonView.isMine is true, it means we need to follow this instance, and so we call _cameraWork.OnStartFollowing()
    /// which effectivly makes the camera follow that very instance in the scene.
    /// </summary>
    private void Start()
    {
        CameraController _cameraController = this.gameObject.GetComponent<CameraController>();

        if (_cameraController != null)
        {
            if (photonView.isMine)
            {
                Canvas _playerUIcanvas = this.gameObject.GetComponentInChildren<Canvas>();
                _cameraController.OnStartFollowing();
                _playerUIcanvas.enabled = true;
                FillInSpellBar();

            }
        }
        else
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }


    // loads the spell into the slots
    // https://www.youtube.com/watch?v=4VxoE-dsziM
    //trying to set up the player's selected skill bar
    //will be generic to begin with, but will later need to access the buildout saved in an locally stored file
    //change to pass in the custom spell bar
    public void FillInSpellBar()
    {
        spellBar.SetActive(true);

        //i want the canvas to generate a button for each spell on the selectedSpellsLoadout array. maybe it should be a spell list?


        SpellCoolDown[] coolDownButtons = GetComponentsInChildren<SpellCoolDown>(); //searching the canvas for any skill bar buttons
        for (int i = 0; i < coolDownButtons.Length; i++)
        {
            coolDownButtons[i].Initialize(selectedSpellsLoadout[i], spellInstantiationLocation);    //don't understand why the buttons need to be initialized but following tutorial

        }
    }


}
