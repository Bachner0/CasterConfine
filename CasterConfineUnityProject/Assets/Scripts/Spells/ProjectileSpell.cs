using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//inherits the scriptable object

[CreateAssetMenu(menuName = "Spells/ProjectileSpell")]
public class ProjectileSpell : Spell
{

    public float projectileForce = 500f;
    public Rigidbody projectile;

    private SpellShootTriggerable launcher;

    public override void Initialize(GameObject obj)
    {
        launcher = obj.GetComponent<SpellShootTriggerable>();
        launcher.projectileForce = projectileForce;
        launcher.projectile = projectile;
    }


    //this overrides the abstract defined call to implement this method. Here I am making the projectile go in the SpellShootTriggerable script
    public override void TriggerAbility()
    {
        launcher.Launch();
    }

}
