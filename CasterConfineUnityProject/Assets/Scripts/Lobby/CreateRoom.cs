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
        //RoomName = "Relic Spire #" + Random.Range(10, 9999);

        //RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 14 };


        //this gives key conflict error
        /*
        ExitGames.Client.Photon.Hashtable CustomRoomOpts = new ExitGames.Client.Photon.Hashtable();

        
        roomOptions.CustomRoomProperties.Add(RoomProperty.RoomNameTitle, RoomName);
        roomOptions.CustomRoomProperties.Add(RoomProperty.GameMode, "Teams");
        roomOptions.CustomRoomProperties.Add(RoomProperty.Map, "ArenaOne");
        roomOptions.CustomRoomProperties.Add(RoomProperty.MapIndex, 1);
        roomOptions.CustomRoomProperties.Add(RoomProperty.TimeLimit, "Ten");
        roomOptions.CustomRoomProperties.Add(RoomProperty.SpectatorsSetting, "Allowed");
        roomOptions.CustomRoomProperties.Add(RoomProperty.TeamAScore, 0);
        roomOptions.CustomRoomProperties.Add(RoomProperty.TeamBScore, 0);

        roomOptions.CustomRoomPropertiesForLobby = new string[]
        {
            RoomProperty.RoomNameTitle,
            RoomProperty.GameMode
        };
        */


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
                { RoomProperty.GameMode, "Teams"},
                { RoomProperty.Map, "ArenaOne"},
                { RoomProperty.MapIndex, 1},
                { RoomProperty.TimeLimit, "Ten"},
                { RoomProperty.SpectatorsSetting, "Allowed"},
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
    public const string GameMode = "GM";
    public const string Map = "MP";
    public const string MapIndex = "MI";
    public const string TimeLimit = "TL";
    public const string SpectatorsSetting = "SS";
    public const string TeamAScore = "AS";
    public const string TeamBScore = "BS";

}