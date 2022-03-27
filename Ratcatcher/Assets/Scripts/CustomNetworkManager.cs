using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CustomNetworkManager : NetworkRoomManager
{
    public GameObject PlayerPrefab2;
    bool isRatCatcher = true;
    Vector3 spawnpoint = new Vector3(-10, 0, 25);

    public override GameObject OnRoomServerCreateGamePlayer(NetworkConnectionToClient conn, GameObject roomPlayer)
    {
        if (!isRatCatcher)
        {
            isRatCatcher = true;
            return Instantiate(PlayerPrefab2, spawnpoint, Quaternion.identity);
        }
        else
        {
            // get start position from base class
            Transform startPos = GetStartPosition();
            if (startPos == null)
                return Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            else
                return Instantiate(playerPrefab, startPos.position, startPos.rotation);
        }
    }
}
