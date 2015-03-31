using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WindowsGameLEAP
{
    public class Alien
    {
        public Vector2 pos;
        public bool Dead;
        Rectangle View;
        int Height;

        public Alien(int HeightInvader, Rectangle ViewPort, int WidthInvader)
        {
            Random r = new Random();
            pos = new Vector2(r.Next(0,ViewPort.Width - WidthInvader), 0 - HeightInvader);
            View = ViewPort;
            Height = HeightInvader;
        }

        public void Update(int speed)
        {
            pos.Y += speed;
        }

        public bool EndCourse()
        {
            if (pos.Y >= View.Height - Height)
            {
                Dead = true;
                return true;
            }
            else
                return false;
        }
    }
}
