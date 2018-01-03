using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// I might not need to use this...
/// </summary>

    /*

//https://www.youtube.com/watch?v=5mm8JheRM9c
//also tells how to cast a spell/projectile


public class SpellCreator : EditorWindow
{
    [MenuItem("Spell Maker/Spell Wizard")]

    static void Init()
    {
        SpellCreator spellWindow = (SpellCreator)CreateInstance(typeof(SpellCreator));
        spellWindow.Show();
    }

    Spell tempSpell = null;
    SpellManager spellManager = null;

    private void OnGUI()
    {
        if(spellManager == null)
        {
            spellManager = GameObject.Find("SpellManager").GetComponent<SpellManager>();
        }

        if (tempSpell)
        {
            tempSpell.spellName = EditorGUILayout.TextField("Spell Name", tempSpell.spellName);
            tempSpell.spellPrefab = (GameObject)EditorGUILayout.ObjectField("Spell Prefab", tempSpell.spellPrefab, typeof(GameObject), false);
            tempSpell.spellCollisionParticle = (GameObject)EditorGUILayout.ObjectField("Spell Collision Effect", tempSpell.spellCollisionParticle, typeof(GameObject), false);
            tempSpell.spellIcon = (Texture2D)EditorGUILayout.ObjectField("Spell Icon", tempSpell.spellIcon, typeof(Texture2D), false);
            tempSpell.spellSound = (AudioClip)EditorGUILayout.ObjectField("Spell Sound", tempSpell.spellSound, typeof(AudioClip), false);


            tempSpell.spellManaCost = EditorGUILayout.IntField("Mana Cost", tempSpell.spellManaCost);
            tempSpell.spellStaminaCost = EditorGUILayout.IntField("Stamina Cost", tempSpell.spellStaminaCost);
            tempSpell.spellMinDamage = EditorGUILayout.IntField("Spell Min Damage", tempSpell.spellMinDamage);
            tempSpell.spellMaxDamage = EditorGUILayout.IntField("Spell Max Damage", tempSpell.spellMaxDamage);
            tempSpell.projectileSpeed = EditorGUILayout.IntField("Projectile Speed", tempSpell.projectileSpeed);
            tempSpell.spellCoolDown = EditorGUILayout.FloatField("Cool Down", tempSpell.spellCoolDown);
        }

        EditorGUILayout.Space();

        if(tempSpell == null)
        {
            if(GUILayout.Button("Create Spell"))
            {
                tempSpell = CreateInstance<Spell>();
            }
        }
        else
        {
            if(GUILayout.Button("Create Scriptable Object"))
            {
                AssetDatabase.CreateAsset(tempSpell, "Assets/Resources/Spells/" + tempSpell.spellName + ".asset");
                AssetDatabase.SaveAssets();
                spellManager.spellList.Add(tempSpell);
                Selection.activeObject = tempSpell;

                tempSpell = null;
            }
            if (GUILayout.Button("Reset"))
            {
                Reset();
            }
        }
    }

    void Reset()
    {
        tempSpell.spellName = "";
        tempSpell.spellIcon = null;
        tempSpell.spellManaCost = 0;
        tempSpell.spellStaminaCost = 0;
        tempSpell.spellMinDamage = 0;
        tempSpell.spellMaxDamage = 0;
        tempSpell.spellPrefab = null;
        tempSpell.spellCollisionParticle = null;
        tempSpell.projectileSpeed = 0;

    }

}
*/