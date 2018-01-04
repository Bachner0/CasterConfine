using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerSettings : Photon.PunBehaviour
{
    private PhotonView PhotonView;
    public float Health;


    void Awake()
    {
        PhotonView = GetComponent<PhotonView>();
    }


    /// <summary>
    /// First, it gets the CameraWork component, we expect this, so if we don't find it, we log an error. 
    /// Then, if photonView.isMine is true, it means we need to follow this instance, and so we call _cameraWork.OnStartFollowing()
    /// which effectivly makes the camera follow that very instance in the scene.
    /// </summary>
    private void Start()
    {
        CameraController _cameraController = this.gameObject.GetComponent<CameraController>();
        Canvas _playerUIcanvas = this.gameObject.GetComponentInChildren<Canvas>();

        if (_cameraController != null)
        {
            if (photonView.isMine)
            {
                _cameraController.OnStartFollowing();
                _playerUIcanvas.enabled = true;

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


    // https://www.youtube.com/watch?v=4VxoE-dsziM
    //trying to set up the player's selected skill bar
    //will be generic to begin with, but will later need to access the buildout saved in an locally stored file
    public Spell[] selectedSpellsLoadout;       //drop these in .... later will need to figure out how to get this list of spells from a player made list
    public GameObject spellBar;
    public GameObject spellInstantiationLocation;   //this is for locating all the spell triggerables. put all the spell triggerables on this
                                                    //attached gameobject

}
