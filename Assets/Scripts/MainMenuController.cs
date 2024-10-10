using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] GameObject mainMenuWindow;
    [SerializeField] GameObject confirmWindow;

    private void Start()
    {
        LeaderboardController leaderboardController = FindObjectOfType<LeaderboardController>();
        if (leaderboardController != null)
        {
            leaderboardController.WriteToFile();
        }
    }

    public void GoToGameScene()
    {
        SceneManager.LoadScene(1);
    }

    public void GoToLeaderboardScene()
    {
        SceneManager.LoadScene(3);
    }

    public void GoToAboutMenuScene()
    {
        SceneManager.LoadScene(2);
    }

    public void ShowOrHideConfirmWindow()
    {
        bool isActive = mainMenuWindow.activeSelf;
        mainMenuWindow.SetActive(!isActive);

        isActive = confirmWindow.activeSelf;
        confirmWindow.SetActive(!isActive);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
