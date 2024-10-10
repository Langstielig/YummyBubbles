using System.IO;
using UnityEngine;

public class LeaderboardController : MonoBehaviour
{
    private string path;

    private void Awake()
    {
        //#if UNITY_ANDROID && !UNITY_EDITOR
        //        path = Application.dataPath + "leaderboard.csv";
        //#else
        //        path = Application.dataPath + "/Resources/" + "leaderboard.csv";

        //        //string path1 = Application.streamingAssetsPath;
        //        //Debug.Log("streamingAssetsPath " + path1);
        //#endif

        path = Application.persistentDataPath + "/leaderboard.csv";
        Debug.Log("path is " + path);

        ReadFromFile();
        BubbleSort();
    }

    private void ReadFromFile()
    {
        //TextAsset leaderboardData = Resources.Load<TextAsset>("leaderboard");
        StreamReader reader = new StreamReader(path);

        if (reader == null)
        {
            FileInfo file = new FileInfo(path);
            file.Create();

            reader = new StreamReader(path);

            Debug.Log("create csv file");
        }

        if (reader != null)
        {
            string[] data = reader.ReadToEnd().Split(new char[] { '\n' });
            int index = 0;

            for (int i = 0; i < data.Length && index < DataHolder.lines.Length; i++)
            {
                string[] row = data[i].Split(new char[] { ';' });
                if (row.Length > 1)
                {
                    LeaderboardLine line = new LeaderboardLine();
                    line.date = row[0];
                    line.score = int.Parse(row[1]);
                    DataHolder.lines[index] = line;
                    index++;
                }
            }

            Debug.Log("read from csv file");
        }
    }

    public void WriteToFile()
    {
        StreamWriter cleaner = new StreamWriter(path, false);
        cleaner.WriteLine("");
        cleaner.Close();

        StreamWriter writer = new StreamWriter(path, true);
        for (int i = 0; i < DataHolder.lines.Length; i++)
        {
            if (DataHolder.lines[i] != null)
            {
                string str = DataHolder.lines[i].date + ';' + DataHolder.lines[i].score.ToString();
                writer.WriteLine(str);
                Debug.Log("write to csv file");
            }
        }
        writer.Close();
    }

    private void BubbleSort()
    {
        bool swapped;
        for(int i = 0; i < DataHolder.lines.Length; i++)
        {
            swapped = false;
            for(int j = 0; j < DataHolder.lines.Length - i - 1; j++)
            {
                if (DataHolder.lines[j] != null && DataHolder.lines[j + 1] != null && 
                    DataHolder.lines[j].score < DataHolder.lines[j + 1].score)
                {
                    LeaderboardLine buffer = DataHolder.lines[j];
                    DataHolder.lines[j] = DataHolder.lines[j + 1];
                    DataHolder.lines[j + 1] = buffer;
                    swapped = true;
                }
            }

            if(!swapped)
            {
                break;
            }
        }
    }
}
