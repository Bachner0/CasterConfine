using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InRoomMapSettings : MonoBehaviour 
{
    //https://www.youtube.com/watch?v=zbNxrGl4nfc&t=876s  submitting a form


    //game mode
    public string GM;
    public enum GameMode { Teams, FreeForAll }

    //map level
    public string ML;
    public enum MapLevel { ArenaOne, ArenaTwo, ArenaThree }

    //time limit
    public string TL;
    public enum TimeLimit { Five, Ten, Fifteen, Twenty }

    //room name
    public string RT;
    public string RoomTitle;


    public Dropdown GameModeDD;
    public Dropdown MapDD;
    public Dropdown TimeLimitDD;
    public Dropdown SpectatorDD;
    List<string> gameModeList = new List<string>() { "Teams", "FFA" };
    List<string> mapLevelList = new List<string>() { "ArenaOne", "ArenaTwo", "ArenaThree" };
    List<string> timeLimitList = new List<string>() { "5 minutes", "10 minutes", "15 minutes", "20 minutes" };
    List<string> spectatorsList = new List<string>() { "Allowed", "None" };


    public void GameModeDropdown_IndexChanged(int index)
    {
        ExitGames.Client.Photon.Hashtable setHash = new ExitGames.Client.Photon.Hashtable
            {
                { RoomProperty.GameMode, index}
            };
        PhotonNetwork.room.SetCustomProperties(setHash);
        // checking to see if this is working
        /*
        Debug.Log("Changed game mode setting to" + gameModeList[index]);
        foreach (var i in PhotonNetwork.room.CustomProperties)
        {
            Debug.Log((string)i.Key + i.Value);
        }
        */
    }

    public void MapDropdown_IndexChanged(int index)
    {
        ExitGames.Client.Photon.Hashtable setHash = new ExitGames.Client.Photon.Hashtable
            {
                { RoomProperty.MapLevel, gameModeList[index]}
            };
        PhotonNetwork.room.SetCustomProperties(setHash);
    }

    public void TimeLimitDropdown_IndexChanged(int index)
    {
        ExitGames.Client.Photon.Hashtable setHash = new ExitGames.Client.Photon.Hashtable
            {
                { RoomProperty.TimeLimit, index}
            };
        PhotonNetwork.room.SetCustomProperties(setHash);
    }

    public void SpectatorDropdown_IndexChanged(int index)
    {
        ExitGames.Client.Photon.Hashtable setHash = new ExitGames.Client.Photon.Hashtable
            {
                { RoomProperty.SpectatorsSetting, index}
            };
        PhotonNetwork.room.SetCustomProperties(setHash);
    }


    //set default custom room properties
    void Start()
    {
        PopulateList();
    }
    /*
    * Don't know how to update the room member's dropdowns
    * 
    * 
       if (!PhotonNetwork.isMasterClient)      //can't click if not host
       {
           GameModeDD.onValueChanged.AddListener(delegate { SetGameModeText(GameModeDD.value); });
       }


   void SetGameModeText(int i)
   {
       GameModeDD.GetComponent<Dropdown>().interactable = true;
       Debug.Log(GameModeDD.value);
       GameModeDD.value = i;

       //GameModeDD.GetComponent<Dropdown>().interactable = false;
   }
   */

        /*  remove the listeners
        private void OnDestroy()
    {
        //destroy something
        Button button = GetComponent<Button>();
        button.onClick.RemoveAllListeners();
    }


        */

        void PopulateList()
    {

        GameModeDD.AddOptions(gameModeList);
        MapDD.AddOptions(mapLevelList);
        TimeLimitDD.AddOptions(timeLimitList);
        SpectatorDD.AddOptions(spectatorsList);
    }
}


