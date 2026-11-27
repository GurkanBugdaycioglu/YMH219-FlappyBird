using System;
using System.IO;

namespace YMH219_FlappyBird.Services;

public class ScoreManager
{
    private readonly string _filePath;
    public ScoreManager(string fileName = "highscore.txt")
    {
        _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
    }
    public int LoadHighScore()
    {
        try
        {
            if (File.Exists(_filePath))
            {
                string content = File.ReadAllText(_filePath);
                if (int.TryParse(content, out int highScore))
                {
                    return highScore;
                }
            }
        }
        catch{}
        return 0;
    }
    public void SaveHighScore(int score)
    {
        try
        {
            File.WriteAllText(_filePath, score.ToString());
        }
        catch{}
    }
}