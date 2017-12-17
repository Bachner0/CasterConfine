using System.Collections.Generic;
using UnityEngine;

public class RoomLayoutGroup : MonoBehaviour
{
    [SerializeField]
    private GameObject _roomListingPrefab;
    private GameObject RoomListingPrefab
    {
        get { return _roomListingPrefab; }
    }

    private List<RoomListing> _roomListingButtons = new List<RoomListing>();
    private List<RoomListing> RoomListingButtons
    {
        get { return _roomListingButtons; }
    }


    //get room update from the photon server
    private void OnReceivedRoomListUpdate()
    {
        RoomInfo[] rooms = PhotonNetwork.GetRoomList();

        foreach (RoomInfo room in rooms)
        {
            RoomReceived(room);
        }
        RemoveOldRooms();
    }

    //Update any rooms listings, add room listings
    private void RoomReceived(RoomInfo room)
    {
        //does the button already exist?
        int index = RoomListingButtons.FindIndex(x => x.RoomName == room.Name);     //it's going to look through all the room names and compare it.
                                                                                    //if it finds a match, it will return a value other than -1
                                                                                    //if it returns -1 it could not be found
        if (index == -1)
        {
            //create button and add it to the list
            if(room.IsVisible && room.PlayerCount < room.MaxPlayers)                //check for invisible rooms and full rooms
            {
                GameObject roomListingObj = Instantiate(RoomListingPrefab);
                roomListingObj.transform.SetParent(transform, false);               //setting the parent of the object we just made to the transform of the owner of this script
                                                                                    //so it aligns

                RoomListing roomListing = roomListingObj.GetComponent<RoomListing>();
                RoomListingButtons.Add(roomListing);

                index = (RoomListingButtons.Count - 1);     //updates the creation in the list
            }
        }

        if (index != -1)
        {
            RoomListing roomListing = RoomListingButtons[index];    //pull it from the index that it found earlier
            Debug.Log(room.Name);
            roomListing.SetRoomNameText(room.Name);                 //update the name, it may have changed
            roomListing.Updated = true;
        }
    }

    //remove old rooms
    private void RemoveOldRooms()
    {
        List<RoomListing> removeRooms = new List<RoomListing>();        //create a list for rooms to be destroyed

        foreach (RoomListing roomListing in RoomListingButtons)
        {
            if (!roomListing.Updated)       //checks for rooms that existed but did not get updated
            {
                removeRooms.Add(roomListing);
            }
            else
            {
                roomListing.Updated = false;
            }
        }

        foreach (RoomListing roomListing in removeRooms)
        {
            GameObject roomListingObj = roomListing.gameObject;         //stores the room object
            RoomListingButtons.Remove(roomListing);                     //removes the button
            DestroyObject(roomListingObj);                              //destroys the room listing object
        }
    }


}
