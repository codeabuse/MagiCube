using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Game
{
    private static Settings gameSettings = new Settings();
    public static Settings Settings { get { return gameSettings; } }

    private static CubeController controller;
    public static CubeController Controller { get { return controller; } }

    public static void RegisterController(CubeController ctrl)
    {
        controller = ctrl;
    }
}

[System.Serializable]
public class Settings
{
    private float sideRotationSpeed;
    public float SideRotationSpeed { get { return sideRotationSpeed; } }

    public Settings()
    {
        sideRotationSpeed = 0.2f;
    }
}
