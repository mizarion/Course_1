using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollScript : MonoBehaviour
{
    [SerializeField] Rigidbody rbody;
    [SerializeField] float forceToAdd = 5000;

    public void StartDeath(Vector3 force, float liveTime)
    {
        Debug.Log(force* forceToAdd);
        rbody.AddForce(force * forceToAdd);
        Destroy(gameObject, liveTime);
    }
}
