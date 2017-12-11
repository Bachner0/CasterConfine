using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerNetwork : MonoBehaviour {

    public static PlayerNetwork Instance;
    public string PlayerName { get; private set; }
    private PhotonView PhotonView;          //we added a photon view component to the player network game object
    private int PlayersInGame = 0;          //to keep track of how many clients

	// Use this for initialization
	private void Awake ()
    {
        Instance = this;
        PhotonView = GetComponent<PhotonView>();
        //To identify people
        PlayerName = "Matt#" + Random.Range(1000, 9999);

        SceneManager.sceneLoaded += OnSceneFinishedLoading;     //creates a delegate the occurs when a scene is loaded that calls the below method.
	}
	
    private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        //check to see if player is master or not
        if(scene.name == "ArenaOne")           //this will need to change when more maps are added  *****
        {
            if (PhotonNetwork.isMasterClient)
                MasterLoadedGame();
            else
                NonMasterLoadedGame();
        }
    }

    private void MasterLoadedGame()
    {
        PhotonView.RPC("RPC_LoadedGameScene", PhotonTargets.MasterClient);      //note: may need to make the playerList reduce when host leaves.
        //this lets you broadcast a message to others
        PhotonView.RPC("RPC_LoadGameOthers", PhotonTargets.Others);
    }

    private void NonMasterLoadedGame()
    {
        PhotonView.RPC("RPC_LoadedGameScene", PhotonTargets.MasterClient);
    }

    [PunRPC]       //necessary for communicating to the other clients in the room
    private void RPC_LoadGameOthers()
    {
        PhotonNetwork.LoadLevel(1);       //this will need to change when more maps are added  ****
    }

    [PunRPC]
    private void RPC_LoadedGameScene()      //called on master, tally the number of players in game, when the right amount of players, start the game
    {
        PlayersInGame++;
        if (PlayersInGame == PhotonNetwork.playerList.Length)
        {
            print("All players are in the game scene.");
        }
    }
}
