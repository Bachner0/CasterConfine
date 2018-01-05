using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//scriptable object
//
//https://www.youtube.com/watch?v=GoeW_ZBvfqM
//https://unity3d.com/learn/tutorials/topics/scripting/ability-system-scriptable-objects

    /// <summary>
    /// These are the basic stats for any created spell
    /// </summary>

public abstract class Spell : ScriptableObject
{
    public string spellName = "";

    public int spellManaCost = 0;       //may need to make these ranges to immitate mana conversion
    public int spellStaminaCost = 0;    //may need to make these ranges to immitate mana conversion
    public int spellMinDamage = 0;
    public int spellMaxDamage = 0;
    public int projectileSpeed = 0;
    public float spellCoolDown = 0f;

    //public GameObject spellPrefab = null; handled in ProjectileSpell.cs
    public GameObject spellCollisionParticle = null;
    public Sprite spellIcon = null;
    public AudioClip spellWindUpSound = null;
    public AudioClip spellSuccessSound = null;
    public AudioClip spellCollisionSound = null;

    // may need to have something that controls the arc on arc spells



    public abstract void Initialize(GameObject obj);
    public abstract void TriggerAbility();

}
