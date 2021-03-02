using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubeController : MonoBehaviour
{
    public List<GameObject> cubes;

    public bool rotating;

    void Awake()
    {
        SetupCubesList();
    }

    void SetupCubesList()
    {
        cubes = new List<GameObject>();

        foreach (Transform child in transform)
        {
            cubes.Add(child.gameObject);
        }
    }

    public void TriggerCubeRotate(Vector3 rotationAxis, float vertex, float rotate)
    {
        if (rotationAxis == Vector3.zero)
        {
            return;
        }

        GameObject parentTemp = new GameObject();
        parentTemp.transform.position = Vector3.zero;

        List<GameObject> cubesToTurn = FindCubesToTurn(rotationAxis, vertex, parentTemp);

        StartCoroutine(Rotate(cubesToTurn, parentTemp, rotationAxis, rotate));
    }

    private List<GameObject> FindCubesToTurn(Vector3 rotationAxis, float vertex, GameObject parentTemp)
    {
        List<GameObject> cubesToTurn = new List<GameObject>();

        foreach (GameObject cube in cubes)
        {
            float vertexOfCube = Vector3.Dot(cube.transform.position, rotationAxis);
            if (Mathf.Abs(vertex - vertexOfCube) <= .5)
            {
                cubesToTurn.Add(cube);
                cube.transform.parent = parentTemp.transform;
            }
        }

        return cubesToTurn;
    }

    private IEnumerator Rotate(List<GameObject> cubesToTurn, GameObject parentTemp, Vector3 rotationAxis, float rotate)
    {
        rotating = true;

        float offset = Mathf.Round(rotate) / 20;

        for (int i = 0; i < 20; i++)
        {
            parentTemp.transform.Rotate(rotationAxis, offset);
            yield return new WaitForEndOfFrame();
        }

        ResetCubes(cubesToTurn);

        Destroy(parentTemp);

        rotating = false;
    }

    private void ResetCubes(List<GameObject> cubesToReset)
    {
        foreach (GameObject cube in cubesToReset)
        {
            cube.transform.parent = transform;

            cube.transform.position = new Vector3(
                Mathf.Round(cube.transform.position.x),
                Mathf.Round(cube.transform.position.y),
                Mathf.Round(cube.transform.position.z));

            float x = cube.transform.position.x;
            float y = cube.transform.position.y;
            float z = cube.transform.position.z;
        }
    }

    public void DoRotation(RaycastHit initHit, RaycastHit finalHit)
    {
        if (!rotating)
        {
            Vector3 initNormal = initHit.normal;
            Vector3 finalNormal = finalHit.normal;

            Vector3 travelDelta = finalHit.transform.position - initHit.transform.position;

            Vector3 rotationAxis = GetRotationAxis(initNormal, travelDelta);
            float vertexOfCube = Mathf.Round(Vector3.Dot(initHit.transform.position, rotationAxis));

            TriggerCubeRotate(rotationAxis, vertexOfCube, 90);
        }
    }

    private Vector3 GetRotationAxis(Vector3 initNormal, Vector3 travelDelta)
    {
        Vector3 axis = Vector3.Cross(initNormal, GetDirection(travelDelta));
        axis.x = Mathf.Round(axis.x);
        axis.y = Mathf.Round(axis.y);
        axis.z = Mathf.Round(axis.z);
        return axis;
    }

    private Vector3 GetDirection(Vector3 delta)
    {
        Vector3 absDelta = new Vector3(Mathf.Abs(delta.x), Mathf.Abs(delta.y), Mathf.Abs(delta.z));

        if (absDelta.x > absDelta.y && absDelta.x > absDelta.z)
        {
            return new Vector3(Mathf.Sign(delta.x), 0, 0);
        }

        else if (absDelta.y > absDelta.z)
        {
            return new Vector3(0, Mathf.Sign(delta.y), 0);
        }

        else
        {
            return new Vector3(0, 0, Mathf.Sign(delta.z));
        }
    }

    public GameObject GetCubeFromVector(Vector3 position)
    {
        Debug.Log(cubes.Count);
        foreach(GameObject cube in cubes)
        {
            if(cube.transform.position == position)
            {
                return cube;
            }
        }
        return null;
    }
}