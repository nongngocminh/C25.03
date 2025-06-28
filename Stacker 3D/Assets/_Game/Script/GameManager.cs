using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static int playerLevel = 0;
    public static bool winState = false;

    // Start is called before the first frame update
    void Start()
    {
        LoadPlayerData();
    }

    void Update()
    {
        if (winState)
        {
            SceneManager.LoadScene(playerLevel, LoadSceneMode.Single);
            winState = false;
        }
    }

    public static void SavePlayerData()
    {
        PlayerPrefs.SetInt("PlayerLevel", playerLevel);
        PlayerPrefs.Save();
        Debug.Log("Data Saved");
    }

    public static void LoadPlayerData()
    {
        playerLevel = PlayerPrefs.GetInt("PlayerLevel");
        Debug.Log("Data Loaded");
    }
}
