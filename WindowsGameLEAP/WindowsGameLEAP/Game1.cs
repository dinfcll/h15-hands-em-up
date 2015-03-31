using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using LeapLibrary;

namespace WindowsGameLEAP
{
    
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D backGroundTexture;    // will point to a picture to use for background
        Texture2D missileTexture;       // will point to a picture to use for the missile
        Texture2D BigMissileTexture;
        Texture2D alien;
        Texture2D Boss;
        Texture2D gameover;
        Texture2D WinTexture;
        Texture2D shipTex;
        Rectangle viewPort;             // tells us the size of the drawable area in the window
        Ship myShip;                    // reference to the ship controlled by the user
        LeapComponet leap;
        Missile.GunType gun = Missile.GunType.NORMAL;
        float timer = 0.2f;         
        float TIMER = 0.2f;

        float timerAlien = 1f;
         float TIMERAlien = 1f;

         int alienspeed = 1;
        float timerSwitchGun = 0.5f;
        const float TIMERGUN = 0.5f;

        List<Missile> Missiles;
        List<Alien> Aliens;
        SpriteFont font;
        bool GameOver = true;
        int Life = 3;
        int score = 0;
        bool Win = false;
        bool Updated = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            leap = new LeapComponet(this);
            this.Components.Add(leap);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            viewPort = GraphicsDevice.Viewport.Bounds;
            myShip = new Ship(new Vector2((viewPort.Width) / 2,
                                          viewPort.Bottom - 39));
            Missiles = new List<Missile>();
            Aliens = new List<Alien>();
            base.Initialize();
            
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("SpriteFont1");
            backGroundTexture = Content.Load<Texture2D>("stars");
            shipTex = Content.Load<Texture2D>("ship-small");
            Boss = Content.Load<Texture2D>("boss");
            gameover = Content.Load<Texture2D>("gameover");
            alien = Content.Load<Texture2D>("spaceinvader");
            missileTexture = Content.Load<Texture2D>("missile");
            BigMissileTexture = Content.Load<Texture2D>("BigMissile");
            WinTexture = Content.Load<Texture2D>("You-Win");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// This gets called ~60 times per second
        protected override void Update(GameTime gameTime)
        {
            if (!GameOver && !Win)
            {

                if (score > 0 && score % 20 != 0)
                {
                    Updated = false;
                }
                switch (score)
                {
                    case 20: if (!Updated) { alienspeed++; Updated = true; }
                        break;
                    case 40: if (!Updated) { alienspeed++; Updated = true; }
                        break;
                    case 60: if (!Updated) { alienspeed++; Updated = true; }
                        break;
                    case 80: if (!Updated) { alienspeed++; Updated = true; }
                        break;
                    case 100: if (!Updated) { alienspeed++; Updated = true; }
                        break;
                    case 120: if (!Updated) { alienspeed++; Updated = true; }
                        break;
                    case 140: if (!Updated) { alienspeed++; Updated = true; }
                        break;
                    case 160: if (!Updated) { alienspeed++; Updated = true; }
                        break;
                    case 180: if (!Updated) { alienspeed++; Updated = true; }
                        break;
                    case 200: if (!Updated) { Win = true; }
                        break;
                }
                float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
                timer -= elapsed;
                timerSwitchGun -= elapsed;
                timerAlien -= elapsed;

                if (timerSwitchGun < 0)
                {
                    SelectGun();
                    timerSwitchGun = TIMERGUN;
                }
                if (timer < 0)
                {
                    //Timer expired, execute action
                   
                    fireMissle();   // create a missle if the space bar was hit
                    if (gun == Missile.GunType.LARGE)
                        TIMER = 0.6f;
                    else
                        TIMER = 0.2f;
                    timer = TIMER;   //Reset Timer
                }


                moveShip();     // if a arrow key is held down, move the ship
                moveMissle();  // if the missle is alive, update its position
                
                if (timerAlien < 0)
                {
                    //Spawn Alien
                    Aliens.Add(new Alien(alien.Height, viewPort, alien.Width));
                    timerAlien = TIMERAlien;
                }
                foreach (Alien alie in Aliens)
                {
                    alie.Update(alienspeed);
                    if (alie.EndCourse())
                    {
                        Life--;
                    }
                }



                for (int i = 0; i < Missiles.Count; i++)
                {
                    if (Missiles[i].guntype == Missile.GunType.NORMAL)
                    {
                        Rectangle miss = new Rectangle((int)Missiles[i].pos.X, (int)Missiles[i].pos.Y, missileTexture.Width, missileTexture.Height);
                        for (int y = 0; y < Aliens.Count; y++)
                        {

                            Rectangle ali = new Rectangle((int)Aliens[y].pos.X, (int)Aliens[y].pos.Y, alien.Width, alien.Height);
                            if (Missiles[i].Dead == false)
                                if (miss.Intersects(ali))
                                {
                                    Aliens[y].Dead = true;
                                    Missiles[i].Dead = true;
                                    score++;
                                }
                        }
                    }
                    else
                    {
                        Rectangle miss = new Rectangle((int)Missiles[i].pos.X, (int)Missiles[i].pos.Y, BigMissileTexture.Width, BigMissileTexture.Height);
                        for (int y = 0; y < Aliens.Count; y++)
                        {

                            Rectangle ali = new Rectangle((int)Aliens[y].pos.X, (int)Aliens[y].pos.Y, alien.Width, alien.Height);
                            if (Missiles[i].Dead == false)
                                if (miss.Intersects(ali))
                                {
                                    Aliens[y].Dead = true;
                                    Missiles[i].Dead = true;
                                    score++;
                                }
                        }
                    }
                    
                }
                List<Alien> al = new List<Alien>();
                foreach (Alien a in Aliens)
                {
                    if (!a.Dead)
                        al.Add(a);
                }
                Aliens = al;
                List<Missile> missi = new List<Missile>();
                foreach (Missile m in Missiles)
                {
                    if (!m.Dead)
                    {
                        missi.Add(m);
                    }
                }
                Missiles = missi;
                if (Life <= 0)
                    GameOver = true;
            }
            else
            {
                var frame = leap.controller.Frame();
                if (frame.Hands[0].PalmPosition.y >= 300)
                {
                    Aliens = new List<Alien>();
                    Missiles = new List<Missile>();
                    Life = 3;
                    GameOver = false;
                    gun = Missile.GunType.NORMAL;
                    score = 0;
                    alienspeed = 1;
                    Win = false;
                }
            }
            base.Update(gameTime);
        }


