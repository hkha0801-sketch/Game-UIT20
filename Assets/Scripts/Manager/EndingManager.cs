using UnityEngine;
using UnityEngine.Video;

public class EndingManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string videoName_Bad = "BadEnding.mp4";
    public string videoName_Good = "GoodEnding.mp4";

    void Start()
    {
        videoPlayer.source = VideoSource.Url;
        string fileName = CheckEndingCondition() ? videoName_Good : videoName_Bad;

        string fullPath = Application.streamingAssetsPath + "/" + fileName;
        
        videoPlayer.url = fullPath;
        videoPlayer.loopPointReached += OnVideoFinished;
        videoPlayer.Play();
        
        Debug.Log("🚀 Đang cố gắng chạy Video tại: " + fullPath);
    }

    private bool CheckEndingCondition()
    {
        if (MedalManager.Instance == null) return false;
        return MedalManager.Instance.HasMedal("4MedalFlag") && 
               MedalManager.Instance.HasMedal("8MedalFlag") && 
               MedalManager.Instance.HasMedal("12MedalFlag");
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        if (SceneController.Instance != null)
            SceneController.Instance.ChangeScene("Login");
    }
}