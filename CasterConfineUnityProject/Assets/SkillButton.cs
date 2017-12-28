using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// TODO:
/// 
///     -make the highlighted sprite not show on hover
///  
///     -make a variable for the clicked button and set it when the button is pressed so the spell can be called
/// 
///     -make the highlighted sprite stay on the button that it is on
///     
///     -select the button with the keyboard
/// 
/// </summary>





public class SkillButton : MonoBehaviour
{

    Button button;



	// Use this for initialization
	void Start ()
    {
        SpriteState frank = new SpriteState();


        //.spriteState.disabledSprite = Resources.Load<Sprite>("spriteName");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
