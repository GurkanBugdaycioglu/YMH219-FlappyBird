using System;
using System.Drawing;

namespace YMH219_FlappyBird.Models;

public class Pipes
{
    public float X { get; private set; }
    public int Width { get; } = 65;
    public int GapY { get; private set; }
    public int GapHeight { get; } = 145;
    public bool IsScored { get; set; }
    private readonly int _playfieldHeight;
    private readonly Random _random;
    public RectangleF TopPipeBounds => new(X, 0, Width, GapY);
    public RectangleF BottomPipeBounds => new(X, GapY + GapHeight, Width, _playfieldHeight - (GapY + GapHeight));
    public Pipes(float startX, int playfieldHeight, Random random)
    {
        X = startX;
        _playfieldHeight = playfieldHeight;
        _random = random;
        RandomizeGap();
    }
    public void Move(float speed) => X -= speed;
    public void Reposition(float newX)
    {
        X = newX;
        IsScored = false;
        RandomizeGap();
    }
    private void RandomizeGap()
    {
        int margin = 70;
        GapY = _random.Next(margin, _playfieldHeight - GapHeight - margin);
    }
}