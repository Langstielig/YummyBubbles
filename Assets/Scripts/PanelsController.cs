using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelsController : MonoBehaviour
{
    [SerializeField] private TMP_Text countOfTurnsText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private GameObject loseMessage;

    private int countOfTurns;
    private int score;

    private void Awake()
    {
        countOfTurns = 10;
        score = 0;

        countOfTurnsText.text = countOfTurns.ToString();
        scoreText.text = score.ToString();

        loseMessage.SetActive(false);
    }

    public void ChangeCountOfTurns(int num)
    {
        countOfTurns += num;
        countOfTurnsText.text = countOfTurns.ToString();

        if(countOfTurns <= 0)
        {
            int max = 0;
            int countOfNullLines = 0;

            for(int i = 0; i < DataHolder.lines.Length; i++)
            {
                if (DataHolder.lines[i] != null)
                {
                    if (DataHolder.lines[i].score > max)
                    {
                        max = DataHolder.lines[i].score;
                    }
                }
                else
                {
                    countOfNullLines++;
                }
            }

            if (score > max || countOfNullLines > 0)
            {
                for(int i = 0; i < DataHolder.lines.Length; i++)
                {
                    if (DataHolder.lines[i] == null || i == DataHolder.lines.Length - 1)
                    {
                        LeaderboardLine newLine = new LeaderboardLine();
                        newLine.date = System.DateTime.Now.ToString("dd.MM.yyyy");
                        newLine.score = score;
                        newLine.isNewLine = true;
                        DataHolder.lines[i] = newLine;
                        break;
                    }
                }
                SceneManager.LoadScene(3);
            }

            else
            {
                loseMessage.SetActive(true);
            }
        }
    }

    public void ChangeScore(int num)
    {
        score += num;
        scoreText.text = score.ToString();
    }
}
