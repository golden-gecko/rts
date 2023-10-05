using System.IO;

public class Config
{
    public class Asset
    {
        public static string Disasters { get; } = Path.Join("Objects", "Disasters");
        public static string Structures { get; } = Path.Join("Objects", "Structures");
        public static string Units { get; } = Path.Join("Objects", "Units");
    }

    public class Camera
    {
        public static float MinHeight { get; } = 2.0f;
        public static float MaxHeight { get; } = 100.0f;
    }

    public class Cursor3D
    {
        public static float RotateStep { get; } = 45.0f;
        public static bool SnapToGrid { get; } = true;
    }

    public class Indicator
    {
        public static float Margin { get; } = 1.1f;
        public static float TextOffset { get; } = 2.0f;
    }

    public class Map
    {
        public static float ConstructionScale { get; } = 2.0f;
        public static float MaxHeight { get; } = 2000.0f;
        public static float VisibilityScale { get; } = 2.0f;
        public static int VisibilitySize { get; } = 250;
    }

    public class UI
    {
    }
}
