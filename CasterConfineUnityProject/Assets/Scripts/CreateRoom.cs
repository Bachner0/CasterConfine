using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoom : MonoBehaviour
{

    [SerializeField]
    private Text _roomName; //accessor
    private Text RoomName
    {
        get { return _roomName; }
    }

    public void OnClick_CreateRoom()
    {
        //Define room settings................**** come back to expand this to give player ability to modify settings *****

        RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 4 };
        

        if (PhotonNetwork.CreateRoom(RoomName.text, roomOptions, TypedLobby.Default))        //tell photon to create the room
        {
            print("Create room successfully sent");
        }
        else
        {
            print("Create room failed to send");    //If this shows, you are not connected to server when it called create room
        }
    }

    private void OnPhotonCreateRoomFailed(object[] codeAndMessage)
    {
        print("Create room failed: " + codeAndMessage[1]);
    }

    private void OnCreatedRoom()
    {
        print("Room created successfully");
    }

}
