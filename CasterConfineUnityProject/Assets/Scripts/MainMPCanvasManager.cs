﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMPCanvasManager : MonoBehaviour
{

    public static MainMPCanvasManager Instance;

    [SerializeField]
    private LobbyCanvas _lobbyCanvas;
    public LobbyCanvas LobbyCanvas
    {
        get { return _lobbyCanvas; }
    }

    private void Awake()
    {
        Instance = this;
    }



}
