using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerSettings : Photon.PunBehaviour
{



    /// <summary>
    /// First, it gets the CameraWork component, we expect this, so if we don't find it, we log an error. 
    /// Then, if photonView.isMine is true, it means we need to follow this instance, and so we call _cameraWork.OnStartFollowing()
    /// which effectivly makes the camera follow that very instance in the scene.
    /// </summary>
    private void Start()
    {
        CameraController _cameraController = this.gameObject.GetComponent<CameraController>();
        Canvas _playerUIcanvas = this.gameObject.GetComponentInChildren<Canvas>();

        if (_cameraController != null)
        {
            if (photonView.isMine)
            {
                _cameraController.OnStartFollowing();
                _playerUIcanvas.enabled = true;
            }
        }
        else
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
