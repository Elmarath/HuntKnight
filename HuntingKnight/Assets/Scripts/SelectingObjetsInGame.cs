using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectingObjetsInGame : MonoBehaviour
{
    public LayerMask selectableMask;
    private List<GameObject> selectedObjects = new List<GameObject>();
    public GameObject animal_UI_Canvas;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (AnimalNavigationHelper.IsLayerInLayerMask(hit.transform.gameObject.layer, selectableMask))
                {
                    Debug.Log("Selected: " + hit.transform.parent.gameObject.name);
                    // field of view
                    FieldOfView fieldOfView = hit.transform.gameObject.transform.parent.GetComponentInChildren<FieldOfView>();
                    fieldOfView.isFieldOfViewVisible = !fieldOfView.isFieldOfViewVisible;
                    // animal stat bar activation
                    CommonAnimal _commonAnimal = hit.transform.gameObject.transform.parent.GetComponentInChildren<CommonAnimal>();
                    _commonAnimal.ToggleCanvas();
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            foreach (GameObject selectedObject in selectedObjects)
            {
                FieldOfView fieldOfView = selectedObject.transform.parent.GetComponentInChildren<FieldOfView>();
                fieldOfView.isFieldOfViewVisible = false;
            }
            selectedObjects.Clear();
        }
    }
}
