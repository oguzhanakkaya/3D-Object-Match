using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public LoadingUI loadingUI;
    public LobbyUI lobbyUI;
    public GameUI gameUI;
    public LevelFailedUI levelFailedUI;
    public LevelWinUI levelWinUI;

    public void Initialize()
    {
        loadingUI.Initialize();
        lobbyUI.Initialize();
        gameUI.Initialize();
        levelFailedUI.Initialize();
        levelWinUI.Initialize();
    }
}
