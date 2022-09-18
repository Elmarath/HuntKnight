using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopdownCharacterMover : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 2.8f;

    private InputHandler inputHandler;
    private CharacterController characterController;    

    private void Awake()
    {
        inputHandler = GetComponent<InputHandler>();
    }

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        MoveCharacterInDirection();
    }

    private void MoveCharacterInDirection()
    {
        Vector3 movementDirection = new Vector3 (inputHandler.InputVector.x, 0, inputHandler.InputVector.y);
        float magnitude = Mathf.Clamp01(movementDirection.magnitude) * moveSpeed;
        movementDirection.Normalize();
        characterController.SimpleMove(movementDirection * magnitude);
    }
}
