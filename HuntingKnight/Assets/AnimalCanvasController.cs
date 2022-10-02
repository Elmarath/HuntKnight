using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalCanvasController : MonoBehaviour
{
    private Transform cam;
    private CommonAnimal commonAnimal;
    private bool readtToUpdateUI = false;
    [HideInInspector] public List<UI_AnimalStatBar> statBars = new List<UI_AnimalStatBar>();

    [HideInInspector] public UI_AnimalStatBar healthBar;
    [HideInInspector] public UI_AnimalStatBar nutrientBar;
    [HideInInspector] public UI_AnimalStatBar staminaBar;
    [HideInInspector] public UI_AnimalStatBar excreteBar;
    [HideInInspector] public UI_CurrentStateText currentStateText;

    //first enable does not work
    private bool _isFirstEnable = true;

    private void OnEnable()
    {
        cam = Camera.main.transform;
        if (_isFirstEnable)
        {
            _isFirstEnable = false;
            return;
        }
        else
        {
            commonAnimal = GetComponentInParent<CommonAnimal>();
            currentStateText = GetComponentInChildren<UI_CurrentStateText>();
            currentStateText.Init();
            foreach (Transform child in transform)
            {
                statBars.Add(child.GetComponentInChildren<UI_AnimalStatBar>());
                if (child.name == "HealthBar")
                {
                    healthBar = child.GetComponentInChildren<UI_AnimalStatBar>();
                    healthBar.Init();
                    healthBar.SetMax(commonAnimal.animalAttributes.health);
                }
                if (child.name == "NutrientBar")
                {
                    nutrientBar = child.GetComponentInChildren<UI_AnimalStatBar>();
                    nutrientBar.Init();
                    nutrientBar.SetMax(commonAnimal.animalAttributes.nutrientCapacity);
                }
                if (child.name == "StaminaBar")
                {
                    staminaBar = child.GetComponentInChildren<UI_AnimalStatBar>();
                    staminaBar.Init();
                    staminaBar.SetMax(commonAnimal.animalAttributes.stamina);
                }
                if (child.name == "ExcreteBar")
                {
                    excreteBar = child.GetComponentInChildren<UI_AnimalStatBar>();
                    excreteBar.Init();
                    excreteBar.SetMax(commonAnimal.animalAttributes.holdingPoopCapacity);
                }
            }
            InvokeRepeating("UpdateUI", 0f, 0.25f);
        }
    }

    private void OnDisable()
    {
        CancelInvoke("UpdateUI");
    }

    void LateUpdate()
    {
        // find commonAnimal's currentstate

        transform.LookAt(transform.position + cam.forward);
    }

    private void UpdateUI()
    {
        currentStateText.SetText(GetCurrentState());
        if (commonAnimal != null)
        {
            healthBar.SetCurrent(commonAnimal.currentHealth);
            nutrientBar.SetCurrent(commonAnimal.currentNutrientNeed);
            staminaBar.SetCurrent(commonAnimal.currentStamina);
            excreteBar.SetCurrent(commonAnimal.currentPoopNeed);
        }
    }

    private string GetCurrentState()
    {
        string currentState = "";
        State _currentState = commonAnimal.stateMachine.CurrentState;

        if (_currentState == commonAnimal.attackState)
        {
            currentState = "attackState";
        }
        else if (_currentState == commonAnimal.customState)
        {
            currentState = "customState";
        }
        else if (_currentState == commonAnimal.deathState)
        {
            currentState = "deathState";
        }
        else if (_currentState == commonAnimal.eatState)
        {
            currentState = "eatState";
        }
        else if (_currentState == commonAnimal.excreteState)
        {
            currentState = "excreteState";
        }
        else if (_currentState == commonAnimal.idleState)
        {
            currentState = "idleState";
        }
        else if (_currentState == commonAnimal.makeBirthState)
        {
            currentState = "makeBirthState";
        }
        else if (_currentState == commonAnimal.mateState)
        {
            currentState = "mateState";
        }
        else if (_currentState == commonAnimal.fleeState)
        {
            currentState = "fleeState";
        }
        else if (_currentState == commonAnimal.chaseState)
        {
            currentState = "chaseState";
        }
        else if (_currentState == commonAnimal.takeDamageState)
        {
            currentState = "takeDamageState";
        }
        else if (_currentState == commonAnimal.walkState)
        {
            currentState = "walkState";
        }
        else if (_currentState == commonAnimal.takeCoverState)
        {
            currentState = "takeCoverState";
        }
        else
        {
            currentState = "***********";
        }
        return currentState;
    }
}
