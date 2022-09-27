using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public static class AnimalNavigationHelper
{

    public static void MatchAgentToSurfaceSlope(NavMeshAgent agent)
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

    /// <summary>
    /// Check if position1's and 2's distance is less than closeEnoughTolerance  
    /// </summary>
    /// <param name="position1"></param>
    /// <param name="position2"></param>
    /// <param name="closeEnoughTolerance"></param>
    /// <returns></returns>
    public static bool IsCloseEnough(Vector3 position1, Vector3 position2, float closeEnoughTolerance)
    {
        return ((position1 - position2).sqrMagnitude) < (closeEnoughTolerance * closeEnoughTolerance);
    }

    public static Vector3 GetSurfaceNormal(NavMeshAgent agent)
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
    /// Goes to target Destination in navmesh if the navmesh path is valid
    /// </summary>
    /// <param name="targetPosition"></param>
    /// <returns>if new destination has found</returns>
    public static bool GoDestination(NavMeshAgent agent, Vector3 targetPosition)
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
    public static void DrawNavMeshPath(NavMeshAgent agent)
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
    public static Vector3 GetMousePositionOnPlane()
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
    public static bool CheckForwardWalkable(NavMeshAgent agent, float distance)
    {
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(agent.transform.position + ((agent.transform.forward) * distance), path);
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
    public static void CreateIndicator(Vector3 position, Color color = default, float height = 1f)
    {
        if (position != Vector3.zero)
        {
            GameObject indicator = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            indicator.transform.position = position;
            indicator.transform.localScale = new Vector3(0.1f, height, 0.1f);
            indicator.transform.position = new Vector3(indicator.transform.position.x, indicator.transform.position.y + height, indicator.transform.position.z);
            var indicatorRenderer = indicator.GetComponent<Renderer>();
            indicatorRenderer.material.color = color;
            GameObject.Destroy(indicator, 6f);
        }
    }

    /// <summary>
    /// Returns the angle between the agent and the target
    /// </summary>
    /// <param name="agent"></param>
    /// <param name="targetPosition"></param>
    /// <param name="deltaTime"></param>
    /// <param name="rotationSpeed"></param>
    public static void RotateNavMeshAgent(NavMeshAgent agent, Vector3 targetPosition, float deltaTime, float rotationSpeed = 3f)
    {
        Vector3 direction = (targetPosition - agent.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, lookRotation, deltaTime * rotationSpeed);
    }

    public static void CreateCircle(Vector3 center, float radius, Color color = default, float height = 0.5f)
    {
        int segments = 30;
        float x;
        float z;
        float angle = 20f;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            z = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

            Debug.DrawLine(new Vector3(center.x + x, center.y, center.z + z), new Vector3(center.x + x, center.y + height, center.z + z), color, 6f, true);

            angle += (360f / segments);
        }
    }

    /// <summary>
    /// Check if the layer is in the layermask
    /// </summary>
    /// <param name="layer"></param>
    /// <param name="layerMask"></param>
    /// <returns></returns>
    public static bool IsLayerInLayerMask(int layer, LayerMask layerMask)
    {
        return layerMask == (layerMask | (1 << layer));
    }

    /// <summary>
    /// Creates a random position considering agent's walkable area, position distance as radius, angle
    /// if Color attribute is set the position will be drawn as an indicator
    /// </summary>
    /// <param name="agent"></param>
    /// <param name="fieldOfView"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    public static Vector3 GetRandomWalkablePosition(NavMeshAgent agent, FieldOfView fieldOfView, Color color = default)
    {
        NavMeshPath path = new NavMeshPath();
        Vector3 randomDirection = Vector3.zero;
        bool found = false;

        for (int i = 0; i < 20; i++)
        {
            randomDirection = Random.insideUnitCircle * fieldOfView.viewRadius;
            randomDirection = new Vector3(randomDirection.x, 2, randomDirection.y) + agent.transform.position;
            found = false;
            RaycastHit hit;
            Physics.Raycast(randomDirection, Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Terrain"));
            randomDirection = hit.point;

            // check if the random position is in view angle
            if (Vector3.Angle(fieldOfView.transform.forward, randomDirection - fieldOfView.transform.position) > (fieldOfView.viewAngle / 2))
            {
                found = false;
                continue;
            }

            if (IsCloseEnough(randomDirection, agent.transform.position, fieldOfView.viewRadius / 20)) // if found loc is too close to the agent
            {
                found = false;
                continue;
            }

            agent.CalculatePath(randomDirection, path);

            if (path.status != NavMeshPathStatus.PathComplete)
            {
                found = false;
                continue;
            }
            found = true;
            if (found)
            {
                break;
            }
        }

        if (color != default)
        {
            if (found)
            {
                CreateCircle(randomDirection, agent.stoppingDistance, color);
                CreateIndicator(randomDirection, color);
            }
            else
            {
                CreateCircle(randomDirection, agent.stoppingDistance, Color.black);
                CreateIndicator(randomDirection, Color.black);
            }
        }

        if (!found)
        {
            return Vector3.zero;
        }

        return randomDirection;
    }

    /// <summary>
    /// Creates a random position considering agent's walkable area, position distance as radius, angle
    /// if Color attribute is set the position will be drawn as an indicator
    /// </summary>
    /// <param name="agent"></param>
    /// <param name="viewRadius"></param>
    /// /// <param name="viewAngle"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    public static Vector3 GetRandomWalkablePosition(NavMeshAgent agent, float viewRadius, float viewAngle, Color color = default)
    {
        NavMeshPath path = new NavMeshPath();
        Vector3 randomDirection = Vector3.zero;
        bool found = false;

        for (int i = 0; i < 20; i++)
        {
            randomDirection = Random.insideUnitCircle * viewRadius;
            randomDirection = new Vector3(randomDirection.x, 2, randomDirection.y) + agent.transform.position;
            found = false;
            RaycastHit hit;
            Physics.Raycast(randomDirection, Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Terrain"));
            randomDirection = hit.point;

            // check if the random position is in view angle
            if (Vector3.Angle(agent.transform.forward, randomDirection - agent.transform.position) > (viewAngle / 2))
            {
                found = false;
                continue;
            }

            if (IsCloseEnough(randomDirection, agent.transform.position, viewRadius / 20)) // if found loc is too close to the agent
            {
                found = false;
                continue;
            }

            agent.CalculatePath(randomDirection, path);

            if (path.status != NavMeshPathStatus.PathComplete)
            {
                found = false;
                continue;
            }
            found = true;
            if (found)
            {
                break;
            }
        }

        if (color != default)
        {
            if (found)
            {
                CreateCircle(randomDirection, agent.stoppingDistance, color);
                CreateIndicator(randomDirection, color);
            }
            else
            {
                CreateCircle(randomDirection, agent.stoppingDistance, Color.black);
                CreateIndicator(randomDirection, Color.black);
            }
        }

        if (!found)
        {
            return Vector3.zero;
        }

        return randomDirection;
    }

    /// <summary>
    /// Creates a random position considering in the direction of direction, agent's walkable area, position distance as radius, angle
    /// if Color attribute is set the position will be drawn as an indicator.
    /// if findPosLock is true the position will be found if not found in the first place without checking the direction or distance
    /// </summary>
    /// <param name="agent"></param>
    /// <param name="direction"></param>
    /// <param name="viewRadius"></param>
    /// /// <param name="viewAngle"></param>
    /// <param name="color"></param>
    /// <param name="findPosLock"></param>
    /// <returns></returns>
    public static Vector3 GetRandomWalkablePosition(NavMeshAgent agent, Vector3 direction, float viewRadius, float viewAngle, bool findPosLock = false, Color color = default)
    {
        NavMeshPath path = new NavMeshPath();
        Vector3 randomDirection = Vector3.zero;
        bool found = false;

        for (int i = 0; i < 30; i++)
        {
            randomDirection = Random.insideUnitCircle * viewRadius;
            randomDirection = new Vector3(randomDirection.x, 2, randomDirection.y) + agent.transform.position;
            found = false;
            RaycastHit hit;
            Physics.Raycast(randomDirection, Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Terrain"));
            randomDirection = hit.point;

            // check if the random position is in view angle
            if (Vector3.Angle(direction, randomDirection - agent.transform.position) > (viewAngle / 2))
            {
                found = false;
                continue;
            }

            if (IsCloseEnough(randomDirection, agent.transform.position, viewRadius / 20)) // if found loc is too close to the agent
            {
                found = false;
                continue;
            }

            agent.CalculatePath(randomDirection, path);

            if (path.status != NavMeshPathStatus.PathComplete)
            {
                found = false;
                continue;
            }
            found = true;
            if (found)
            {
                break;
            }
        }

        if (!found && findPosLock)
        {
            return GetRandomWalkablePosition(agent, viewRadius, viewAngle + 180, color);
        }

        if (color != default)
        {
            if (found)
            {
                CreateCircle(randomDirection, agent.stoppingDistance, color);
                CreateIndicator(randomDirection, color);
            }
            else
            {
                CreateCircle(randomDirection, agent.stoppingDistance, Color.black);
                CreateIndicator(randomDirection, Color.black);
            }
        }

        if (!found)
        {
            return Vector3.zero;
        }

        return randomDirection;
    }
}


