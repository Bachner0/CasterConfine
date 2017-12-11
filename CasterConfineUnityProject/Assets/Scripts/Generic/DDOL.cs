using UnityEngine;

public class DDOL : MonoBehaviour {

	// Use this for initialization
	private void Awake ()
    {

        //Anything on this object will not be destroyed. Important to keep data across scene changes.

        DontDestroyOnLoad(this);

	}
	
}
