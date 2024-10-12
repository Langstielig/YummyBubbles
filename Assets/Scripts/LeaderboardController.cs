using System.IO;
using UnityEngine;

public class LeaderboardController : MonoBehaviour
{
    private string path;

    private void Awake()
    {
        path = Application.persistentDataPath + "/leaderboard.csv";

        ReadFromFile();
        BubbleSort();
    }

    private void ReadFromFile()
    {
        StreamReader reader = new StreamReader(path);

        if (reader == null)
        {
            FileInfo file = new FileInfo(path);
            file.Create();

            reader = new StreamReader(path);
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
