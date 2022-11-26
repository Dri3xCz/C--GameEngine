using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.GameEngine
{
    static public class Camera
    {
        static public Vector2 position = Vector2.Zero();
        static public void MoveCamera(Vector2 pos) 
        {
            foreach (Shape shape in Engine.Shapes)
            {
                shape.position.X -= pos.X;
                shape.position.Y -= pos.Y;
            }
        }

        static public void AppendCamera(Shape shape) 
        {
            shape.cameraIsAppended = true;
        }

    }
}
