using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using YMH219_FlappyBird.Models;
using YMH219_FlappyBird.Services;

namespace YMH219_FlappyBird;

public partial class MainForm : Form
{
    private readonly GameEngine _engine;
    private readonly ScoreManager _scoreManager;
    private readonly System.Windows.Forms.Timer _gameTimer;
    private const int GroundHeight = 70;

    public MainForm()
    {
        InitializeComponent();

        this.Text = "YMH219 - Flappy Bird";
        this.ClientSize = new Size(800, 550);
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;
        this.StartPosition = FormStartPosition.CenterScreen;
        this.DoubleBuffered = true;
        this.KeyPreview = true;

        _scoreManager = new ScoreManager();
        _engine = new GameEngine(this.ClientSize.Width, this.ClientSize.Height, GroundHeight);
        _engine.State.SetInitialHighScore(_scoreManager.LoadHighScore());

        _gameTimer = new System.Windows.Forms.Timer();
        _gameTimer.Interval = 16;
        _gameTimer.Tick += GameTimer_Tick;
        _gameTimer.Start();
    }

    private void GameTimer_Tick(object? sender, EventArgs e)
    {
        var previousStatus = _engine.State.Status;
        _engine.Update();

        if (previousStatus == GameStatus.Playing && _engine.State.Status == GameStatus.GameOver)
        {
            _scoreManager.SaveHighScore(_engine.State.HighScore);
        }
        this.Invalidate();
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);

        if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Up)
        {
            _engine.HandleJumpInput();
            e.Handled = true;
            e.SuppressKeyPress = true;
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        g.Clear(Color.FromArgb(112, 197, 206));

        using Brush pipeBrush = new SolidBrush(Color.FromArgb(115, 191, 46));
        using Pen pipeBorder = new Pen(Color.FromArgb(83, 141, 34), 3);

        foreach (var pipe in _engine.Pipes)
        {
            g.FillRectangle(pipeBrush, pipe.TopPipeBounds);
            g.DrawRectangle(pipeBorder, Rectangle.Round(pipe.TopPipeBounds));

            g.FillRectangle(pipeBrush, pipe.BottomPipeBounds);
            g.DrawRectangle(pipeBorder, Rectangle.Round(pipe.BottomPipeBounds));
        }

        int groundY = this.ClientSize.Height - GroundHeight;
        using Brush groundBrush = new SolidBrush(Color.FromArgb(222, 216, 149));
        using Brush grassBrush = new SolidBrush(Color.FromArgb(115, 191, 46));
        g.FillRectangle(groundBrush, 0, groundY, this.ClientSize.Width, GroundHeight);
        g.FillRectangle(grassBrush, 0, groundY, this.ClientSize.Width, 12);

        using Brush birdBrush = new SolidBrush(Color.FromArgb(247, 198, 49));
        using Pen birdBorder = new Pen(Color.FromArgb(180, 110, 20), 2);
        g.FillEllipse(birdBrush, _engine.Player.Bounds);
        g.DrawEllipse(birdBorder, _engine.Player.Bounds);

        g.FillEllipse(Brushes.White, _engine.Player.X + 22, _engine.Player.Y + 6, 8, 8);
        g.FillEllipse(Brushes.Black, _engine.Player.X + 24, _engine.Player.Y + 8, 4, 4);

        using Font scoreFont = new Font("Segoe UI", 16, FontStyle.Bold);
        g.DrawString($"Skor: {_engine.State.Score}", scoreFont, Brushes.White, 15, 15);
        g.DrawString($"En Yüksek: {_engine.State.HighScore}", scoreFont, Brushes.Gold, 15, 45);

        if (_engine.State.Status == GameStatus.Ready)
        {
            DrawCenterOverlay(g, "FLAPPY BIRD", "Başlamak ve zıplamak için SPACE tuşuna basın");
        }
        else if (_engine.State.Status == GameStatus.GameOver)
        {
            DrawCenterOverlay(g, "OYUN BİTTİ", $"Skorunuz: {_engine.State.Score} | Yeniden başlamak için SPACE tuşuna basın");
        }
    }
    private void DrawCenterOverlay(Graphics g, string title, string subtitle)
    {
        Rectangle banner = new Rectangle(150, 180, this.ClientSize.Width - 300, 130);
        using Brush bgBrush = new SolidBrush(Color.FromArgb(180, 30, 30, 30));
        g.FillRectangle(bgBrush, banner);
        g.DrawRectangle(Pens.White, banner);

        using Font titleFont = new Font("Segoe UI", 20, FontStyle.Bold);
        using Font subFont = new Font("Segoe UI", 11, FontStyle.Regular);
        StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

        g.DrawString(title, titleFont, Brushes.White, new Rectangle(banner.X, banner.Y + 15, banner.Width, 45), sf);
        g.DrawString(subtitle, subFont, Brushes.LightGray, new Rectangle(banner.X, banner.Y + 65, banner.Width, 40), sf);
    }
}