using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimationTestScript : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;
    private string currentAnimation = "Walking";
    private string exAnimation = "null";

    public bool button = true;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (button)
        {
            animator.CrossFade("Walking", 0.2f);
            button = false;
        }
    }
}
