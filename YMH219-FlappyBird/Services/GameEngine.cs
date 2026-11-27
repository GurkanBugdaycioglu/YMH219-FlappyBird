using System;
using System.Collections.Generic;
using System.Drawing;
using YMH219_FlappyBird.Models;

namespace YMH219_FlappyBird.Services;

public class GameEngine
{
    public Bird Player { get; }
    public List<Pipes> Pipes { get; } = new();
    public GameState State { get; } = new();
    private readonly int _width;
    private readonly int _groundY;
    private readonly Random _random = new();
    private const int PipeSpacing = 280;
    public GameEngine(int width, int height, int groundHeight)
    {
        _width = width;
        _groundY = height - groundHeight;
        Player = new Bird(100, _groundY / 2f);
        InitializePipes();
    }
    private void InitializePipes()
    {
        Pipes.Clear();
        for (int i = 0; i < 3; i++)
        {
            Pipes.Add(new Pipes(_width + (i * PipeSpacing), _groundY, _random));
        }
    }
    public void StartOrRestart()
    {
        Player.Reset(_groundY / 2f);
        InitializePipes();
        State.Reset();
        Player.Flap();
    }
    public void HandleJumpInput()
    {
        if (State.Status == GameStatus.Ready)
        {
            StartOrRestart();
            return;
        }
        if (State.Status == GameStatus.GameOver)
        {
            if (State.CanRestart)
            {
                StartOrRestart();
            }
            return;
        }
        if (State.Status == GameStatus.Playing)
        {
            Player.Flap();
        }
    }
    public void Update()
    {
        if (State.Status != GameStatus.Playing) return;
        Player.Update();
        if (Player.Y + Player.Height >= _groundY)
        {
            State.SetGameOver();
            return;
        }

        foreach (var pipe in Pipes)
        {
            pipe.Move(State.CurrentSpeed);
            if (Player.Bounds.IntersectsWith(pipe.TopPipeBounds) ||
                Player.Bounds.IntersectsWith(pipe.BottomPipeBounds))
            {
                State.SetGameOver();
                return;
            }
            if (!pipe.IsScored && pipe.X + pipe.Width < Player.X)
            {
                pipe.IsScored = true;
                State.IncrementScore(State.HighScore);
            }
            if (pipe.X + pipe.Width < 0)
            {
                float maxX = 0;
                foreach (var p in Pipes)
                {
                    if (p.X > maxX) maxX = p.X;
                }
                pipe.Reposition(maxX + PipeSpacing);
            }
        }
    }
}