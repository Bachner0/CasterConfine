using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagement : MonoBehaviour {

    public static PlayerManagement Instance;    //made it a singleton
    private PhotonView PhotonView;


    private List<PlayerStats> PlayerStats = new List<PlayerStats>();



    private void Awake()
    {
        Instance = this;    //all singletons do this
        PhotonView = GetComponent<PhotonView>();
    }

    public void AddPlayerStats(PhotonPlayer photonPlayer)
    {
        int index = PlayerStats.FindIndex(x => x.PhotonPlayer == photonPlayer); //find the index of the player in the player list
        if (index == -1)
        {
            PlayerStats.Add(new PlayerStats(photonPlayer, 30)); //30 here is the health
        }
    }

    public void ModifyHealth(PhotonPlayer photonPlayer, int value)
    {
        //find the players statistics that we are going to be modifying
        int index = PlayerStats.FindIndex(x => x.PhotonPlayer == photonPlayer);
        if(index != -1)
        {
            PlayerStats playerStats = PlayerStats[index];
            playerStats.Health += value;                        //so now the changes are saved on the server, but need to update to client
                                                                //he wants to put it under player network script - says could put anywhere

            //when health is modified, we need to call new health
            PlayerNetwork.Instance.NewHealth(photonPlayer, playerStats.Health); 
        }
    }

    public void ModifyStamina(PhotonPlayer photonPlayer, int value)
    {

    }

    public void ModifyMana(PhotonPlayer photonPlayer, int value)
    {

    }
}


public class PlayerStats
{
    public PlayerStats(PhotonPlayer photonPlayer, int health)
    {
        PhotonPlayer = photonPlayer;
        Health = health;
        //Stamina = stamina;
        //Mana = mana;
    }

    public readonly PhotonPlayer PhotonPlayer;

    public int Health;
    public int Stamina;
    public int Mana;
}