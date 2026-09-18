using UnityEditor;

[InitializeOnLoad]
public static class GenerateSolution
{
    static GenerateSolution()
    {
        // Tự động yêu cầu Unity đồng bộ và sinh file .sln / .csproj khi mở Unity hoặc compile
        EditorApplication.delayCall += () =>
        {
            EditorApplication.ExecuteMenuItem("Assets/Open C# Project");
        };
    }

    [MenuItem("Tools/Generate C# Project Files")]
    public static void Sync()
    {
        EditorApplication.ExecuteMenuItem("Assets/Open C# Project");
    }
}
