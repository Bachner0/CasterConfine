using UnityEngine;
using UnityEngine.UI;

public class LobbyCanvas : MonoBehaviour {

    [SerializeField]
    private RoomLayoutGroup _roomLayoutGroup;
    private RoomLayoutGroup RoomLayoutGroup
    {
        get { return _roomLayoutGroup; }
    }

    [SerializeField]
    private Button StartMatchButton;
    [SerializeField]
    private Dropdown PlayerCount;
    [SerializeField]
    private Dropdown MapSelection;
    [SerializeField]
    private Dropdown TimeLimit;
    [SerializeField]
    private Dropdown Spectators;
    [SerializeField]
    private Button KickPlayer;



    public void RoomOptionsPermissions()
    {
        if (!PhotonNetwork.isMasterClient)      //can't click if not host
        {
            // the true settings are in PlayerLayoutGroup
            StartMatchButton.enabled = false;
            PlayerCount.enabled = false;
            MapSelection.enabled = false;
            TimeLimit.enabled = false;
            Spectators.enabled = false;
            KickPlayer.enabled = false;
            KickPlayer.image.enabled = false;
            KickPlayer.GetComponentInChildren<Text>().enabled = false;
        }
        return;
    }

    public void OnClickJoinRoom(string roomName)
    {

        if (PhotonNetwork.JoinRoom(roomName))
        {
            RoomOptionsPermissions();
        }
        else
        {
            print("Join room failed.");
        }
    }

}