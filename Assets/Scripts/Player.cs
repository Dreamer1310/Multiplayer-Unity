using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Microsoft.AspNetCore.SignalR.Client;
using ServerLayer;

public class Player : MonoBehaviour
{
    public Int64 ID { get; set; }
    public GameConnection connection => GameSceneManager.Instance.connection;
    // Start is called before the first frame update

    public Player()
    {
        ID = 1;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Debug.Log("here W");
        }

        //if (Input.GetKey(KeyCode.A))
        //{
        //    connection.InvokeAsync("MoveLeft");
        //}

        //if (Input.GetKey(KeyCode.S))
        //{
        //    connection.InvokeAsync("MoveBackward");
        //}

        //if (Input.GetKey(KeyCode.D))
        //{
        //    connection.InvokeAsync("MoveRight");
        //}
    }
}
