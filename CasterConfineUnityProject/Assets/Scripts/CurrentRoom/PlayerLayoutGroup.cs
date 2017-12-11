using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLayoutGroup : MonoBehaviour
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


    //called whenever the master leaves and called on all players - this would boot all players if the host leaves
    /*
    private void OnMasterClientSwitched(PhotonPlayer newMasterClient)
    {
        PhotonNetwork.LeaveRoom();
    }
    */

        //    *********  we have a problem where host name stays in room if host leaves.


    //called by photon whenever you join a room.
    private void OnJoinedRoom()
    {
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
        if (index!= -1)
        {
            Destroy(PlayerListings[index].gameObject);
            //remove object script reference from list
            PlayerListings.RemoveAt(index);

        }
    }

    [SerializeField]
    private Text _roomStateText;
    private Text RoomStateText
    {
        get { return _roomStateText; }
    }
    public string RoomState { get; private set; }



    //the toggle button method
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

    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    //when called it will set the text from the create room stuff to be the name of the game room
    public void SetRoomStateText(string text)
    {
        RoomState = text;                    // helps to locate potential problem
        RoomStateText.text = RoomState;       // .text is a property of the Text component
    }
}
