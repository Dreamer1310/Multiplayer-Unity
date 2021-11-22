using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ServerLayer;
using System;
using System.Linq;
using UnityEngine.Networking;
using System.Net.Http;
using System.Net;
using Assets.Scripts.Models;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance;
    private Int64 _myID { get; set; }

    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private Transform spawnPoint;

    public GameConnection connection;

    private Dictionary<Int64, MyPlayer> Players = new Dictionary<Int64, MyPlayer>();
    private List<MyPlayer> _players => Players.Values.ToList();



    private void Awake()
    {
        Instance = this;
        var url = "http://localhost:5201/api/login/signin";
        Cookie c = new Cookie();
        CookieContainer cookies = new CookieContainer();
        HttpClientHandler handler = new HttpClientHandler();
        handler.CookieContainer = cookies;
        HttpClient client = new HttpClient(handler);
        //var content = new FormUrlEncodedContent(new[] { parameters });
        client.GetAsync(url).GetAwaiter().GetResult();
        Uri uri = new Uri(url);
        IEnumerable<Cookie> responseCookies = cookies.GetCookies(uri).Cast<Cookie>();
        Debug.Log("GetCookies()");
        foreach (Cookie cookie in responseCookies)
        {
            Debug.Log("cookie - " + cookie.Name);
            if (cookie.Name != ".Auth.Unity.Server") continue;

            c = cookie;
            Debug.Log("cookie.Value - " + cookie.Value);
            PlayerPrefs.SetString("cookieName", cookie.ToString().Split('=')[0]);
            PlayerPrefs.SetString("cookieValue", cookie.ToString().Split('=')[1]);
            PlayerPrefs.SetString("cookieDomain", cookie.Domain);
        }

        connection = new GameConnection("http://localhost:5201", c);
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
            .PlayerJoined((player) =>
            {
                _myID = player.ID;
                var newPlayer = new MyPlayer(player, playerPrefab);
                Players.Add(newPlayer.ID, newPlayer);
                return 0;
            })
            .PlayerDisconnected((playerID) =>
            {
                try
                {
                    var player = Players[playerID];
                    player.Destroy();
                    Players.Remove(playerID);
                    return 0;
                }
                catch (Exception ex)
                {
                    Debug.Log(ex);
                    throw;
                }
                
            })
            .State((state) =>
            {
                state.Players.ForEach(x =>
                {
                    var newPlayer = new MyPlayer(x, playerPrefab);
                    Players.Add(newPlayer.ID, newPlayer);
                });


                return 0;
            })
            .PlayerData((player) =>
            {
                _myID = player.ID;
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
        //if (Input.GetKey(KeyCode.D))
        //{
        //    Players[_myID].Rotate(Vector3.right);
        //    //connection.Server.Disconnect();
        //}

        //if (Input.GetKey(KeyCode.A))
        //{
        //    Players[_myID].Rotate(Vector3.left);
        //}
    }
}
