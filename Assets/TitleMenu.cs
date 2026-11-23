using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// タイトル画面のメニューを担当するコンポーネント。
/// STARTボタンから呼ばれ、ステージのシーンを読み込む。
/// </summary>
public class TitleMenu : MonoBehaviour
{
    [SerializeField] private string stageSceneName = "Stage1"; // 開始時に読み込むシーン名

    /// <summary>
    /// STARTボタンから呼ばれ、ステージを開始する。
    /// </summary>
    public void StartGame()
    {
        SceneManager.LoadScene(stageSceneName);
    }
}