using UnityEngine;
using UnityEngine.UI;

public class RoomListing : MonoBehaviour {


    [SerializeField]
    private Text _roomNameText;
    private Text RoomNameText
    {
        get { return _roomNameText; }
    }

    public string RoomName { get; private set; }


    public bool Updated { get; set; }

	private void Start ()
    {

        //Getting the Lobby Canvas object

        GameObject LobbyCanvasObj = MainMPCanvasManager.Instance.LobbyCanvas.gameObject;
        if (LobbyCanvasObj == null)
        {
            return;
        }

        LobbyCanvas lobbyCanvas = LobbyCanvasObj.GetComponent<LobbyCanvas>();

        //Get the Room Name text

        Button button = GetComponent<Button>();
        button.onClick.AddListener(() => lobbyCanvas.OnClickJoinRoom(RoomNameText.text));
	}

    private void OnDestroy()
    {
        //destroy something
        Button button = GetComponent<Button>();
        button.onClick.RemoveAllListeners();
    }

    //when called it will set the text from the create room stuff to be the name of the game room
    public void SetRoomNameText(string text)
    {
        RoomName = text;                    // helps to locate potential problem

        RoomNameText.text = RoomName;       // .text is a property of the Text component
    }

}
