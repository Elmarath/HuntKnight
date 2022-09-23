using UnityEngine;
using UnityEngine.AI;

public static class AnimalHelper
{
    // Creates a random Destination which in the animals searchable area
    public static Vector3 CreateRandomDestination(Animal animal)
    {
        Vector3 randomDirection;
        Vector2 randomDirectionV2;
        Vector3 finalPosition;
        NavMeshHit hit;

        for (int i = 0; i < 10; i++)
        {
            randomDirectionV2 = Random.insideUnitCircle * animal.viewRadius;
            randomDirection = new Vector3(randomDirectionV2.x, 0, randomDirectionV2.y);
            randomDirection += animal.transform.position;

            float distance = Vector3.Distance(randomDirection, animal.transform.position);

            // Distance Check
            if (distance < animal.minSearchDistance)
            {
                continue;
            }
            // Angle Check
            if (Vector3.Angle(animal.transform.forward, randomDirection - animal.transform.position) > animal.viewAngle / 2)
            {
                continue;
            }
            // Navigation Validation Check
            bool isWalkable = NavMesh.SamplePosition(randomDirection, out hit, animal.closeEnoughTolerance, NavMesh.AllAreas);
            if (!isWalkable)
            {
                continue;
            }

            finalPosition = randomDirection;

            return finalPosition;
        }

        finalPosition = animal.transform.position - (animal.transform.forward * animal.minSearchDistance);
        return finalPosition;
    }

    // sets given destination to the agent's destination and draws ray to that position
    public static void GotoDestination(Animal animal, Vector3 destination)
    {
        animal.agent.SetDestination(destination);
    }

    // Check if self is close enough to destination
    public static bool IsCloseEnough(Vector3 position1, Vector3 position2, float closeEnoughTolerance)
    {
        position1 = new Vector3(position1.x, 0, position2.z);
        position2 = new Vector3(position2.x, 0, position2.z);
        return ((position1 - position2).sqrMagnitude) < (closeEnoughTolerance * closeEnoughTolerance);
    }

    public static LayerMask AddTwoMask(LayerMask mask1, LayerMask mask2)
    {
        return mask1 | mask2;
    }

    public static bool IsLayerInLayerMask(int layer, LayerMask layerMask)
    {
        return layerMask == (layerMask | (1 << layer));
    }

    // Spawns the given indicator in position
    public static void SpawnIndicator(GameObject indicator, Vector3 position)
    {
        GameObject _indicator = GameObject.Instantiate(indicator, position, Quaternion.identity);
        GameObject.Destroy(_indicator, 5f);
    }
}