using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Side : MonoBehaviour
{
    //Стороны хранят фиксированный набор номеров мест, которые окружают соответствующий центр (8 подвижных элементов)
    [SerializeField]
    private int[] boundElements = new int[8];
    public int[] BoundElements { get { return boundElements; } }

    [SerializeField]
    private CubeSide sideColor;
    public CubeSide SideColor { get { return sideColor; } }

    private ColorPlate[,] sidePlates = new ColorPlate[3, 3];

    

    private List<Transform> tempChilds;

    private Cube cube;

    void Start()
    {
        
    }

    private void Awake()
    {
        cube = transform.parent.GetComponent<Cube>();
    }

    void Update()
    {
        //Orientation();
    }

    //void MarkElements()
    //{
    //    cube.MarkBindings(boundPositions, this);
    //}

    //При вращении стороны куб получает набор из 8 позиций
    //Элементы, расположенные на указанных позициях, нужно переставить на два индекса вперёд или назад
    public void RotateClockwise(bool writeAction)
    {
        if (!cube.IsRotationLocked)
        {
            cube.LockCube();
            tempChilds = cube.GetElementsByPlaces(boundElements);
            foreach (var element in tempChilds)
            {
                element.parent = transform;
            }
            cube.PositiveRotation(boundElements);
            StartCoroutine(RClockwise(writeAction));
        }
    }

    public void RotateCounterclockwise(bool writeAction)
    {
        if (!cube.IsRotationLocked)
        {
            cube.LockCube();
            tempChilds = cube.GetElementsByPlaces(boundElements);
            foreach (var element in tempChilds)
            {
                element.parent = transform;
            }
            cube.NegativeRotation(boundElements);
            StartCoroutine(RCounterclockwise(writeAction));
        }
    }

    public void RotateSide(int direction)
    {
        switch (direction)
        {
            case 0:
                RotateClockwise(false);
                break;
            case 1:
                RotateCounterclockwise(false);
                break;
        }
    }

    IEnumerator RClockwise(bool writeAction)
    {
        Quaternion targetRotation = transform.localRotation * Quaternion.Euler(0, 0, 90);

        float rotationSpeed = 1 / Game.Settings.SideRotationSpeed;
        while (transform.localRotation != targetRotation)
        {
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRotation, 90 * Time.deltaTime * rotationSpeed);
            yield return null;
        }

        foreach (var element in tempChilds)
        {
            element.parent = cube.transform;
        }
        if (writeAction) Game.Controller.WriteAction(new Turn(sideColor, true));
        cube.UnlockCube();
    }

    IEnumerator RCounterclockwise(bool writeAction)
    {
        Quaternion targetRotation = transform.localRotation * Quaternion.Euler(0, 0, -90);

        float rotationSpeed = 1 / Game.Settings.SideRotationSpeed;
        while (transform.localRotation != targetRotation)
        {
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRotation, 90 * Time.deltaTime * rotationSpeed);
            yield return null;
        }

        foreach(var element in tempChilds)
        {
            element.parent = cube.transform;
        }
        if (writeAction) Game.Controller.WriteAction(new Turn(sideColor, false));
        cube.UnlockCube();
    }

    

    public void Orientation()
    {
        if (Vector3.Angle(transform.forward, Vector3.left) < 45f)
        {
            cube.SetLeftSide(this);
        }
        if (Vector3.Angle(transform.forward, Vector3.back) < 45f)
        {
            cube.SetFrontSide(this);
        }
        if (Vector3.Angle(transform.forward, Vector3.up) < 45f)
        {
            cube.SetTopSide(this);
        }
    }
    
}
