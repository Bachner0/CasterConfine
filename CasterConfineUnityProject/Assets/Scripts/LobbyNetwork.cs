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

        //Setting player name
        PhotonNetwork.playerName = PlayerNetwork.Instance.PlayerName;

        //Join lobby
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    private void OnJoinedLobby()
    {
        print("Joined lobby");
    }


}
