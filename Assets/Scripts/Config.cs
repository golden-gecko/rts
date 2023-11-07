using System.IO;
using UnityEngine;

public static class Config
{
    public static class Blueprints
    {
        public static string Directory = Path.Join(Application.persistentDataPath, "Blueprints");
    }

    public static class Camera
    {
        public static float MinHeight = 2.0f;
        public static float MaxHeight = 100.0f;
    }

    public static class Cursor3D
    {
        public static float RotateStep = 90.0f;
        public static bool SnapToGrid = true;
    }

    public static class DataLayer
    {
        public static Color ColorOccupation = new Color(1.0f, 1.0f, 1.0f, 0.25f);
        public static Color ColorExploration = new Color(1.0f, 1.0f, 1.0f, 0.25f);
        public static Color ColorPower = new Color(1.0f, 1.0f, 0.0f, 0.25f);
        public static Color ColorPowerRelay = new Color(1.0f, 1.0f, 0.5f, 0.25f);
        public static Color ColorRadar = new Color(0.0f, 0.0f, 1.0f, 0.25f);
        public static Color ColorSight = new Color(0.0f, 1.0f, 0.0f, 0.25f);
        public static Color ColorPassiveDamage = new Color(1.0f, 0.0f, 0.5f, 0.25f);
        public static Color ColorPassivePower = new Color(1.0f, 0.0f, 0.5f, 0.25f);
        public static Color ColorPassiveRange = new Color(1.0f, 0.0f, 0.5f, 0.25f);
        public static Color ColorEmpty = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    }

    public static class Editor
    {
        public static float RotateSpeed = 3.0f;
    }

    public static class Formation
    {
        public static float Spacing = 5.0f;
    }

    public static class Indicator
    {
        public static float Margin = 1.1f;
        public static float TextOffset = 2.0f;
    }

    public static class Map
    {
        public static float ConstructionScale = 1.0f;
        public static float DamageFactor = 0.0002f;
        public static float MaxHeight = 2000.0f;
        public static float Scale = 1.0f;
        public static int Size = 512;
    }
}
