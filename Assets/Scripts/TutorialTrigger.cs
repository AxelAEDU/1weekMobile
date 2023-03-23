using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public GameObject Tutorial;
    public void OnTriggerStay(Collider collide)
    {
        if (collide.gameObject.CompareTag("Player"))
        {
            Tutorial.SetActive(true);
        }
    }
    public void OnTriggerExit(Collider collide)
    {
        if (collide.gameObject.CompareTag("Player"))
        {
            Tutorial.SetActive(false);
        }
    }
}
