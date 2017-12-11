using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyNetwork : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        print("Connecting to server");

        PhotonNetwork.ConnectUsingSettings("0.0.0.0");  //could be used for connecting by version
	}
	
    private void OnConnectedToMaster()
    {
        print("Connected to master");

        //whenever connected to room, will sync to whatever scene the master client is currently on
        PhotonNetwork.automaticallySyncScene = false;

        //Setting player name
        PhotonNetwork.playerName = PlayerNetwork.Instance.PlayerName;

        //Join lobby
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    private void OnJoinedLobby()
    {
        print("Joined lobby");

        if (!PhotonNetwork.inRoom)
        {
            //when you join lobby, show the lobby panel
            MainMPCanvasManager.Instance.LobbyCanvas.transform.SetAsLastSibling();
        }
    }


}
