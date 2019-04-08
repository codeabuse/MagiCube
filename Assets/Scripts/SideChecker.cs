using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideChecker : MonoBehaviour
{
    [SerializeField]
    private Cube cube;
    [SerializeField]
    private Checker sideCheckerPosition;

    Ray ray;
    RaycastHit hit;
    float raycastTimer;
    float raycastDelay = 0.2f;

    void Start()
    {
        ray = new Ray(transform.position, transform.forward);
    }

    
    void Update()
    {
        raycastTimer += Time.deltaTime;
        if (raycastTimer >= raycastDelay)
        {
            //CheckSide();
        }
    }

    void CheckSide()
    {
        Physics.Raycast(ray, out hit);
        if (hit.collider.tag == "CubeSide")
        {
            Side side = hit.transform.GetComponent<Side>();
            switch (sideCheckerPosition)
            {
                case Checker.left:
                    cube.SetLeftSide(side);
                    break;
                case Checker.up:
                    cube.SetTopSide(side);
                    break;
                case Checker.front:
                    cube.SetFrontSide(side);
                    break;
                default:
                    break;
            }
        }
    }
}

public enum Checker
{
    left, up, front
}
