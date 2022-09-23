using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopdownCharacterAnimationHandler : MonoBehaviour
{
    private InputHandler inputHandler;
    private Animator animator;


    private int isWalkingForward;

    // Start is called before the first frame update
    void Start()
    {
        inputHandler = GetComponent<InputHandler>();    
        animator = GetComponent<Animator>();

        isWalkingForward = Animator.StringToHash("isWalkingForward");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMovementAnimations();
        UpdateForwardDirection();
    }

    private void UpdateMovementAnimations()
    {
        if(inputHandler.InputVector.sqrMagnitude > inputHandler.threshold * inputHandler.threshold)
        {
            animator.SetBool("isWalkingForward", true);
        }
        else
        {
            animator.SetBool( "isWalkingForward", false);
        }
    }

    private void UpdateForwardDirection()
    {      
        Vector3 currentPosition = transform.position;
    
        Vector3 desiredRotation = currentPosition + new Vector3 (inputHandler.InputVector.x, 0, inputHandler.InputVector.y);

        transform.LookAt(desiredRotation);
    }
}
