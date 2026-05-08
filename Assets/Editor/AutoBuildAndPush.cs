using UnityEditor;
using UnityEngine;
using System.Diagnostics;
using System.IO;
using UnityEditor.Build.Reporting;

public class AutoBuildAndPush : EditorWindow
{
    // CẤU HÌNH THÔNG TIN ITCH.IO
    private const string ItchUsername = "vpthanh";
    private const string GameSlot = "uit-game-20-years";
    private const string BuildPath = @"C:\Unity assets\U20Y_WebBuild"; // Thư mục sẽ build ra

    [MenuItem("UIT Tools/Build and Push to Itch.io")]
    public static void BuildAndPush()
    {
        // 1. Cấu hình Build
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = GetScenePaths();
        buildPlayerOptions.locationPathName = BuildPath;
        buildPlayerOptions.target = BuildTarget.WebGL;
        buildPlayerOptions.options = BuildOptions.None;

        UnityEngine.Debug.Log("🚀 Đang bắt đầu Build WebGL...");
        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            UnityEngine.Debug.Log("✅ Build thành công! Đang đẩy lên Itch.io bằng Butler...");
            PushToItch();
        }
        else
        {
            UnityEngine.Debug.LogError("❌ Build thất bại.");
        }
    }

    private const string ButlerExecutablePath = @"C:\Users\Admin\Downloads\butler-windows-amd64-head\butler.exe";

    private static void PushToItch()
    {
        // 1. Kiểm tra file Butler có tồn tại không
        if (!File.Exists(ButlerExecutablePath))
        {
            UnityEngine.Debug.LogError($"❌ Không thấy Butler tại: {ButlerExecutablePath}");
            return;
        }

        // 2. Chuẩn hóa đường dẫn (Đổi hết gạch ngược \ thành gạch xuôi / để tránh lỗi thoát chuỗi)
        string cleanButlerPath = ButlerExecutablePath.Replace("\\", "/");
        string cleanBuildPath = BuildPath.Replace("\\", "/");

        // 3. Công thức CMD thần thánh: cmd /k ""đường dẫn 1" "đường dẫn 2" ..."
        // Phải có 2 dấu ngoặc kép ở đầu và cuối cụm lệnh
        string command = $"/k \"\"{cleanButlerPath}\" push \"{cleanBuildPath}\" {ItchUsername}/{GameSlot}:webgl\"";

        ProcessStartInfo processInfo = new ProcessStartInfo();
        processInfo.FileName = "cmd.exe";
        processInfo.Arguments = command;
        processInfo.CreateNoWindow = false;
        processInfo.UseShellExecute = true;

        try
        {
            UnityEngine.Debug.Log("🚀 Đang gọi Butler đẩy game lên Itch.io...");
            Process.Start(processInfo);
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.LogError("❌ Lỗi thực thi: " + e.Message);
        }
    }

    private static string[] GetScenePaths()
    {
        string[] scenes = new string[EditorBuildSettings.scenes.Length];
        for (int i = 0; i < scenes.Length; i++)
        {
            scenes[i] = EditorBuildSettings.scenes[i].path;
        }
        return scenes;
    }
}