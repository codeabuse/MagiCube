using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{    
    [SerializeField]
    private Cube cube;
    private GameObject container;
    [SerializeField]
    private float rotationSensivity;
    [SerializeField]
    private float verticalRotationThreshold;
    [SerializeField]
    private int verticalThreshold;
    private bool verticalRotation;
    private bool isShuffleMode;
    public bool IsShuffleMode { get { return isShuffleMode; } }

    private Vector3 mousePositionDelta, lastMousePosition;

    private List<Turn> actionList = new List<Turn>();
    

    void Start()
    {
        
    }

    private void Awake()
    {
        container = cube.transform.parent.gameObject;
        verticalThreshold = (int)(Screen.height * verticalRotationThreshold);
        Game.RegisterController(this);
    }

    void Update()
    {
        if (!isShuffleMode)
        {
            AdaptiveControl();
        }
        MouseRotate();
    }

    public void ShuffleCube()
    {
        StartCoroutine(Shuffle(20));
        actionList.Clear();
    }

    IEnumerator Shuffle(int steps)
    {
        isShuffleMode = true;
        List<Turn> instructions = new List<Turn>();
        Turn lastGenerated = new Turn((CubeSide)Random.Range(0, 6), Random.Range(0, 2) == 0 ? true : false);
        instructions.Add(lastGenerated);
        for (int i = 0; i < steps; i++)
        {
            Turn generated = new Turn((CubeSide)Random.Range(0, 6), Random.Range(0, 2) == 0 ? true : false);
            if (generated.Side == lastGenerated.Side)
            {
                generated.Side = (CubeSide)Random.Range(0, 6);
            }
            instructions.Add(generated);
            lastGenerated = generated;
        }
        while (steps > 0)
        {
            if (!cube.IsRotationLocked)
            {
                //cube.Sides[Random.Range(0, 6)].RotateSide(Random.Range(0, 2));
                DoAction(instructions[steps], false);
                steps--;
            }
            yield return null;
        }
        isShuffleMode = false;
    }

    public void UndoAction()
    {
        if (actionList.Count > 0)
        {
            Turn undo = Turn.Inverse(actionList[actionList.Count - 1]);
            actionList.RemoveAt(actionList.Count - 1);
            Side selected = cube.Sides.Find(x => x.SideColor == undo.Side);
            if (undo.Direction) selected.RotateClockwise(false);
            else selected.RotateCounterclockwise(false);
        }
    }
    
    void DoAction(Turn action, bool writeAction)
    {
        Side selected = cube.Sides.Find(x => x.SideColor == action.Side);
        if (action.Direction) selected.RotateClockwise(writeAction);
        else selected.RotateCounterclockwise(writeAction);
    }

    public void WriteAction(Turn action)
    {
        actionList.Add(action);
    }

    void MouseRotate()
    {
        mousePositionDelta = Input.mousePosition - lastMousePosition;
        lastMousePosition = Input.mousePosition;

        if (Input.GetKey(KeyCode.Mouse1))
        {

            if (!cube.IsRotationLocked)
            {
                container.transform.Rotate(Vector3.up, -mousePositionDelta.x * rotationSensivity, Space.World);
                cube.UpdateOrientation();
            }
            StepRotateX();
        }
        KeyboardShortcuts();
    }

    void StepRotateX()
    {
        if (Mathf.Abs(mousePositionDelta.y) > verticalThreshold & !verticalRotation)
        {
            verticalRotation = true;
            cube.LockCube();
            StartCoroutine(RotateCube(mousePositionDelta.y > 0 ? -cube.Left.transform.forward : cube.Left.transform.forward, 90, 4));
        }
    }

    IEnumerator RotateCube(Vector3 axis, float angle, float speed)
    {
        float rotation = angle;
        float currentStep;
        float report = 0;

        while (rotation > 0)
        {
            currentStep = angle * Time.deltaTime * speed;
            if (rotation - currentStep < 0)
            {
                currentStep += rotation - currentStep;
            }
            report += currentStep;
            cube.transform.RotateAround(cube.transform.position, axis, currentStep);
            rotation -= currentStep;
            yield return null;
        }
        //Debug.Log("Rotation finished at " + report.ToString() + " degrees");
        verticalRotation = false;
        cube.UpdateOrientation();
        cube.UnlockCube();
    }

    void AdaptiveControl()
    {
        //Левая сторона
        if (Input.GetKeyDown(KeyCode.A) & !Input.GetKeyDown(KeyCode.Q))
        {
            cube.Left.RotateClockwise(true);
        }
        if (Input.GetKeyDown(KeyCode.Q) & !Input.GetKeyDown(KeyCode.A))
        {
            cube.Left.RotateCounterclockwise(true);
        }

        //Правая сторона
        if (Input.GetKeyDown(KeyCode.R) & !Input.GetKeyDown(KeyCode.F))
        {
            cube.Right.RotateClockwise(true);
        }
        if (Input.GetKeyDown(KeyCode.F) & !Input.GetKeyDown(KeyCode.R))
        {
            cube.Right.RotateCounterclockwise(true);
        }

        //Верхняя сторона
        if (Input.GetKeyDown(KeyCode.W) & !Input.GetKeyDown(KeyCode.E))
        {
            cube.Top.RotateClockwise(true);
        }
        if (Input.GetKeyDown(KeyCode.E) & !Input.GetKeyDown(KeyCode.W))
        {
            cube.Top.RotateCounterclockwise(true);
        }

        //Передняя сторона
        if (Input.GetKeyDown(KeyCode.D) & !Input.GetKeyDown(KeyCode.S))
        {
            cube.Front.RotateClockwise(true);
        }
        if (Input.GetKeyDown(KeyCode.S) & !Input.GetKeyDown(KeyCode.D))
        {
            cube.Front.RotateCounterclockwise(true);
        }

        //Нижняя сторона
        if (Input.GetKeyDown(KeyCode.C) & !Input.GetKeyDown(KeyCode.X))
        {
            cube.Bottom.RotateClockwise(true);
        }
        if (Input.GetKeyDown(KeyCode.X) & !Input.GetKeyDown(KeyCode.C))
        {
            cube.Bottom.RotateCounterclockwise(true);
        }
    }

    void KeyboardShortcuts()
    {
        if ( Input.GetKeyDown(KeyCode.Tab))//Input.GetKey(KeyCode.LeftControl) &
        {
            UndoAction();
        }
    }

    void DetectCubeClick()
    {
        if (Input.GetMouseButtonDown(0))
        {

        }
    }

    void TrackMouse()
    {
        StartCoroutine(TrackMouseControl());
    }

    IEnumerator TrackMouseControl()
    {
        lastMousePosition = Vector3.zero;
        while (Input.GetMouseButton(0))
        {
            mousePositionDelta = Input.mousePosition - lastMousePosition;
            lastMousePosition = Input.mousePosition;
            if (Mathf.Abs(mousePositionDelta.x) > 5)
            {

            }
            else if (Mathf.Abs(mousePositionDelta.y) > 5)
            {

            }
            yield return null;
        }
        
    }
}

public enum CubeSide
{
    White,
    Yellow,
    Blue,
    Red,
    Green,
    Orange
}
