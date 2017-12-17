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
            StartMatchButton.GetComponent<Button>().interactable = false;
            PlayerCount.GetComponent<Dropdown>().interactable = false;
            MapSelection.GetComponent<Dropdown>().interactable = false;
            TimeLimit.GetComponent<Dropdown>().interactable = false;
            Spectators.GetComponent<Dropdown>().interactable = false;
            KickPlayer.GetComponent<Button>().enabled = false;
            KickPlayer.GetComponent<Button>().image.enabled = false;
            KickPlayer.GetComponent<Button>().GetComponentInChildren<Text>().enabled = false;
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
