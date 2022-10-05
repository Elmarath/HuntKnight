using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatableVegatation : MonoBehaviour
{
    public bool _isStartedToBeEaten = false;
    public CommonAnimal _eater;
    public Vector3 _mainScale;
    public int _mainLayer;
    public float _growBackTime = 20f;

    public void StartEating(CommonAnimal eater)
    {
        _mainScale = transform.localScale;
        _mainLayer = gameObject.layer;
        _eater = eater;

        StartCoroutine("Eating");
    }

    private IEnumerator Eating()
    {
        Debug.Log("Eating in veg");
        float animalConsumeTime = _eater.animalAttributes.consumeTime;
        float waitForSec = 0.1f;
        float ticCount = 1 / waitForSec;
        float scaleChange = _mainScale.x / ticCount;
        for (int i = 0; i < ticCount; i++)
        {
            transform.localScale -= new Vector3(scaleChange, scaleChange, scaleChange);
            yield return new WaitForSeconds(waitForSec);
        }
        gameObject.layer = 0;
        yield return new WaitForSeconds(_growBackTime);
        StartCoroutine("GrowBack");
    }

    private IEnumerator GrowBack()
    {
        float waitForSec = 0.1f;
        float ticCount = 1 / waitForSec;
        float scaleChange = _mainScale.x / ticCount;
        for (int i = 0; i < ticCount; i++)
        {
            transform.localScale += new Vector3(scaleChange, scaleChange, scaleChange);
            yield return new WaitForSeconds(waitForSec);
        }
        gameObject.layer = _mainLayer;
    }

}
