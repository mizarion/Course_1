using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollScript : MonoBehaviour
{
#pragma warning disable 649

    [SerializeField] Rigidbody rbody;
    [SerializeField] float forceToAdd = 5000;

#pragma warning restore 649

    public void StartDeath(Vector3 force, float liveTime)
    {
        rbody.AddForce(force * forceToAdd);
        Destroy(gameObject, liveTime);
    }
}
