using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CommonAnimalAnimations : MonoBehaviour
{
    private Animator animator;

    public CommonAnimalAnimations(Animator _animator)
    {
        animator = _animator;
    }

    public struct StateAnimation
    {
        public string name;
        public int hash;
        public float duration;
        public float normalizedTime;
        public bool isFinished;

        public StateAnimation(string name, int hash, float duration, float normalizedTime, bool isFinished)
        {
            this.name = name;
            this.hash = hash;
            this.duration = duration;
            this.normalizedTime = normalizedTime;
            this.isFinished = isFinished;
        }
    }

    public StateAnimation IDLE = new StateAnimation("Idle", Animator.StringToHash("Idle"), 0.1f, 0f, false); // 1
    public StateAnimation WALK = new StateAnimation("Walk", Animator.StringToHash("Walk"), 0.1f, 0f, false); // 2
    public StateAnimation RUN = new StateAnimation("Run", Animator.StringToHash("Run"), 0.1f, 0f, false); // 3 
    public StateAnimation ATTACK = new StateAnimation("Attack", Animator.StringToHash("Attack"), 0.1f, 0f, false); // 4
    public StateAnimation DEATH = new StateAnimation("Death", Animator.StringToHash("Death"), 0.1f, 0f, false); // 5
    public StateAnimation EAT = new StateAnimation("Eat", Animator.StringToHash("Eat"), 0.1f, 0f, false); // 6
    public StateAnimation EXCRETE = new StateAnimation("Excrete", Animator.StringToHash("Excrete"), 0.1f, 0f, false); // 7
    public StateAnimation MAKE_BIRTH = new StateAnimation("MakeBirth", Animator.StringToHash("MakeBirth"), 0.1f, 0f, false); // 8
    public StateAnimation MATE = new StateAnimation("Mate", Animator.StringToHash("Mate"), 1f, 0f, false); // 9
    public StateAnimation TAKE_DAMAGE = new StateAnimation("TAKEDAMAGE", Animator.StringToHash("TAKEDAMAGE"), 0.1f, 0f, false); // 10
    public StateAnimation TAKE_COVER = new StateAnimation("TAKECOVER", Animator.StringToHash("TAKECOVER"), 0.1f, 0f, false); // 11 
    public StateAnimation CUSTOM = new StateAnimation("CUSTOM", Animator.StringToHash("CUSTOM"), 0.1f, 0f, false); // 12

    public void PlayAnimation(StateAnimation stateAnimation)
    {
        animator.CrossFade(stateAnimation.hash, stateAnimation.duration);
    }

}