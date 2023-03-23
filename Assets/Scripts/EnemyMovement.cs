using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    //[SerializeField] private GameObject Spiner;
    public float Speed = 1f;
    public bool length;
    public Vector3 StartPos;
    public Vector3 EndPos;




    void Update()
    {
        //Will pinpong between Start and End position and Rotating the spinner
        //float time = Mathf.PingPong(Time.deltaTime * Speed, 1);
        //transform.position = Vector3.Lerp(StartPos, EndPos, time * Time.deltaTime);
        //transform.position = new Vector3(transform.position.x + (Mathf.PingPong(Time.time * Speed, length) - 0.5 * length), transform.position.y, transform.position.z);
        //transform.Rotate(0, 0, 0 * Time.deltaTime * Speed);
    }

}
