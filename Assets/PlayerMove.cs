using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Colyseus;
public class PlayerMove : MonoBehaviour
{

    /*
     *  FIELDS
    */

    public float speed = 1f;
    Rigidbody2D Player;
    public myColyseusClient myClient;



    /*
     *  FUNCTIONS 
    */

    // Start is called before the first frame update
    void Start()
    {
        Player = GetComponent<Rigidbody2D>();
        GameObject.Find("NetworkClient").GetComponent<myColyseusClient>().player1 = GameObject.Find("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // get input
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        // calculate velocity of player
        Player.velocity = new Vector2 (moveX * speed, moveY * speed);
        if (Player.velocity != Vector2.zero)
        {
            myClient.OnPlayerMove();
        }

    }
}
