using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellCoolDown : MonoBehaviour {

    public KeyCode activateKey;             //the key that triggers this button, also see below in update
    public Image darkMask;
    //public Text coolDownTextDisplay;

    private Spell spell;
    private GameObject weaponHolder;       

    private Image myButtonImage;
    private AudioSource spellSoundSource;
    private float coolDownDuration;
    private float nextReadyTime;
    private float coolDownTimeLeft;


    void Start()
    {
        //Initialize(spell, weaponHolder);    //will need to remove
    }

    public void Initialize(Spell selectedSpell, GameObject weaponHolder)
    {
        spell = selectedSpell;
        myButtonImage = GetComponent<Image>();
        spellSoundSource = GetComponent<AudioSource>();
        myButtonImage.sprite = spell.spellIcon;
        darkMask.sprite = spell.spellIcon;
        coolDownDuration = spell.spellCoolDown;
        spell.Initialize(weaponHolder);
        SpellReady();
    }

    // Update is called once per frame
    void Update()
    {
        bool coolDownComplete = (Time.time > nextReadyTime);
        if (coolDownComplete)
        {
            SpellReady();
            if (Input.GetKeyDown(activateKey))                   //this is where the spell is first pressed
                                                                    // add an 'OR' statement that checks to see if double clicked or selected and the end key is pressed.
                                                                    //i'll need to integrate all my spell sequence and checks
                                                                    //before it actually gets triggered
            {
                ButtonTriggered();
            }
        }
        else
        {
            CoolDown();
        }
    }

    private void SpellReady()
    {
        //coolDownTextDisplay.enabled = false;
        darkMask.enabled = false;
    }

    private void CoolDown()
    {
        coolDownTimeLeft -= Time.deltaTime;
        //float roundedCd = Mathf.Round(coolDownTimeLeft);
        //coolDownTextDisplay.text = roundedCd.ToString();
        darkMask.fillAmount = (coolDownTimeLeft / coolDownDuration);
    }

    private void ButtonTriggered()
    {
        nextReadyTime = coolDownDuration + Time.time;
        coolDownTimeLeft = coolDownDuration;
        darkMask.enabled = true;
        //coolDownTextDisplay.enabled = true;

        spellSoundSource.clip = spell.spellSuccessSound;
        spellSoundSource.Play();
        spell.TriggerAbility();
    }
}
