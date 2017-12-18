using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class PlayerNetwork : MonoBehaviour {

    public static PlayerNetwork Instance;
    public string PlayerName { get; private set; }
    private PhotonView PhotonView;          //we added a photon view component to the player network game object
    private int PlayersInGame = 0;          //to keep track of how many clients
    //private ExitGames.Client.Photon.Hashtable m_playerCustomProperties = new ExitGames.Client.Photon.Hashtable();
    private PlayerMovement CurrentPlayer;   //this is our reference to the player to destroy on death
    //private Coroutine m_pingCoroutine;

    // Use this for initialization
    private void Awake ()
    {
        Instance = this;
        PhotonView = GetComponent<PhotonView>();
        //To identify people
        PlayerName = "Caster #" + Random.Range(1000, 9999);

        // times per second
        //determines how much to push the communication send rate of packets - will result in higher bandwidth if higher, but more accurate
        PhotonNetwork.sendRate = 60;                    //default is 20
        PhotonNetwork.sendRateOnSerialize = 30;         //default is 10


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
        PhotonView.RPC("RPC_LoadedGameScene", PhotonTargets.MasterClient, PhotonNetwork.player);      //note: may need to make the playerList reduce when host leaves.
        //this lets you broadcast a message to others
        PhotonView.RPC("RPC_LoadGameOthers", PhotonTargets.Others);
    }

    private void NonMasterLoadedGame()
    {
        PhotonView.RPC("RPC_LoadedGameScene", PhotonTargets.MasterClient, PhotonNetwork.player);
    }

    [PunRPC]                                //necessary for communicating to the other clients in the room
    private void RPC_LoadGameOthers()
    {
        PhotonNetwork.LoadLevel(1);         //this will need to change when more maps are added  ****
    }

    [PunRPC]
    private void RPC_LoadedGameScene(PhotonPlayer photonPlayer)      //called on master, tally the number of players in game, when the right amount of players, start the game
    {
        PlayerManagement.Instance.AddPlayerStats(photonPlayer);

        PlayersInGame++;
        if (PlayersInGame == PhotonNetwork.playerList.Length)
        {
            print("All players are in the game scene.");
            PhotonView.RPC("RPC_CreatePlayer", PhotonTargets.All);      //called for all new players entering room - it instantiates them with the location and prefab (below)
        }
    }

    //letting the client know of health change
    public void NewHealth(PhotonPlayer photonPlayer, int health)
    {
        //broadcast what we just manipulated on server
        PhotonView.RPC("RPC_NewHealth", photonPlayer, health);
    }
    //make the method from directly above
    [PunRPC]
    private void RPC_NewHealth(int health)
    {
        //if the health is less than or equal to 0, we have to destroy the player, so we need a reference to the player
        if (CurrentPlayer == null)
            return;
        Debug.Log(health);

        //calling a network destroy
        if (health <= 0)
        {
            PhotonNetwork.Destroy(CurrentPlayer.gameObject);        //
        }
        else
        {
         //   CurrentPlayer.Health = health;
        }
    }

    /*
    private IEnumerator C_SetPing()
    {
        while (PhotonNetwork.connected)
        {
            m_playerCustomProperties["Ping"] = PhotonNetwork.GetPing();
            PhotonNetwork.player.SetCustomProperties(m_playerCustomProperties);

            yield return new WaitForSeconds(5f);
        }

        yield break;
    }
    */




    [PunRPC]
    private void RPC_CreatePlayer()
    {
        //creating the player. the vector3 is the location, then rotation, and last setting is for group... not sure what that is
        GameObject obj = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "NewPlayer"), Vector3.zero, Quaternion.identity, 0);
        CurrentPlayer = obj.GetComponent<PlayerMovement>();

    }

}
