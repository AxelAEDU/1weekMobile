using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This script control so that music dont destroy on load a new scene, if an one more scene with music load it will destroy it
/// </summary>
public class Music : MonoBehaviour
{
    GameObject[] Sound;
    private void Awake()
    {
        //Finding Object with Tag Music
        Sound = GameObject.FindGameObjectsWithTag("Music");
        //Will not destroy on load
        DontDestroyOnLoad(gameObject);
        //If i have more then 1 Object tag with Music it will destroy it
        if (Sound.Length > 1)
        {
            Destroy(gameObject);
        }
    }
}
