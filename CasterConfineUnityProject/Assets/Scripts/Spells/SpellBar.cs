using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBar : MonoBehaviour {

    public int numberOfSpells = 12;
    public SpellSlot[] spellsEquipped;




    private void Awake()
    {
        //initialize the spells??
        //https://www.youtube.com/watch?v=qXNLMLcVY2E @11:30
        //But I have spell manager that may load the scriptable objects from the other video


    }

    void Start()
    {
        spellsEquipped = new SpellSlot[numberOfSpells];
        //load the spells into this array??


    }

    void Update ()
    {
		
	}
}
