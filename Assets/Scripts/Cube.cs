using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField]
    private List<Side> sides = new List<Side>();
    public List<Side> Sides { get { return sides; } }

    public Side White { get { return sides[0]; } }
    public Side Yellow { get { return sides[1]; } }
    public Side Red { get { return sides[2]; } }
    public Side Blue { get { return sides[3]; } }
    public Side Orange { get { return sides[4]; } }
    public Side Green { get { return sides[5]; } }

    private Side left, right, front, back, top, bottom;

    public Side Left { get { return left; } }
    public Side Right { get { return right; } }
    public Side Front { get { return front; } }
    public Side Back { get { return back; } }
    public Side Top { get { return top; } }
    public Side Bottom { get { return bottom; } }

    //Куб состоит из 20 подвижных элементов, не считая шесть центральных, которые своего положения в кубе не меняют.
    //Места элементов (виртуальные ячейки) пронумерованы и при вращении любой из сторон элементы переставляются в ячейках.
    [SerializeField]
    private Element[] elements = new Element[20];
    [SerializeField]
    private ColorPlate[,] whitePlates = new ColorPlate[3, 3];
    [SerializeField]
    private ColorPlate[,] yellowPlates = new ColorPlate[3, 3];
    [SerializeField]
    private ColorPlate[,] bluePlates = new ColorPlate[3, 3];
    [SerializeField]
    private ColorPlate[,] redPlates = new ColorPlate[3, 3];
    [SerializeField]
    private ColorPlate[,] greenPlates = new ColorPlate[3, 3];
    [SerializeField]
    private ColorPlate[,] orangePlates = new ColorPlate[3, 3];

    private Dictionary<CubeSide, ColorPlate[,]> plates = new Dictionary<CubeSide, ColorPlate[,]>();

    private bool isRotationLocked = false;
    public bool IsRotationLocked { get { return isRotationLocked; } }

    void Start()
    {
        plates.Add(CubeSide.White, whitePlates);
        plates.Add(CubeSide.Yellow, yellowPlates);
        plates.Add(CubeSide.Blue, bluePlates);
        plates.Add(CubeSide.Red, redPlates);
        plates.Add(CubeSide.Green, greenPlates);
        plates.Add(CubeSide.Orange, orangePlates);
        UpdateOrientation();
        UpdateElementBindings();
    }

    private void Awake()
    {
        
    }

    void Update()
    {
        
    }

    void BindElementsToSide(Side side)
    {
        foreach (int position in side.BoundElements)
        {
            elements[position].BindToSide(side);
        }
    }

    void UpdateElementBindings()
    {
        foreach (var element in elements)
        {
            element.ResetBindings();
        }
        foreach (var side in sides)
        {
            BindElementsToSide(side);
        }
    }

    public void SetLeftSide(Side side)
    {
        if (!isRotationLocked)
        {
            left = side;
            right = sides.Find(x => x.SideColor == GetOppositeSide(side.SideColor));
            //Debug.Log(side.SideColor.ToString() + " side set as left");
        }
    }

    public void SetFrontSide(Side side)
    {
        if (!isRotationLocked)
        {
            front = side;
            back = sides.Find(x => x.SideColor == GetOppositeSide(side.SideColor));
            //Debug.Log(side.SideColor.ToString() + " side set as front");
        }
    }

    public void SetTopSide(Side side)
    {
        if (!isRotationLocked)
        {
            top = side;
            bottom = sides.Find(x => x.SideColor == GetOppositeSide(side.SideColor));
            //Debug.Log(side.SideColor.ToString() + " side set as top");
        }
    }

    //Возвращает цвет стороны, противоположный указанному
    private CubeSide GetOppositeSide(CubeSide side)
    {
        switch (side)
        {
            case CubeSide.White:
                return CubeSide.Yellow;
            case CubeSide.Yellow:
                return CubeSide.White;
            case CubeSide.Blue:
                return CubeSide.Red;
            case CubeSide.Red:
                return CubeSide.Blue;
            case CubeSide.Green:
                return CubeSide.Orange;
            case CubeSide.Orange:
                return CubeSide.Green;
            default:
                return CubeSide.White;
        }
    }

    public void UpdateOrientation()
    {
        foreach (var side in sides)
        {
            side.Orientation();
        }
    }

    //Перестановка при вращении по часовой стрелке
    public void PositiveRotation(int[] positions)
    {
        Element[] temp = new Element[8];
        int index = 0;

        foreach (int position in positions)
        {
            temp[index] = elements[position];
            index++;
        }

        for (int i = 0; i < 8; i++)
        {
            int pointer = i + 2;
            if (pointer >= 8) pointer = pointer - 8;
            elements[positions[pointer]] = temp[i];
            //Debug.Log(string.Format("Element from pos.{0} moved to pos {1}", positions[i], positions[pointer]));
        }
    }

    //Перестановка при вращении против часовой стрелки
    public void NegativeRotation(int[] positions)
    {
        Element[] temp = new Element[8];
        int index = 0;

        foreach (int position in positions)
        {
            temp[index] = elements[position];
            index++;
        }

        for (int i = 0; i < 8; i++)
        {
            int pointer = i - 2;
            if (pointer < 0) pointer = 8 + pointer;
            elements[positions[pointer]] = temp[i];
        }
    }

    public void LockCube()
    {
        isRotationLocked = true;
    }

    public void UnlockCube()
    {
        UpdateElementBindings();
        isRotationLocked = false;
    }

    public List<Transform> GetElementsByPlaces(int[] places)
    {
        List<Transform> picked = new List<Transform>();
        foreach (int number in places)
        {
            picked.Add(elements[number].transform);
        }
        return picked;
    }
}

public struct Turn
{
    public CubeSide Side;
    public bool Direction;

    public Turn(CubeSide side, bool direction)
    {
        this.Side = side;
        this.Direction = direction;
    }

    public static Turn WhiteClock { get { return new Turn(CubeSide.White, true); } }
    public static Turn WhiteCounter { get { return new Turn(CubeSide.White, false); } }
    public static Turn YellowClock { get { return new Turn(CubeSide.Yellow, true); } }
    public static Turn YellowCounter { get { return new Turn(CubeSide.Yellow, false); } }
    public static Turn BlueClock { get { return new Turn(CubeSide.Blue, true); } }
    public static Turn BlueCounter { get { return new Turn(CubeSide.Blue, false); } }
    public static Turn RedClock { get { return new Turn(CubeSide.Red, true); } }
    public static Turn RedCounter { get { return new Turn(CubeSide.Red, false); } }
    public static Turn GreenClock { get { return new Turn(CubeSide.Green, true); } }
    public static Turn GreenCounter { get { return new Turn(CubeSide.Green, false); } }
    public static Turn OrangeClock { get { return new Turn(CubeSide.Orange, true); } }
    public static Turn OrangeCounter { get { return new Turn(CubeSide.Orange, false); } }

    public static Turn Inverse(Turn instruction)
    {
        return new Turn(instruction.Side, !instruction.Direction);
    }
}
