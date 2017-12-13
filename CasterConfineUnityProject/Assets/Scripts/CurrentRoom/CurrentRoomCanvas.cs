using UnityEngine;
using UnityEngine.UI;

public class CurrentRoomCanvas : MonoBehaviour
{

    /*
        NEED to disable buttons if not the host

    */






    public void OnClickStartDelayed()
    {
        if (!PhotonNetwork.isMasterClient)      //can't click if not host
            return;

        PhotonNetwork.room.IsOpen = false;
        PhotonNetwork.room.IsVisible = false;
        PhotonNetwork.LoadLevel(1);
    }

}


//Use this method to make a match that is continuous...
/*
public void OnClickStartSync()
{
    if (!PhotonNetwork.isMasterClient)      //can't click if not host
        return;

    //loads the scene #..... will want to modify this method based on the room settings
    PhotonNetwork.LoadLevel(1);
}
*/
