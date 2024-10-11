using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaderboardUIController : MonoBehaviour
{
    [SerializeField] private GameObject lines;
    [SerializeField] private GameObject leaderboardLine;

    private int numberOfLine = 1;
    private string pattern = "{0}. {1} - {2} points";

    private void Start()
    {
        FillLeaderboardLines();
    }

    public void GoToMainMenuScene()
    {
        SceneManager.LoadScene(0);
    }

    public void FillLeaderboardLines()
    {
        for(int i = 0; i < DataHolder.lines.Length; i++)
        {
            if (DataHolder.lines[i] != null)
            {
                GameObject currentLine = Instantiate(leaderboardLine);
                string lineText = string.Format(pattern, numberOfLine, DataHolder.lines[i].date, DataHolder.lines[i].score);
                currentLine.GetComponent<TMP_Text>().text = lineText;
                currentLine.transform.SetParent(lines.transform);
                currentLine.transform.localScale = new Vector3(1f, 1f, 1f);

                if (DataHolder.lines[i].isNewLine)
                {
                    Animator animator = currentLine.GetComponent<Animator>();
                    animator.SetBool("isHighlighting", true);

                    DataHolder.lines[i].isNewLine = false;
                }

                numberOfLine++;
            }
        }
    }
}
