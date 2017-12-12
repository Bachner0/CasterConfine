using UnityEngine;
using UnityEngine.UI;

public class CurrentRoomCanvas : MonoBehaviour
{

    /*
        NEED to hide buttons if not the host

    */

    public void OnClickStartSync()
    {
        if (!PhotonNetwork.isMasterClient)      //can't click if not host
            return;

        //loads the scene #..... will want to modify this method based on the room settings
        PhotonNetwork.LoadLevel(1);
    }

    public void OnClickStartDelayed()
    {
        if (!PhotonNetwork.isMasterClient)      //can't click if not host
            return;

        PhotonNetwork.room.IsOpen = false;
        PhotonNetwork.room.IsVisible = false;
        PhotonNetwork.LoadLevel(1);
    }

}
