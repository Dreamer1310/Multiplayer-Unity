using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Microsoft.AspNetCore.SignalR.Client;
using ServerLayer;

public class Player : MonoBehaviour
{
    private readonly float _velocity = 50.0f;
    private readonly float _moveSpeed = 5.0f;
    private Camera myCamera;

    public static Player Init(GameObject prefab, Vector3 position) // usefull staff...
    {
        var gameObject = Instantiate(prefab, position, Quaternion.identity);
        return gameObject.GetComponent<Player>();
    }


    // Start is called before the first frame update
    void Start()
    {
        AssignCamera();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            var a = gameObject.GetComponentInChildren<Camera>();
            Debug.Log(a);
            Rotate(-1);
        }
        if (Input.GetKey(KeyCode.D))
        {
            Rotate(1);
        }
        if (Input.GetKey(KeyCode.W))
        {
            var forward = GetLookingDirection();
            transform.Translate(forward * _moveSpeed * Time.deltaTime);
        }

    }

    public void Move()
    {

    }

    public Vector3 GetLookingDirection()
    {
        var camera = gameObject.GetComponent<Camera>();
        return camera.transform.forward;
    }

    public void AssignCamera()
    {
        gameObject.AddComponent<Camera>();
    }

    public void Rotate(Int32 direction)
    {
        transform.Rotate(Vector3.up * direction, _velocity * Time.deltaTime, Space.Self);
        //transform.RotateAround(transform.position, direction, 50 * Time.deltaTime);
    }

    public void DestroyThis()
    {
        Destroy(gameObject);
    }
}
