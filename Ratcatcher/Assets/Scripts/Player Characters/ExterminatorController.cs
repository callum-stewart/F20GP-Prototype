using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ExterminatorController : PlayerController
{
    public int health = 2;
    private float rushVal = 7f;
    public SprayParticle particle;

    public void Update()
    {
        if (GetComponent<NetworkIdentity>().hasAuthority)
        {
            base.Update();
            
            if(Input.GetButtonDown("Fire1"))
            {
                SprayParticle newSpray = Object.Instantiate(particle, transform.position + (Vector3.up / 2), transform.rotation);
                newSpray.SetInitPos(transform.position + (Vector3.up / 2));
            }
        }
    }

    public void Attacked()
    {
        health--;
        Debug.Log("Player Health: " + health);
        if (health < 1)
        {
            RpcEndGame();
        }
        Rush();
    }

    [ClientRpc]
    public void RpcEndGame()
    {
        FindObjectOfType<GameManager>().ChangeScene(2);
    }

    /**
     * When a player is hit they gain a short speed burst so they cannot be
     * chain hit and taken out the game unfairly
     */
    IEnumerator Rush()
    {
        float speedPrev = base.speed;
        base.speed = rushVal;
        yield return new WaitForSeconds(3);
        base.speed = speedPrev;
    }
}
