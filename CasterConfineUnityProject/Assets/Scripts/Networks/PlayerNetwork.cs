using UnityEngine;

public class PlayerNetwork : MonoBehaviour {

    public static PlayerNetwork Instance;
    public string PlayerName { get; private set; }

	// Use this for initialization
	private void Awake ()
    {
        Instance = this;

        //To identify people
        PlayerName = "Matt#" + Random.Range(1000, 9999);
	}
	

}
