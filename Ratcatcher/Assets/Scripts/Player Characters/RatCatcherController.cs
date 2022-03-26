using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatCatcherController : PlayerController
{
    new float speed = 4f;
    float chargeTimer = 0f;
    float attackRange = 3f;

    private void Update()
    {
        base.Update();
        if(Input.GetButton("Interact"))
        {
            chargeTimer += Time.deltaTime;
            base.speed = 2f;
        }

        if(Input.GetButtonUp("Interact"))
        {
            Debug.Log(chargeTimer);
            base.speed = this.speed;

            if (chargeTimer > 2f)
            {
                Attack();
            }

            chargeTimer = 0f;
        }
    }

    private void Attack()
    {
        Debug.Log("swing");

        // create a raycast object originating from the player, moving in the direction they are facing
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, this.transform.forward, out hit, attackRange))
        {
            // report if a player is hit
            ExterminatorController exterm = hit.transform.GetComponent<ExterminatorController>();
            if (exterm != null)
            {
                Debug.Log("Attack Landed");
            }
                
        }
    }
}
