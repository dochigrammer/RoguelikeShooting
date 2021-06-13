using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyControl : MonoBehaviour
{
    public void OnEnterExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OnEnterGameLevel()
    {
        SceneManager.LoadScene("Shooting");
    }

    public void OnEnterMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
