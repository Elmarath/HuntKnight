using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;


public abstract class Herbivore : Animal
{
    public bool isSearchingForFood = false;
    public bool isSearchingForWater = false;

    public bool isEating = false;
    public bool isDrinking = false;
    
}