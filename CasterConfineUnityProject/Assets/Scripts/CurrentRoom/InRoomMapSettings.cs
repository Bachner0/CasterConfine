using System.Collections;
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


    //set default custom room properties
    private void Awake()
    {

    }




    public string SetRoomTitle()
    {
        /*
        RoomTitle = rn + "\nMode: " + gm + "   Zone: " + ml + "   Time Limit: " + tl + "m";
                */
        return RoomTitle;

    }

}


