using System;
using UnityEngine;

/// <summary>
/// �������� �� ������� �������� ������ �� �������
/// </summary>
public class SmoothFollowTarget : Singleton<SmoothFollowTarget>
{
    public GameObject target;
    public Vector3 offset { get; set; }

    bool hasTarget;

    private void LateUpdate()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
            hasTarget = false;
            return;
        }
        else if (!hasTarget)
        {
            transform.position = new Vector3(1.5f, 12, -7) + target.transform.position;
            offset = transform.position - target.transform.position;
            hasTarget = true;
        }

        transform.position = Vector3.Lerp(transform.position, target.transform.position + offset, Time.deltaTime * 5);
    }
}

