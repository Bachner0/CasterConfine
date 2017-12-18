using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoom : MonoBehaviour
{

    [SerializeField]
    private Text _roomName;
    private Text RoomName
    {
        get { return _roomName; }
    }

    public void OnClick_CreateRoom()
    {
        //tell photon to create the room
        if (PhotonNetwork.CreateRoom(RoomName.text, new RoomOptions()
        {
            //Don't think we need this because we're transfering the info through the text stuff
            //CustomRoomPropertiesForLobby = new string[]
            //{
            //    RoomProperty.RoomNameTitle,
            //    RoomProperty.GameMode
            //},
            CustomRoomProperties = new ExitGames.Client.Photon.Hashtable
            {
                { RoomProperty.GameMode, 0},
                { RoomProperty.MapLevel, 1},
                { RoomProperty.TimeLimit, 2},
                { RoomProperty.SpectatorsSetting, 0},
                { RoomProperty.TeamAScore, 0},
                { RoomProperty.TeamBScore, 0}
            },
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = 14
            },
            TypedLobby.Default))        
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

public class RoomProperty
{
    public const string GameMode = "G";
    public const string MapLevel = "M";
    public const string TimeLimit = "T";
    public const string SpectatorsSetting = "S";
    public const string TeamAScore = "A";
    public const string TeamBScore = "B";

}