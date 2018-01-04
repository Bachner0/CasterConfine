using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://www.youtube.com/watch?v=4VxoE-dsziM
//trying to set up the player's selected skill bar
//will be generic to begin with, but will later need to access the buildout saved in an locally stored file

public class LoadSpellBar : MonoBehaviour
{
    public Spell[] selectedSpellsLoadout;       //drop these in .... later will need to figure out how to get this list of spells from a player made list

    public GameObject spellBar;

    public GameObject spellInstantiationLocation;   //this is for locating all the spell triggerables. put all the spell triggerables on this
                                                    //attached gameobject
    private PhotonView photonView;

    void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (photonView.isMine)
        {


            FillInSpellBar();
        }

    }



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
