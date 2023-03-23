using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EndScreen : MonoBehaviour
{
    public GameObject Endscreen;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("die");
            Endscreen.SetActive(true);
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            //Destroy(other.gameObject);
        }
    }
}
