using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ServerLayer;
using System;
using System.Linq;
using UnityEngine.Networking;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance;

    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private Transform spawnPoint;

    public GameConnection connection;

    private Dictionary<Int64, GameObject> Players;
    private List<GameObject> _players => Players.Values.ToList();



    private void Awake()
    {
        Instance = this;

        var response = UnityWebRequest.Get("http://localhost:5201/api/login/signin");
        response.

        connection = new GameConnection("http://localhost:5201");
        connection.Client
            .Moved((x, y) =>
            {
                Debug.Log($"x - {x}, y - {y}");
                return 0;
            })
            .MovementError((errorString) =>
            {
                Debug.Log($"errorString - {errorString}");
                return 0;
            })
            .PlayerJoined(() =>
            {


                new Models.Player();
                return 0;
            });

        connection.Start();
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            
        }
    }
}
