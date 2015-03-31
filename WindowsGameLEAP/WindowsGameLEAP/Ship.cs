using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGameLEAP
{
    class Ship
    {
        public Vector2 _pos;
        public enum Direction { Left, Right, Up, Down };
        public Texture2D Texture { get; set; }

        public Ship(Vector2 pos)
        {
            _pos = pos;
        }

        public void Draw(SpriteBatch spriteBatch,Texture2D texture)
        {
            spriteBatch.Draw(texture, _pos, Color.White);
        }

        public void move(Direction dir)
        {
            switch (dir)
            {
                case Direction.Left: _pos.X -= 5;
                    break;
                case Direction.Right: _pos.X += 5;
                    break;
                /*case Direction.Up: _pos.Y -= 5;
                    break;
                case Direction.Down: _pos.Y += 5;
                    break;*/
            }
        }



    }
}
