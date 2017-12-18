using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //we want the server to handle this and not the client
        //but if host drops, then game is dead
        if (!PhotonNetwork.isMasterClient)
            return;

        //get the photonview component that hit the trigger (if it needs to sync with the server, it should have a photonview component
        PhotonView photonView = other.GetComponent<PhotonView>();
        //make sure the photonview is not null
        if (photonView != null)
            PlayerManagement.Instance.ModifyHealth(photonView.owner, -10); //owner is the player that owns the photonview. -10 is the amount of health change.
    }


}