        private void SelectGun()
        {
            if (leap.fingerPoints.Count > 0)
            {
                Vector2 distance = leap.fingerPoints[0] - leap.fingerPoints[leap.fingerPoints.Count - 1];
                if (distance.X < 0) distance = distance * -1;
                if (distance.X > 300)
                {
                    switch (gun)
                    {
                        case Missile.GunType.NORMAL: gun = Missile.GunType.LARGE;
                            break;
                        case Missile.GunType.LARGE: gun = Missile.GunType.NORMAL;
                            break;
                    }
                }
            }

        }
        
        private void moveShip()
        {

            var frame = leap.controller.Frame();
            myShip._pos.X = (int)((frame.Hands[0].PalmPosition.x *4 + viewPort.Width/2));


            if (myShip._pos.X <= 0)
            {
                myShip._pos.X = 0;
            }

            if (myShip._pos.X >= viewPort.Width - shipTex.Width)
            {
                myShip._pos.X = viewPort.Width - shipTex.Width;
            }
        }


        private void fireMissle()
        {
            KeyboardState newState = Keyboard.GetState();

            Vector2 GunPosition = new Vector2(myShip._pos.X + (shipTex.Width / 2), myShip._pos.Y);
            if (gun == Missile.GunType.NORMAL)
            {
                Missiles.Add(new Missile(new Vector2(GunPosition.X + shipTex.Width / 2, GunPosition.Y), gun));
                Missiles.Add(new Missile(new Vector2(GunPosition.X - shipTex.Width / 2, GunPosition.Y), gun));
            }
            else
            {
                Missiles.Add(new Missile(new Vector2(GunPosition.X + shipTex.Width / 2 + 10, GunPosition.Y), gun));
                Missiles.Add(new Missile(new Vector2(GunPosition.X - shipTex.Width / 2 - 10, GunPosition.Y), gun));
            }
            
        }

        // If the missle is alive, move it up a few clicks until it goes off screen
        public void moveMissle()
        {
            List<Missile> miss = new List<Missile>();
            foreach (Missile m in Missiles)
            {
                m.Update();
                if (!m.Dead)
                {
                    miss.Add(m);
                }
            }
            Missiles = miss;
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            
            
            // draw the ship over background
            if (!GameOver)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(backGroundTexture, viewPort, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1);
                myShip.Draw(spriteBatch, shipTex);

                //draw alien
                foreach (Alien a in Aliens)
                    spriteBatch.Draw(alien, a.pos, Color.White);

                // draw the missle
                foreach (Missile m in Missiles)
                {
                    if (m.guntype == Missile.GunType.NORMAL)
                        spriteBatch.Draw(missileTexture, m.pos, Color.White);
                    else
                        spriteBatch.Draw(BigMissileTexture, m.pos, Color.White);

                }


                //Text for LeapController
                var frame = leap.controller.Frame();
                spriteBatch.DrawString(font, "Niveau: " + alienspeed.ToString(), new Vector2(10, 35), Color.Red);
                spriteBatch.DrawString(font, "Vie: " +Life.ToString(), new Vector2(10, 50), Color.Orange);
                spriteBatch.DrawString(font, "Score: " + score.ToString(), new Vector2(10, 65), Color.Orange);

                spriteBatch.End();
            }
            else
            {
                if (Win)
                {
                    spriteBatch.Begin();
                    spriteBatch.Draw(backGroundTexture, viewPort, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1);
                    spriteBatch.Draw(WinTexture, new Rectangle(viewPort.Width / 2 - gameover.Width / 2, viewPort.Height / 2 - gameover.Height / 2, gameover.Width, gameover.Height), Color.White);
                    spriteBatch.End();
                }
                else
                {
                    spriteBatch.Begin();
                    spriteBatch.Draw(backGroundTexture, viewPort, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1);
                    spriteBatch.Draw(gameover, new Rectangle(viewPort.Width / 2 - gameover.Width / 2, viewPort.Height / 2 - gameover.Height / 2, gameover.Width, gameover.Height), Color.White);
                    spriteBatch.End();
                }
                
            }

            base.Draw(gameTime);
        }
    }
}
