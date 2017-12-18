using UnityEngine;
using UnityEngine.UI;

public class PlayerListing : MonoBehaviour
{
    public PhotonPlayer PhotonPlayer { get; private set; }

    [SerializeField]
    private Text _playerName;
    private Text PlayerName { get { return _playerName; } }

    [SerializeField]
    private Text _roomNameText;
    private Text RoomNameText
    {
        get { return _roomNameText; }
    }

    public void ApplyPhotonPlayer(PhotonPlayer photonPlayer)
    {
        PhotonPlayer = photonPlayer;
        PlayerName.text = photonPlayer.NickName;
    }
}
