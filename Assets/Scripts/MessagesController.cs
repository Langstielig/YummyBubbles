using UnityEngine;
using UnityEngine.SceneManagement;

public class MessagesController : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuMessage;
    [SerializeField] private GameObject tutorial;

    private void Awake()
    {
        mainMenuMessage.SetActive(false);
        tutorial.SetActive(true);
    }

    public void GoToMainMenuScene()
    {
        SceneManager.LoadScene(0);
    }

    public void OpenOrCloseMainMenuMessage()
    {
        bool isActive = mainMenuMessage.activeSelf;
        mainMenuMessage.SetActive(!isActive);

        BubblesGenerator bubblesGenerator = FindObjectOfType<BubblesGenerator>();
        if(bubblesGenerator != null)
        {
            bubblesGenerator.ChangeGameStatus(isActive);
        }
    }

    public void CloseTutorial()
    {
        tutorial.SetActive(false);
    }
}
