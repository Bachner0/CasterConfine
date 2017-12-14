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
    private InputField RoomTitle;
    [SerializeField]
    private Button TitleSet;
    [SerializeField]
    private Dropdown PlayerCount;
    [SerializeField]
    private Dropdown MapSelection;
    [SerializeField]
    private Dropdown TimeLimit;



    public void RoomOptionsPermissions()
    {
        if (PhotonNetwork.isMasterClient)      //can't click if not host
        {
            StartMatchButton.GetComponent<Button>().interactable = true;
            RoomTitle.GetComponent<InputField>().interactable = true;
            TitleSet.GetComponent<Button>().interactable = true;
            PlayerCount.GetComponent<Dropdown>().interactable = true;
            MapSelection.GetComponent<Dropdown>().interactable = true;
            TimeLimit.GetComponent<Dropdown>().interactable = true;
        }
        else
        {
            StartMatchButton.GetComponent<Button>().interactable = false;
            RoomTitle.GetComponent<InputField>().interactable = false;
            TitleSet.GetComponent<Button>().interactable = false;
            PlayerCount.GetComponent<Dropdown>().interactable = false;
            MapSelection.GetComponent<Dropdown>().interactable = false;
            TimeLimit.GetComponent<Dropdown>().interactable = false;
        }
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
