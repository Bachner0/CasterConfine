using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : Photon.PunBehaviour
{

    public float health = 350f;
    private float currentHitPoints;

    //Use for initialization
    private void Start()
    {
        currentHitPoints = currentHitPoints;            //something wrong here? 
    }

    [PunRPC]
    public void TakeDamage(float amt)
    {
        currentHitPoints -= amt;

        if(currentHitPoints <= 0)
        {
            Die();
        }
    }


    private void Die()
    {
        //something something 
        //PhotonNetwork.Destroy(gameObject);
    }

}
