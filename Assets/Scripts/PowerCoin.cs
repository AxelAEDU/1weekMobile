using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerCoin : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Head")
        {
            Debug.Log("coin");
            //Destroy(other.gameObject);
        }
    }
}
