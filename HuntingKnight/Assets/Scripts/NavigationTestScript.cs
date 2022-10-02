using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationTestScript : MonoBehaviour
{
    public CommonAnimalAttributes animalAttributes;
    public bool isBeingAttacked;
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // to create new destination
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            Vector3 MousePositonOnPlane = GetMousePositionOnPlane();
            bool hasPathFound = GoDestination(agent, MousePositonOnPlane);
            CreateIndicator(agent.destination, Color.yellow);
            CreateCircle(agent.destination, agent.stoppingDistance, Color.blue);
            DrawNavMeshPath(agent);
        }
        // check if destination is reached if reached and forward is close turn around
        if (IsDesinationReached(agent))
        {
            if (!CheckForwardWalkable(agent, 2f))
            {
                CreateIndicator(agent.transform.position + (agent.transform.forward * 2f), Color.red, height: 5f);
                RotateNavMeshAgent(agent, agent.transform.right * -2f, Time.deltaTime);
            }
        }

        if (isBeingAttacked)
        {
            isBeingAttacked = false;
        }
        //MatchAgentToSurfaceSlope(agent);
    }


    private void MatchAgentToSurfaceSlope(NavMeshAgent agent)
    {
        // get the normal of the surface the agent is on
        Vector3 surfaceNormal = GetSurfaceNormal(agent);
        // get the angle between the surface normal and the up vector
        float angle = Vector3.Angle(surfaceNormal, Vector3.up);
        // get the rotation axis
        Vector3 axis = Vector3.Cross(surfaceNormal, Vector3.up);
        // create a rotation from the angle and axis
        Quaternion rotation = Quaternion.AngleAxis(angle, axis);
        // rotate the agent to match the surface slope
        agent.transform.rotation = rotation;
    }

    private Vector3 GetSurfaceNormal(NavMeshAgent agent)
    {
        // get the position of the agent
        Vector3 position = agent.transform.position;
        // create a ray from the position to the surface
        Ray ray = new Ray(position, Vector3.down);
        // create a raycast hit to store the raycast hit information
        RaycastHit hit;
        // raycast to the surface
        if (Physics.Raycast(ray, out hit))
        {
            // return the normal of the surface
            return hit.normal;
        }
        // if the raycast did not hit anything return the up vector
        return Vector3.up;
    }

    /// <summary>
    /// Check if the agent is no longer have a path an in the stopping distance
    /// </summary>
    /// <param name="agent"></param>
    /// <returns></returns>
    private bool IsDesinationReached(NavMeshAgent agent)
    {
        // Check if we've reached the destination
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude < 0.1f)
                {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Goes to target Destination in navmesh if the navmesh path is valid
    /// </summary>
    /// <param name="targetPosition"></param>
    /// <returns>if new destination has found</returns>
    private bool GoDestination(NavMeshAgent agent, Vector3 targetPosition)
    {
        Vector3 agentPreviousDestination = agent.destination;
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(targetPosition, path);
        if (path.status == NavMeshPathStatus.PathComplete)
        {
            agent.SetDestination(targetPosition);
        }
        else
        {
            Debug.Log("Path not valid");
        }

        return agentPreviousDestination != agent.destination;
    }

    /// <summary>
    /// Draws a debug line to the target destination using navmesh path
    /// </summary>
    /// <param name="targetPosition"></param>
    private void DrawNavMeshPath(NavMeshAgent agent)
    {
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(agent.destination, path);
        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.black, 3f, true);
        }
    }

    /// <summary>
    ///  Get mouse position
    /// </summary>
    /// <returns>Position of the hit, if couldn't find object to hit: Vector3.Zero</returns>
    private Vector3 GetMousePositionOnPlane()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }
        return Vector3.zero;
    }

    /// <summary>
    /// Checks if the agents "distance" away is walkable
    /// </summary>
    /// <param name="agent"></param>
    /// <param name="distance"></param>
    /// <returns></returns>
    private bool CheckForwardWalkable(NavMeshAgent agent, float distance)
    {
        Vector3 agentPreviousDestination = agent.destination;
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(agent.transform.position + ((transform.forward) * distance), path);
        if (path.status == NavMeshPathStatus.PathComplete)
        {
            return true;
        }
        else return false;
    }

    /// <summary>
    /// Creates a capsule indicator at given position and Destroys it after 1 second
    /// </summary>
    /// <param name="position"></param>
    /// <param name="color" if not initialized default value will be black></param>
    private void CreateIndicator(Vector3 position, Color color = default, float height = 0.5f)
    {
        if (position != Vector3.zero)
        {
            GameObject indicator = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            indicator.transform.position = position;
            indicator.transform.localScale = new Vector3(0.1f, height, 0.1f);
            indicator.transform.position = new Vector3(indicator.transform.position.x, indicator.transform.position.y + height, indicator.transform.position.z);
            var indicatorRenderer = indicator.GetComponent<Renderer>();
            indicatorRenderer.material.color = color;
            Destroy(indicator, 1f);
        }
    }

    /// <summary>
    /// Returns the angle between the agent and the target
    /// </summary>
    /// <param name="agent"></param>
    /// <param name="targetPosition"></param>
    /// <param name="deltaTime"></param>
    /// <param name="rotationSpeed"></param>
    private void RotateNavMeshAgent(NavMeshAgent agent, Vector3 targetPosition, float deltaTime, float rotationSpeed = 3f)
    {
        Vector3 direction = (targetPosition - agent.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, lookRotation, deltaTime * rotationSpeed);
    }

    private void CreateCircle(Vector3 center, float radius, Color color = default)
    {
        int segments = 30;
        float x;
        float z;
        float angle = 20f;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            z = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

            Debug.DrawLine(new Vector3(center.x + x, center.y, center.z + z), new Vector3(center.x + x, center.y + 0.2f, center.z + z), color, 3f, true);

            angle += (360f / segments);
        }
    }

}
