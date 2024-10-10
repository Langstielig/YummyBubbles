using UnityEngine;
using UnityEngine.SceneManagement;

public class AboutMenuController : MonoBehaviour
{
    public void GoToMainMenuScene()
    {
        SceneManager.LoadScene(0);
    }
}
