using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public Health health;

    public void OnRaycastHit(float damage, Vector3 direction)
    {
        if (health != null)
        {
            health.TakeDamage(damage, direction);
        }
    }

    // Keep compatibility with any callers that expect the weapon instance
    public void OnRaycastHit(RaycastWeapon weapon, Vector3 direction)
    {
        if (weapon != null)
        {
            OnRaycastHit(weapon.damage, direction);
        }
    }
}
