using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://www.youtube.com/watch?v=GoeW_ZBvfqM


public class Spell : ScriptableObject
{
    public string spellName = "";
    public GameObject spellPrefab = null;
    public GameObject spellCollisionParticle = null;
    public Texture2D spellIcon = null;

    public int spellManaCost = 0;       //may need to make these ranges to immitate mana conversion
    public int spellStaminaCost = 0;    //may need to make these ranges to immitate mana conversion
    public int spellMinDamage = 0;
    public int spellMaxDamage = 0;
    public int projectileSpeed = 0;

    // may need to have something that controls the arc on arc spells

}
