using GameEngine.GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using GameEngine;

namespace GameEngine.DemoGame
{
    public class DemoGame : Engine
    {
        Shape player;
        Shape rect;
        Shape boarder;
        Shape boarder2;
        public DemoGame() : base(new Vector2(512, 512), "New Game")
        {
        }

        public override void OnDraw()
        {

        }

        public override void OnLoad()
        {   
            BackgroundColor = Color.Beige;
            player = new Shape(new Vector2(250, 250), new Vector2(20, 20), "Player", Color.Red, true);
            rect = new Shape(new Vector2(150, 150), new Vector2(100, 100), "rect", Color.Cyan, true);
            boarder = new Shape(new Vector2(500,0), new Vector2(5,1000), "boarder", Color.Black, false);
            boarder2 = new Shape(new Vector2(0, 500), new Vector2(1000, 5), "boarder", Color.Black, false);
            Camera.AppendCamera(player);
            player.AppendShape(rect);
        }

        public override void OnUpdate()
        {
            if (GetActiveKeys(Keys.A)) 
            {
                player.ShapeMove(new Vector2(-2f, 0));
            }
            if (GetActiveKeys(Keys.D))
            {
                player.ShapeMove(new Vector2(2f, 0));
            }
            if (GetActiveKeys(Keys.W))
            {
                player.ShapeMove(new Vector2(0, -2f));
            }
            if (GetActiveKeys(Keys.S))
            {
                player.ShapeMove(new Vector2(0, 2f));
            }

        }
    }
}
