using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartHitCheck : MonoBehaviour
{
    [HideInInspector] public ParticipantController playerOwner;      // Para el player
    [HideInInspector] public DopplegangerController enemyOwner;      // Para el enemigo

    public string BodyName;
    public float Multiplier = 1f;
    public float LastDamage;

    public void TakeHit(float damage)
    {
        LastDamage = damage * Multiplier;

        if (enemyOwner != null)
        {
            enemyOwner.TakeDamage(LastDamage);
            Debug.Log($"Golpe al enemigo {BodyName}: {LastDamage}");
        }
        else if (playerOwner != null)
        {
            playerOwner.TakeDamage(LastDamage);
            Debug.Log($"Golpe al jugador {BodyName}: {LastDamage}");
        }
        else
        {
            Debug.LogError("BodyPartHitCheck: Sin dueño asignado (playerOwner/enemyOwner).");
        }
    }
}
