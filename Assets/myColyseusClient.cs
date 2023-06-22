using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Colyseus;
using Colyseus.Schema;
using GameDevWare;
using GameDevWare.Serialization;
using UnityEngine;

public class myColyseusClient : ColyseusManager<myColyseusClient>
{
    public String sessionId;
    public String roomName;
    public GameObject player1;
    public GameObject enemy;
    protected ColyseusClient myClient;
    public ColyseusRoom<State> Room;
    protected IndexedDictionary<Player, GameObject> players =
    new IndexedDictionary<Player, GameObject>();

    void Start()
    {
        player1 = GameObject.Find("myPlayer");
        myClient = new ColyseusClient("ws://localhost:2567");
        JoinOrCreateRoom();
        
    }

    public async void JoinOrCreateRoom()
    {
        try
        {
            Room = await myClient.JoinOrCreate<State>(roomName);
            Debug.Log("Joined Room " + Room.RoomId);

        }
        catch (Exception ex)
        {
            Debug.Log("join error");
            Debug.Log(ex.Message);
        }
    }

    public void RegisterRoomHandlers()
    {
        try
        {
            sessionId = Room.SessionId;
            // https://docs.colyseus.io/migrating/0.15/#colyseusarena-has-been-renamed-to-colyseustools

            Room.State.players.OnAdd(OnPlayerAdd);
            Room.OnLeave += (code) => {
                Debug.Log("Room.OnLeave");
            };

            Room.OnError += (code, message) =>
            {
                Debug.Log("Room.OnError " + code + " " + message);
            };

            Room.OnStateChange += Room_OnStateChange;

            PlayerPrefs.SetString("roomId", Room.RoomId);
            PlayerPrefs.SetString("sessionId", Room.SessionId);
            PlayerPrefs.Save();

        }
        catch (Exception ex){
            Debug.LogError(ex.Message);
        }
    }

    private void Room_OnStateChange(State state, bool isFirstState)
    {
        throw new NotImplementedException();
    }

    void OnPlayerAdd(string key, Player player)
    {
        Debug.Log("OnPlayerAdded");

        if (player.sessionId == sessionId)
        {
            players.Add(player, player1);
            
            Debug.Log("Player 1 added to player list " + Room.RoomId);
        }
        else
        {
            GameObject myEnemy = Instantiate(enemy);
            myEnemy.transform.position = new Vector2(player.xPos,
            player.yPos);
            // Add "enemy" to map of players
            players.Add(player, myEnemy);
            Debug.Log("Enemy added to players list " + player.ownerId);
            player.OnChange(() => { myEnemy.transform.position = new Vector2(player.xPos, player.yPos); });

        }
    }

    
    async void LeaveRoom()
    {
        await Room.Leave(false);
        // Destroy player entities
        foreach (KeyValuePair<Player, GameObject> player in players)
        {
            Destroy(player.Value);
        }
        players.Clear();
    }
    public void OnPlayerMove()
    {
        if (Room != null)
            {
            Room.Send("move", new
            {
                xPos = player1.transform.position.x,
                yPos = player1.transform.position.y
            });
        }
        else
        {
            Debug.Log("Unable to move ! we're not in a room");
        }
    }

}