using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;

namespace GameEngine.GameEngine
{
    public class Shape
    {
        public Vector2 position = new Vector2(0, 0);
        public Vector2 lastPos = new Vector2(0, 0);
        public Vector2 scale = new Vector2(0,0);

        // maxPosX.X = rightMostPosition , maxPosX.Y = leftMostPosition, same for maxPosY (x lower, y upper)
        public Vector2 maxPosX = new Vector2(0, 0);
        public Vector2 maxPosY = new Vector2(0, 0);
        public bool movable = false;
        public bool isAppended = false;


        public string tag = string.Empty;
        public Color color = Color.Red;
        

        public List<Shape> appendedShapes = new List<Shape>();
        public bool cameraIsAppended = false;

        public Shape(Vector2 pos, Vector2 scale, string tag, Color color, bool movable) 
        {
            this.position = pos;
            this.lastPos = new Vector2(0,0);
            this.scale = scale;
            this.tag = tag;
            this.color = color;
            this.movable = movable;
            Engine.AddShape(this);
            Log.Info("A shape has been created, tag: " + this.tag);
        }

        public void DestroySef() 
        {
            Engine.RemoveShape(this);
        }

        public Rectangle HitBox() 
        {
            return new Rectangle((int)this.position.X, (int)this.position.Y, (int)this.scale.X, (int)this.scale.Y);
        }

        public void ShapeMove(Vector2 force) 
        {
            Vector2 oldpos = Vector2.Zero();
            oldpos.X = this.position.X;
            oldpos.Y = this.position.Y;

            foreach (Shape shape in appendedShapes)
            {
                //Zjistim kde je shape vuci base
                //nastavit maximalní hodnoty podle pozice
                //doufat
            }
           
            if (force.X > 0)
            {
                if (this.position.X + this.scale.X + force.X > maxPosX.X)
                {
                    this.position.X = maxPosX.X - this.scale.X;
                    Log.Warning("Collided with object. Stopping object");
                }
                else this.position.X += force.X;
            }
            else if(force.X < 0)
            {
                if (this.position.X + force.X < maxPosX.Y)
                {
                    this.position.X = maxPosX.Y;
                    Log.Warning("Collided with object. Stopping object");
                }
                else this.position.X += force.X;
            }

            if (force.Y > 0)
            {
                if (this.position.Y + this.scale.Y + force.Y > maxPosY.X)
                {
                    this.position.Y = maxPosY.X - this.scale.Y;
                    Log.Warning("Collided with object. Stopping object");
                }
                else this.position.Y += force.Y;
            }
            else if (force.Y < 0)
            {
                if (this.position.Y + force.Y < maxPosY.Y)
                {
                    this.position.Y = maxPosY.Y;
                    Log.Warning("Collided with object. Stopping object");
                }
                else this.position.Y += force.Y;
            }

            foreach (Shape shape in appendedShapes)
            {             
                shape.ShapeMove(new Vector2(position.X - oldpos.X, position.Y - oldpos.Y));
            }

            if (cameraIsAppended) 
            {
                Camera.MoveCamera(new Vector2(position.X - oldpos.X, position.Y - oldpos.Y));
            }
        }

        public void AppendShape(Shape shape) 
        {
            appendedShapes.Add(shape);
            shape.isAppended = true;
        }

        public Shape IsColiding(string tag) 
        {
            foreach (Shape b in Engine.Shapes) 
            {
                if (b.tag == tag) 
                {
                    if (position.X < b.position.X + b.scale.X &&
                        position.X + scale.X > b.position.X &&
                        position.Y < b.position.Y + b.scale.Y &&
                        position.Y + scale.Y > b.position.Y) 
                    {
                        return b;
                    }
                }
            }
            return null;
        }
    }
}
