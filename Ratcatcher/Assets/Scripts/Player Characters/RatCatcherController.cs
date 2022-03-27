using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class RatCatcherController : PlayerController
{
    new float speed = 4f;
    float chargeTimer = 0f;
    float attackRange = 1f;
    Vector3 heightOffset = new Vector3(0, 1, 0);

    private void Update()
    {
        if (GetComponent<NetworkIdentity>().hasAuthority)
        {
            base.Update();
            if (Input.GetButton("Interact"))
            {
                chargeTimer += Time.deltaTime;
                base.speed = 2f;
            }

            if (Input.GetButtonUp("Interact"))
            {
                Debug.Log(chargeTimer);
                base.speed = this.speed;

                if (chargeTimer > 1f)
                {
                    Attack();
                }

                chargeTimer = 0f;
            }
        }
    }

    [Command]
    private void Attack()
    {
        // get the layer mask for the hitbox layer
        //int layerMask = 1 << 3;

        // Debug.Log(this.transform.forward);
        this.transform.position += this.transform.forward;

        // create a raycast object originating from the player, moving in the direction they are facing
        RaycastHit hit;
        if (Physics.SphereCast(this.transform.position + heightOffset, attackRange, this.transform.forward, out hit))
        {
            // Debug.Log(hit.collider.name);
            // report if a player is hit
            ExterminatorController exterm = hit.transform.GetComponent<ExterminatorController>();
            if (exterm != null)
            {
                // Debug.Log("Attack Landed");
                exterm.Attacked();
            }

        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Debug.DrawLine(this.transform.position + heightOffset,
            (this.transform.position + heightOffset) + this.transform.forward * attackRange);
        Gizmos.DrawWireSphere((this.transform.position + heightOffset) * attackRange
            , attackRange);
    }
}
