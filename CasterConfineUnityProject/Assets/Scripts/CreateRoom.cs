using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoom : MonoBehaviour
{

    private string RoomName;

    public string RoomTitle;
    public int RoomPlayerCount;
    public string RoomMapName;
    public int RoomTimeLimit;
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

        /*
         * Making the room invisible is a good way to hide it from randomly joining players, yes.
You shouldn't change the room's name though. It's the ID by which we find and handle the room.
Better: Use a room property to store the current scenery name. The props are available for new players before events come in, so you can pause the message queue and load the scenery, before you turn on the queue again when you loaded.
PhotonNetwork.LoadLevel and PhotonNetwork.automaticallySyncScene = true will do this for you, too.
*/



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
