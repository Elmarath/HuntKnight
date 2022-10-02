using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "AnimalAttributes", menuName = "Scriptables/AnimalAttributes", order = 2)]
public class CommonAnimalAttributes : ScriptableObject
{
    [Header("General Stats")]
    public float nutrientCapacity = 100f;
    public float holdingPoopCapacity = 100f;
    public float health = 100f;
    public float angularSpeed = 180f;
    public float walkSpeed = 1f;
    public float runSpeed = 2.5f;
    public float stamina = 50f;
    public GameObject animalExcrete;

    [Header("State Durations")]
    public float idleTime = 1f;
    public float consumeTime = 1f;
    public float poopTime = 1f;
    public float birthTime = 5f;
    public float mateTime = 5f;
    public float nutrientIncreaseRate = 1f;
    public float poopIncreaseRate = 1f;
    public float mateIncreaseRate = 1f;
    public float staminaIncreaseRate = 1f;
    public float staminaDecreaseRate = 5f;
    public float poopThreshold = 90f;
    public float nutientThreshold = 50f;
    public float mateThreshold = 50f;

    [Header("Combat Stats")]
    public float strength = 50f;
    public float attackSpeed = 1f;
    public float attackRange = 2f;
    public bool isTerritorial = false;
    public bool isStealthy = false;

    [Header("Field Of View Stats")]
    public float sightRange = 15f;
    public float sightAngle = 120f;


    [Header("Be Aware of:")]
    public LayerMask runFromThese;
    public LayerMask attackThese;
    public LayerMask eatThese;

}
