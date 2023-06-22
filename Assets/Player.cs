/*
 
 The Player.cs file describes the basic data that will be passed from the
server to the client. It includes identification information on each player
that is connected to the room and their position. This file must match the
server’s definition or you will receive errors that the schema is not in
agreement between the client and server

 */

using Colyseus.Schema;

public partial class Player : Schema
{
    [Type(0, "string")]
    public string id = default(string);

    [Type(1, "string")]
    public string ownerId = default(string);

    [Type(2, "number")]
    public float xPos = default(float);

    [Type(3, "number")]
    public float yPos = default(float);

    [Type(4, "string")]
    public string sessionId = default(string);

    [Type(5, "boolean")]
    public bool connected = default(bool);

}