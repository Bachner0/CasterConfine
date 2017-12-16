using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CurrentRoomCanvas : MonoBehaviour, ISelectHandler, IDeselectHandler
{


    //start match function
    public void OnClickStartMatch()
    {
        if (!PhotonNetwork.isMasterClient)      //can't click if not host
            return;


        PhotonNetwork.room.IsOpen = false;
        PhotonNetwork.room.IsVisible = false;
        PhotonNetwork.LoadLevel(1);
    }



    // How to move from one team to the other?
    // Is this how you select a button and them give a command to it?

    public void OnSelect(BaseEventData eventData)
    {
        //base.OnSelect(eventData);
        Debug.Log("Selected");

    }

    public void OnDeselect(BaseEventData eventData)
    {
        //base.OnDeselect(eventData);
        UnityEngine.Debug.Log("De-Selected");
    }


    public void OnClickMoveToTeamLeft()
    {

        //if not master, and not already on left side, move to the left side
        //if master, must click on your own name to move and click on other players to move them
        //remove from right list
        //set as team left
    }

    public void OnClickMoveToTeamRight()
    {
        //if not master, and not already on right side, move to the right side
        //if master, must click on your own name to move and click on other players to move them
        //remove from left list
        //set as team right
    }

    public void OnClickMoveToSpectate()
    {
        //if spectate setting is on
        //if not master cannot spectate
        //if master, must click on other players to move them
        //remove from left list/right list
        //set as team spectate
    }

    public void OnSelectedPlayer()
    {

    }


    public void OnClickKickPlayer()
    {
        if (!PhotonNetwork.isMasterClient)      //make it so you can't kick yourself
            return;
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
