using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public float threshold = 0.2f;
    public Vector2 InputVector { get; private set; }
    public Vector3 MousePosition { get; private set; }

    void Update(){
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");
        Vector2 TempInputVector = new Vector2(h, v);

        if (TempInputVector.magnitude > threshold)
        {
            InputVector = TempInputVector;
        }
        else
        {
            InputVector = Vector2.zero;
        }

        MousePosition = Input.mousePosition;
    }
}
