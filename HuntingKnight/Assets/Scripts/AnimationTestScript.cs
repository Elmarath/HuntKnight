using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimationTestScript : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;
    private string currentAnimation = "isIdling";
    private string exAnimation = "null";

    public bool button;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        UpdateAnimations();
    }

    private void UpdateAnimations()
    {

        if (agent.velocity.magnitude > 0.1f)
        {
            currentAnimation = "isWalking";
        }
        else
        {
            currentAnimation = "isIdling";
        }

        bool isSwitchedAnimation = !(exAnimation == currentAnimation);

        if (isSwitchedAnimation)
        {
            if (agent.velocity.magnitude > 0.1f)
            {
                animator.SetTrigger("isWalking");
                currentAnimation = "isWalking";
            }
            else
            {
                animator.SetTrigger("isIdling");
                currentAnimation = "isIdling";
            }
            exAnimation = currentAnimation;
        }

    }
}
