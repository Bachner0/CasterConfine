using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public void PlayerSingleplayerGame()
    {
        SceneManager.LoadScene("ArenaOne");
    }

    public void MultiplayerGame()
    {
        SceneManager.LoadScene("MultiplayerMainLobby");
    }

    public void QuiteGame()
    {
        Debug.Log("Quit game button pressed.");
        Application.Quit();
    }

}
