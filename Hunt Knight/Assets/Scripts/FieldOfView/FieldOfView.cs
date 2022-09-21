using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [HideInInspector]
    public float viewRadius = 10f;
    [HideInInspector]
    public float viewAngle = 120f;
    [HideInInspector]
    public LayerMask targetMask;
    public List<GameObject> visibleTargets = new List<GameObject>();

    // returns the first seen target in the field of view
    public List<GameObject> FindVisibleTargets()
    {
        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            GameObject targetObject = targetsInViewRadius[i].gameObject;
            Transform targetTransform = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (targetTransform.position - transform.position).normalized;

            // Angle Check
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                // Distance check
                float dstToTarget = Vector3.Distance(transform.position, targetTransform.position);
                if (dstToTarget < viewRadius)
                {
                    // Check if there is an obstacle between the target and the agent
                    RaycastHit hit;
                    Physics.Raycast(transform.position, dirToTarget, out hit);
                    int hitObjectLayer = hit.transform.gameObject.layer;
                    int targetObjectLayer = targetObject.layer;

                    if (hitObjectLayer == targetObjectLayer)
                    {
                        visibleTargets.Add(targetObject);
                    }
                }
            }
            // if seen object is self remove
            if (targetObject == gameObject)
            {
                visibleTargets.Remove(targetObject);
            }
        }

        return visibleTargets;
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public List<GameObject> GetVisibleTargets()
    {
        return visibleTargets;
    }
}