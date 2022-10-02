using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatableVegatation : MonoBehaviour
{
    public bool _isStartedToBeEaten = false;
    public bool _isEaten = false;
    public GameObject _eater;
    public Vector3 _mainPoisiton;
    public Vector3 _mainScale;
    public float _growBackTime = 20f;

    public bool button = false;

    private void Update()
    {
        if (button)
        {
            button = false;
            StartEating(_eater);
        }
    }

    public void StartEating(GameObject eater)
    {
        _isStartedToBeEaten = true;
        _mainPoisiton = transform.position;
        _mainScale = transform.localScale;
        _eater = eater;

        StartCoroutine("Eating");
    }

    private IEnumerator Eating()
    {
        float animalConsumeTime = _eater.GetComponent<CommonAnimal>().animalAttributes.consumeTime;
        float waitForSec = 0.1f;
        int ticCount = Mathf.FloorToInt(animalConsumeTime / waitForSec);
        for (int i = 0; i < ticCount; i++)
        {
            transform.localScale -= new Vector3(waitForSec, waitForSec, waitForSec);
            yield return new WaitForSeconds(waitForSec);
        }
        _isEaten = true;
        yield return new WaitForSeconds(_growBackTime);
        StartCoroutine("GrowBack");
    }

    private IEnumerator GrowBack()
    {
        float waitForSec = 0.1f;
        int ticCount = Mathf.FloorToInt(1 / waitForSec);
        for (int i = 0; i < ticCount; i++)
        {
            transform.localScale += new Vector3(waitForSec, waitForSec, waitForSec);
            yield return new WaitForSeconds(waitForSec);
        }
    }

}
