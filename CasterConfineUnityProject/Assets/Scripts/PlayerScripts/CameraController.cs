using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // do not change variables
    [Tooltip("The Smooth time lag for the height of the camera.")]
    public float heightSmoothLag = 0.3f;
    [Tooltip("Set this as false if a component of a prefab being instanciated by Photon Network, and manually call OnStartFollowing() when and if needed.")]
    public bool followOnStart = false;
    // cached transform of the target
    Transform cameraTransform;
    // maintain a flag internally to reconnect if target is lost or camera is switched
    bool isFollowing;

    //regular variables







    



    void Start()
    {
        // Start following the target if wanted.
        if (followOnStart)
        {
            OnStartFollowing();
        }
    }

    private void Update()
    {

    }

    void LateUpdate()
    {
        // The transform target may not destroy on level load, 
        // so we need to cover corner cases where the Main Camera is different everytime we load a new scene, and reconnect when that happens
        if (cameraTransform == null && isFollowing)
        {
            OnStartFollowing();
        }
        // only follow is explicitly declared
        if (isFollowing)
        {
            Apply();
        }
    }

    public void OnStartFollowing()
    {
        cameraTransform = Camera.main.transform;
        isFollowing = true;
        // we don't smooth anything, we go straight to the right camera shot
        Cut();
    }

    /// <summary>
    /// This is what goes where LateUpdate stuff goes
    /// </summary>
    void Apply()
    {

    }

    /// <summary>
    /// Directly position the camera to a the specified Target and center.
    /// </summary>
    void Cut()
    {
        float oldHeightSmooth = heightSmoothLag;
        heightSmoothLag = 0.001f;
        Apply();
        heightSmoothLag = oldHeightSmooth;
    }
}

