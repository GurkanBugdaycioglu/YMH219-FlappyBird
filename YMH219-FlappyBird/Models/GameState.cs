using System;

namespace YMH219_FlappyBird.Models;

public enum GameStatus
{
    Ready,
    Playing,
    GameOver
}

public class GameState
{
    public GameStatus Status { get; private set; } = GameStatus.Ready;
    public int Score { get; private set; }
    public int HighScore { get; private set; }
    public float BaseSpeed { get; private set; } = 4.0f;
    public float CurrentSpeed => BaseSpeed + (Score * 0.15f);
    private long _gameOverTick;
    private const long RestartDelayMs = 500;
    public bool CanRestart => Status == GameStatus.GameOver &&
                              (Environment.TickCount64 - _gameOverTick) >= RestartDelayMs;
    public void SetGameOver()
    {
        if (Status == GameStatus.GameOver) return;
        Status = GameStatus.GameOver;
        _gameOverTick = Environment.TickCount64;
    }
    public void Reset()
    {
        Score = 0;
        Status = GameStatus.Playing;
    }
    public void IncrementScore(int highScore)
    {
        Score++;
        if (Score > highScore)
        {
            HighScore = Score;
        }
    }
    public void SetInitialHighScore(int highScore) => HighScore = highScore;
}