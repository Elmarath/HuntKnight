using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "AnimalAttributes", menuName = "Scriptables/AnimalAttributes", order = 2)]
public class CommonAnimalAttributes : ScriptableObject
{
    [Header("General Attributes")]
    public float nutrientCapacity = 100f;
    public float holdingPoopCapacity = 100f;
    public float Health = 100f;
    public float walkSpeed = 1f;
    public float runSpeed = 2.5f;
    public float idleTime = 1f;
    public float consumeTime = 1f;
    public float poopTime = 1f;
    public float strength = 50f;
    public float attackSpeed = 1f;
    public float attackRange = 2f;
    public float sightRange = 15f;
    public float sightAngle = 120f;
    public bool isTerritorial = false;
    public bool isStealthy = false;

    [Header("Be Aware of:")]
    public LayerMask runFromThese;
    public LayerMask attackThese;
    public LayerMask eatThese;

}
