using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager Instance { get; private set; }
    void Awake()
    {
        //If the instance of GameManger exists it will not destroy it
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        //If the instance of GameManger exists it will destroy the old one
        else if (Instance == this)
        {
            Destroy(this);
            return;
        }

    }

    public void StartGame()
    {
        //Will load the level
        SceneManager.LoadScene("Level", LoadSceneMode.Single);
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
