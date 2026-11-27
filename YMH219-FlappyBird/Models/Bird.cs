using System;
using System.Drawing;

namespace YMH219_FlappyBird.Models;

public class Bird
{
    public float X { get; }
    public float Y { get; private set; }
    public int Width { get; } = 36;
    public int Height { get; } = 28;
    public float VelocityY { get; private set; }
    private const float Gravity = 0.55f;
    private const float JumpForce = -8.5f;
    private DateTime _lastFlapTime = DateTime.MinValue;
    private const double FlapCooldownSeconds = 0.15; // Basılı tutmayı engelleyen 150ms koruma
    public RectangleF Bounds => new(X, Y, Width, Height);
    public Bird(float startX, float startY)
    {
        X = startX;
        Y = startY;
    }
    public void Flap()
    {
        if ((DateTime.UtcNow - _lastFlapTime).TotalSeconds < FlapCooldownSeconds)
            return;

        _lastFlapTime = DateTime.UtcNow;
        VelocityY = JumpForce;
    }
    public void Update()
    {
        VelocityY += Gravity;
        Y += VelocityY;
        if (Y < 0)
        {
            Y = 0;
            VelocityY = 0;
        }
    }
    public void Reset(float startY)
    {
        Y = startY;
        VelocityY = 0;
        _lastFlapTime = DateTime.MinValue;
    }
}