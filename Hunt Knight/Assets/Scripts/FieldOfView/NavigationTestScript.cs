using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationTestScript : MonoBehaviour
{
    public GameObject movementIndicator;

    public float logicUpdateInterval = 0.2f;

    public float viewRadius = 15f;
    public float viewAngle = 120f;
    public float minSearchDistance = 1f;

    public List<GameObject> visibleTargets = new List<GameObject>();

    public LayerMask targetMask;

    private NavMeshAgent navMeshAgent;
    private FieldOfView fieldOfView;
    private IEnumerator coroutine;

    private Vector3 createdDestination;
    

    // Start is called before the first frame update
    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        fieldOfView = GetComponent<FieldOfView>();
        fieldOfView.targetMask = targetMask;

        StartCoroutine(CreateRandomDestinationInSec(logicUpdateInterval));
        StartCoroutine(FindTargetsInSec(logicUpdateInterval));
    }

    void LateUpdate()
    {
        UpdateFieldOfViewAttributes(viewRadius, viewAngle);
    }

    public void UpdateFieldOfViewAttributes(float viewRadius, float viewAngle)
    {
        fieldOfView.viewRadius = viewRadius;
        fieldOfView.viewAngle = viewAngle;
    }

    public Vector3 CreateRandomDestination(float viewRadius, float viewAngle, float minSearchDistance)
    {
        Vector3 randomDirection;
        Vector2 randomDirectionV2;
        Vector3 finalPosition;
        NavMeshHit hit;

        for(int i = 0; i < 100; i++)
        {
            randomDirectionV2 = Random.insideUnitCircle * viewRadius;
            randomDirection = new Vector3(randomDirectionV2.x, 0, randomDirectionV2.y);
            randomDirection += transform.position;

            float distance = Vector3.Distance(randomDirection, transform.position);

            // Distance Check
            if(distance < minSearchDistance)
            {
                continue;
            }
            // Angle Check
            if(Vector3.Angle(transform.forward, randomDirection - transform.position) > viewAngle / 2)
            {
                continue;
            }

            // Navigation Validation Check
            bool isWalkable = NavMesh.SamplePosition(randomDirection, out hit, 0.5f, NavMesh.AllAreas);
            if(!isWalkable)
            {
                continue;
            }

            finalPosition = hit.position;
            finalPosition = randomDirection;
            SpawnIndicator(movementIndicator, finalPosition);
            return finalPosition;
        }
        finalPosition = transform.position;
        return finalPosition;
    }

    public void SpawnIndicator(GameObject indicator, Vector3 position)
    {
        GameObject _indicator = Instantiate(indicator, position, Quaternion.identity);
        Destroy(_indicator, 5f);
    }

    // every logicUpdateInterval seconds perform a new search for a new destination
    private IEnumerator CreateRandomDestinationInSec(float waitTime)
    {
        while (true)
        {   
            yield return new WaitForSeconds(waitTime);
            createdDestination = CreateRandomDestination(viewRadius, viewAngle, minSearchDistance);
        }
    }
    private IEnumerator FindTargetsInSec(float waitTime)
    {
        while (true)
        {   
            yield return new WaitForSeconds(waitTime);
            visibleTargets = fieldOfView.FindVisibleTargets();
        }
    }

}
