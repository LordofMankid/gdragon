
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerManager : NetworkBehaviour
{

    public override void OnStartServer()
    {
        base.OnStartServer();
        Debug.Log("Server Started!");
        //this displays in the Console when we hit the Host button to get things started
    }

}