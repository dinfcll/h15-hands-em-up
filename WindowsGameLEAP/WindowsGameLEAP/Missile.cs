
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WindowsGameLEAP
{
    public class Missile
    {
        public enum GunType { NORMAL, FAST, LARGE };
        public Vector2 pos;
        public bool Dead;
        public GunType guntype;

        public Missile(Vector2 startPosition,GunType gunType)
        {
            Dead = false;
            pos = startPosition;
            guntype = gunType;
        }

        public void Update()
        {
            pos.Y -= 8;
            if (pos.Y <= 0)
            {
                Dead = true;
            }
            
        }
    }
}
