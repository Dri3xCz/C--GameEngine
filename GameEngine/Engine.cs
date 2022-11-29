using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;


namespace GameEngine.GameEngine
{

    public class Canvas : Form
    {
        public Canvas()
        {
            DoubleBuffered = true;
        }
    }

    public abstract class Engine
    {
        private Vector2 ScreenSize = new Vector2(512,512);
        private string Title = "New Game";
        private Canvas Window;
        private Thread thread;
        public static List<Shape> Shapes = new List<Shape>();
        public static List<Keys> keys = new List<Keys>();
        public Color BackgroundColor = Color.White;
        public Engine(Vector2 screenSize, string Title) 
        {
            this.ScreenSize = screenSize;
            this.Title = Title;

            Window = new Canvas();
            Window.Size = new Size((int)this.ScreenSize.X, (int)this.ScreenSize.Y);
            Window.Name = this.Title;
            Window.Paint += Renderer;
            Window.KeyDown += Window_KeyDown;
            Window.KeyUp += Window_KeyUp;

            thread = new Thread(GameLoop);
            thread.Start();


            Application.Run(Window);
        }

        private void Window_KeyUp(object? sender, KeyEventArgs e)
        {
            keys.Remove(e.KeyCode);
            Log.Info("" + e.KeyCode + " has been removed");
        }

        private void Window_KeyDown(object? sender, KeyEventArgs e)
        {
            bool check_ = false;
            foreach (Keys key_ in keys)
            {
                if (key_ == e.KeyCode) 
                {
                    check_ = true;
                }
            }
            if (check_ == false)
            {
                keys.Add(e.KeyCode);
                Log.Info("" + e.KeyCode + " has been added");
            }
        }

        private void Renderer(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            foreach (Shape shape in Shapes) 
            {
                g.FillRectangle(new SolidBrush(shape.color), (int)shape.position.X, (int)shape.position.Y, (int)shape.scale.X, (int)shape.scale.Y);
            }
        }

        private void MaxPosition() 
        {
            foreach (Shape shape in Shapes)
            {
                if (shape.movable == false) 
                {
                    break;
                }
                shape.maxPosX = new Vector2(999,-999);
                shape.maxPosY = new Vector2(999, -999);
                foreach (Shape shape_ in Shapes)
                { 
                    if (shape.position.Y + shape.scale.Y > shape_.position.Y && shape.position.Y < shape_.position.Y + shape_.scale.Y && shape_.tag != shape.tag) 
                    {
                        if (shape.position.X + shape.scale.X <= shape_.position.X) 
                        {
                            if (shape.maxPosX.X >= shape_.position.X || shape.maxPosX.X == 0) 
                            {
                                shape.maxPosX.X = shape_.position.X;
                            }
                        }
                        else 
                        {
                            if (shape.maxPosX.Y <= shape_.position.X + shape_.scale.X || shape.maxPosX.Y == 0)
                            {
                                shape.maxPosX.Y = shape_.position.X + shape_.scale.X;
                            }
                        }
                    }

                    if (shape.position.X + shape.scale.X > shape_.position.X && shape.position.X < shape_.position.X + shape_.scale.X && shape_.tag != shape.tag)
                    {
                        if (shape.position.Y + shape.scale.Y <= shape_.position.Y)
                        {
                            if (shape.maxPosY.X >= shape_.position.Y || shape.maxPosY.X == 0)
                            {
                                shape.maxPosY.X = shape_.position.Y;
                            }
                        }
                        else
                        {
                            if (shape.maxPosY.Y <= shape_.position.Y + shape_.scale.Y || shape.maxPosY.Y == 0)
                            {
                                shape.maxPosY.Y = shape_.position.Y + shape_.scale.Y;
                            }
                        }
                    }
                }
            }
        }

        public void GameLoop() 
        {
            OnLoad();
            Window.BackColor = BackgroundColor;
            Log.Info("Background has been set to: " + BackgroundColor);
            while (thread.IsAlive) 
            {
                try
                {
                    OnDraw();
                    Window.BeginInvoke((MethodInvoker)delegate { Window.Refresh(); });
                    MaxPosition();
                    OnUpdate();
                    Thread.Sleep(1);
                }
                catch 
                {
                    Log.Error("Game Window not found...");
                }
            }  
        }

        static public void AddShape(Shape shape) 
        {
            Shapes.Add(shape);
        }
        static public void RemoveShape(Shape shape)
        {
            Shapes.Remove(shape);
        }

        static public bool GetActiveKeys(Keys key) 
        {
            foreach (Keys key_ in keys)
            {
                if (key_ == key) 
                {
                    return true;
                }
            } 
            return false;
        }

        public abstract void OnLoad();
        public abstract void OnDraw();
        public abstract void OnUpdate();
    }
}
