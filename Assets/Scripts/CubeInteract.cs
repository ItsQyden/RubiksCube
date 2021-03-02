using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeInteract : MonoBehaviour
{
    CubeController controller;

    RaycastHit initHit;

    private void Start()
    {
        controller = transform.parent.parent.GetComponent<CubeController>();
    }

    void OnMouseDown()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out initHit);
    }

    void OnMouseUp()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit finalHit) && CheckCollider(initHit.collider, finalHit.collider))
        {
            controller.DoRotation(initHit, finalHit);
        }
    }

    private bool CheckCollider(Collider col1, Collider col2)
    {
        if (col1 == null || col2 == null)
        {
            return false;
        }
        return true;
    }
}
