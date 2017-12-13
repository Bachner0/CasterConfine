using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoom : MonoBehaviour
{

    private string RoomName;
    private string RoomTitle;
    private int RoomPlayerCount;
    private string RoomMapName;
    private int RoomTimeLimit;
    //private bool RoomSpectators;

    public void OnClick_CreateRoom()
    {
        //Default room settings
        //Name: Relic Spire #random
        //PlayerCount: 2 (1v1)
        //Map: ArenaOne
        //TimeLimit: 10 mins
        //AllowSpectators: false
        RoomTitle = "Relic Spire #" + Random.Range(1000, 9999);
        RoomPlayerCount = 2;
        RoomMapName = "ArenaOne";
        RoomTimeLimit = 10;

        RoomName = RoomTitle + "\nPlayers: " + RoomPlayerCount + "   Zone: " + RoomMapName + "   Time Limit: " + RoomTimeLimit + "m";

        RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 2 };

        if (PhotonNetwork.CreateRoom(RoomName, roomOptions, TypedLobby.Default))        //tell photon to create the room
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
