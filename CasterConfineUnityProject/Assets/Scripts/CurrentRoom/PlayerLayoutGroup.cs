using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerLayoutGroup : MonoBehaviour, ISelectHandler, IDeselectHandler

{

    [SerializeField]
    private GameObject _playerListingPrefab;
    private GameObject PlayerListingPrefab
    {
        get { return _playerListingPrefab; }
    }

    //make a list to store all the players in the room
    private List<PlayerListing> _playerListings = new List<PlayerListing>();
    private List<PlayerListing> PlayerListings
    {
        get { return _playerListings; }
    }

    [SerializeField]
    private Button StartMatchButton;
    [SerializeField]
    private InputField RoomTitleField;
    [SerializeField]
    private Button TitleSet;
    [SerializeField]
    private Dropdown PlayerCount;
    [SerializeField]
    private Dropdown MapSelection;
    [SerializeField]
    private Dropdown TimeLimit;
    [SerializeField]
    private Button KickPlayer;


    //called whenever the master leaves and called on all players - this would boot all players if the host leaves
    /*
    private void OnMasterClientSwitched(PhotonPlayer newMasterClient)
    {
        PhotonNetwork.LeaveRoom();
    }
    */


    //called by photon whenever you join a room.
    private void OnJoinedRoom()
    {
        //this makes the duplicates from host leaving not happen.
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        //Makes the CurrentRoomCanvas UI screen come to the front (hides lobby)
        MainMPCanvasManager.Instance.CurrentRoomCanvas.transform.SetAsLastSibling();


        PhotonPlayer[] photonPlayers = PhotonNetwork.playerList;
        for (int i = 0; i < photonPlayers.Length; i++)
        {
            PlayerJoinedRoom(photonPlayers[i]);
        }
    }

    //add photon call when the player joins the room
    private void OnPhotonPlayerConnected(PhotonPlayer photonPlayer)
    {
        PlayerJoinedRoom(photonPlayer);
    }

    //add photon call back when the player leaves the room
    private void OnPhotonPlayerDisconnected(PhotonPlayer photonPlayer)
    {
        PlayerLeftRoom(photonPlayer);
    }


    private void PlayerJoinedRoom(PhotonPlayer photonPlayer)
    {
        if (photonPlayer == null)       //prevents duplicates
            return;

        PlayerLeftRoom(photonPlayer);

        GameObject playerListingObj = Instantiate(PlayerListingPrefab);
        playerListingObj.transform.SetParent(transform, false);

        PlayerListing playerListing = playerListingObj.GetComponent<PlayerListing>();       //get the playerlisting script from this gameobject
        //pass in the photon player
        playerListing.ApplyPhotonPlayer(photonPlayer);

        PlayerListings.Add(playerListing);
    }

    private void PlayerLeftRoom(PhotonPlayer photonPlayer)
    {
        //try and find the player and the existing player listings list, get by index
        int index = PlayerListings.FindIndex(x => x.PhotonPlayer == photonPlayer);
        if (index != -1)
        {
            Destroy(PlayerListings[index].gameObject);
            //remove object script reference from list
            PlayerListings.RemoveAt(index);

        }
    }



    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        ResetRoomOptionsPermissions();

    }

    public void ResetRoomOptionsPermissions()
    {
        StartMatchButton.interactable = true;
        RoomTitleField.interactable = true;
        TitleSet.interactable = true;
        PlayerCount.interactable = true;
        MapSelection.interactable = true;
        TimeLimit.interactable = true;
        KickPlayer.enabled = true;
        KickPlayer.image.enabled = true;
        KickPlayer.GetComponentInChildren<Text>().enabled = true;
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





    enum Team { teamLeft, teamRight, teamFFA, spectator };
}



/*
[SerializeField]
private Text _roomStateText;
private Text RoomStateText
{
    get { return _roomStateText; }
}
public string RoomState { get; private set; }
*/


//the toggle button method for making the room open or closed
/*
public void OnClickRoomState()
{
    //only want the room host (master client) to be able to do this
    if (!PhotonNetwork.isMasterClient)
        return;

    //these are bool values. so if it's open, it's visible and vice versa
    PhotonNetwork.room.IsOpen = !PhotonNetwork.room.IsOpen;
    PhotonNetwork.room.IsVisible = PhotonNetwork.room.IsOpen;
    if (!PhotonNetwork.room.IsOpen)
    {
        SetRoomStateText("Closed Room");                                        //**** somehow update this for the people in the room
    }
    else
    {
        SetRoomStateText("Open Room");
    }
}
*/


//this might not be used anymore
//when called it will set the text from the create room stuff to be the name of the game room
/*
public void SetRoomStateText(string text)
{
    RoomState = text;                    // helps to locate potential problem
    RoomStateText.text = RoomState;       // .text is a property of the Text component
}
*/
