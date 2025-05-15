using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeathPotion : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        if(player != null)
        {
            player.HealthChange(20f);
            Destroy(gameObject);
        }
    }
}
