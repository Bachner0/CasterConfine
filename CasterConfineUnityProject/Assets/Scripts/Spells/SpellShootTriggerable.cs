using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This will be the launching script for bolts.
/// Make one for Arcs
/// Make one for streaks
/// Make one for other spells
/// </summary>

public class SpellShootTriggerable : MonoBehaviour
{

    [HideInInspector] public Rigidbody projectile;         // Rigidbody variable to hold a reference to our projectile prefab
    public Transform spellSpawn;                           // Transform variable to hold the location where we will spawn our projectile
    [HideInInspector] public float projectileForce = 250f; // Float variable to hold the amount of force which we will apply to launch our projectiles

    public void Launch()
    {
        //Instantiate a copy of our projectile and store it in a new rigidbody variable called clonedBullet
        Rigidbody clonedBullet = Instantiate(projectile, spellSpawn.position, transform.rotation) as Rigidbody;

        //Add force to the instantiated bullet, pushing it forward away from the bulletSpawn location, using projectile force for how hard to push it away
        clonedBullet.AddForce(spellSpawn.transform.forward * projectileForce);
    }
}
