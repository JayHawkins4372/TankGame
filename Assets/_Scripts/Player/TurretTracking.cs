//Author: Wade lawler
//Last Modified: 9/23/26
using UnityEngine;

public class TurretTracking : MonoBehaviour
{
    public float targetingRange = 15f;
    public float rotationSpeed = 10f;
    public LayerMask enemyLayer;

    //center of enemy
    private Vector3 targetAimPoint;
    private bool hasTarget = false;

    void Update()
    {
        FindNearestEnemy();

        if (hasTarget)
        {
            RotateTowards(targetAimPoint);
        }
    }

    void FindNearestEnemy()
    {
        Collider[] hitColliders = new Collider[10];
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, targetingRange, hitColliders, enemyLayer);

        float shortestDistance = Mathf.Infinity;
        bool foundTarget = false;

        for (int i = 0; i < numColliders; i++)
        {
           //get center of enemy
            Vector3 enemyCenter = hitColliders[i].bounds.center;

            float distanceToEnemy = Vector3.Distance(transform.position, enemyCenter);

            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                targetAimPoint = enemyCenter;
                foundTarget = true;
            }
        }

        hasTarget = foundTarget;
    }

    void RotateTowards(Vector3 targetPosition)
    {
        Vector3 flatTargetPos = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
        Vector3 direction = flatTargetPos - transform.position;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
