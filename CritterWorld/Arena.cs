﻿using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SCG.TurboSprite;

namespace CritterWorld
{
    public partial class Arena : UserControl
    {
        public event EventHandler<SpriteEventArgs> CritterEscaped;

        private Random rnd = new Random(Guid.NewGuid().GetHashCode());

        private int critterStartX;
        private int critterStartY;

        private const int launchMarginX = 50;
        private const int launchMarginY = 50;

        private System.Timers.Timer fpsTimer = null;

        private string fpsPrompt = "FPS";
        private int activeFPSPromptState = 0;

        private string ActiveFPSPrompt()
        {
            activeFPSPromptState = (activeFPSPromptState + 1) % fpsPrompt.Length;
            return 
                fpsPrompt.Substring(0, activeFPSPromptState) + 
                fpsPrompt[activeFPSPromptState].ToString().ToLower() + 
                fpsPrompt.Substring(activeFPSPromptState + 1);
        }

        public bool WillCollide(Sprite sprite)
        {
            return spriteEngineMain.WillCollide(sprite);
        }

        public void AddSprite(Sprite sprite)
        {
            spriteEngineMain.AddSprite(sprite);
        }

        public SpriteSurface Surface
        {
            get
            {
                return spriteSurfaceMain;
            }
        }

        public int ActualFPS
        {
            get
            {
                return spriteSurfaceMain.ActualFPS;
            }
        }

        public void AddTerrain(int arenaX1, int arenaX2, int arenaY1, int arenaY2)
        {
            Terrain terrainSprite = new Terrain(arenaX1, arenaX2, arenaY1, arenaY2);
            AddSprite(terrainSprite);
        }

        private delegate Sprite CreateSprite(Point location);
        private delegate void InitialiseSprite(Sprite sprite);

        private void AddItem(int count, CreateSprite factory, InitialiseSprite initialiser = null)
        {
            for (int i = 0; i < count; i++)
            {
                Sprite sprite;
                do
                {
                    int x = rnd.Next(launchMarginX, Surface.Width - launchMarginX);
                    int y = rnd.Next(launchMarginY, Surface.Height - launchMarginY);
                    sprite = factory(new Point(x, y));
                }
                while (WillCollide(sprite));
                AddSprite(sprite);
                initialiser?.Invoke(sprite);
            }
        }

        public void AddGifts(int count)
        {
            AddItem(count, location => new Gift(location));
        }

        public void AddBombs(int count)
        {
            AddItem(count, location => new Bomb(location), sprite => ((Bomb)sprite).LightFuse());
        }

        public void AddFoods(int count)
        {
            AddItem(count, location => new Food(location));
        }

        public void AddCritter(Critter critter)
        {
            do
            {
                critterStartY += 30;
                if (critterStartY >= spriteSurfaceMain.Height - 30)
                {
                    critterStartY = 30;
                    critterStartX += 100;
                }
                critter.Position = new Point(critterStartX, critterStartY);
            }
            while (WillCollide(critter));
            AddSprite(critter);
        }

        public void AddEscapeHatch(Point position)
        {
            EscapeHatch escapeHatch = new EscapeHatch(position);
            AddSprite(escapeHatch);
        }

        public void ResetLaunchPosition()
        {
            critterStartX = 30;
            critterStartY = 0;
        }

        public void Launch()
        {
            if (spriteSurfaceMain.Active)
            {
                return;
            }

            spriteSurfaceMain.Active = true;

            TextSprite fps = new TextSprite("999 FPS", "Courier New", 10, FontStyle.Regular)
            {
                HorizontalAlignment = StringAlignment.Far,
                VerticalAlignment = StringAlignment.Far,
                Position = new Point(spriteSurfaceMain.Width - 5, spriteSurfaceMain.Height - 5),
                Color = Color.Gray,
                Alpha = 128
            };
            AddSprite(fps);

            fpsTimer = new System.Timers.Timer
            {
                Interval = 250
            };
            fpsTimer.Elapsed += (sender, e) => fps.Text = ActualFPS + " " + ActiveFPSPrompt();
            fpsTimer.Start();

            System.Timers.Timer critterStartupTimer = new System.Timers.Timer();
            critterStartupTimer.Interval = 2000;
            critterStartupTimer.AutoReset = false;
            critterStartupTimer.Elapsed += (sender, e) =>
            {
                foreach (Sprite sprite in spriteEngineMain.SpriteArray)
                {
                    if (sprite is Critter critter)
                    {
                        critter.Startup();
                    }
                }
            };
            critterStartupTimer.Start();
        }

        public void Shutdown()
        {
            if (fpsTimer != null)
            {
                fpsTimer.Stop();
                fpsTimer = null;
            }

            spriteEngineMain.Locked = true;
            spriteSurfaceMain.Active = false;

            foreach (Sprite sprite in spriteEngineMain.SpriteArray)
            {
                if (sprite is Critter critter)
                {
                    critter.HardShutdown();
                }
            }
            spriteEngineMain.Purge();
            ResetLaunchPosition();

            spriteEngineMain.Locked = false;
        }

        public int CountOfActiveCritters
        {
            get
            {
                int count = 0;
                foreach (Sprite sprite in spriteEngineMain.SpriteArray)
                {
                    if (sprite is Critter critter && !critter.Dead && !critter.Stopped)
                    {
                        count++;
                    }
                }
                return count;
            }
        }

        private void Collide(Critter critter1, Critter critter2)
        {
            if (critter1.Mover == null || critter2.Mover == null)
            {
                return;
            }

            Sound.PlayBump();

            critter1.Bounceback();
            critter2.Bounceback();

            critter1.FightWith(critter2.NameAndAuthor);
            critter2.FightWith(critter1.NameAndAuthor);

            critter1.AssignRandomDestination();
            critter2.AssignRandomDestination();

            Sprite fight = new ParticleExplosionSprite(10, Color.DarkRed, Color.Red, 1, 5, 10)
            {
                Position = new Point((critter1.Position.X + critter2.Position.X) / 2, (critter1.Position.Y + critter2.Position.Y) / 2)
            };
            AddSprite(fight);
        }

        private void Collide(Critter critter, Terrain terrain)
        {
            if (critter.Mover == null)
            {
                return;
            }

            Sound.PlayZap();

            critter.Bounceback();
            critter.AssignRandomDestination();

            critter.Bump();

            terrain.Nudge();

            Sprite bump = new ParticleExplosionSprite(10, Color.LightBlue, Color.White, 1, 2, 5)
            {
                Position = new Point((critter.Position.X + terrain.Position.X) / 2, (critter.Position.Y + terrain.Position.Y) / 2)
            };
            AddSprite(bump);
        }

        private void Collide(Critter critter, Bomb bomb)
        {
            Sound.PlayBoom();

            critter.Bombed();
            Sprite spew = new StarFieldSprite(100, 5, 5, 10)
            {
                Position = bomb.Position
            };
            AddSprite(spew);
            Sprite explosion = new ParticleFountainSprite(250, Color.DarkGray, Color.White, 1, 3, 20)
            {
                Position = bomb.Position
            };
            AddSprite(explosion);
            critter.Mover = null;
            System.Timers.Timer explosionTimer = new System.Timers.Timer
            {
                Interval = 250,
                AutoReset = false
            };
            explosionTimer.Elapsed += (sender, e) =>
            {
                explosion.Kill();
                spew.Kill();
                critter.StopAndSmoke(Color.Black, Color.Brown);
            };
            explosionTimer.Start();
            bomb.Kill();

            AddBombs(1);
        }

        private void Collide(Critter critter, Food food)
        {
            Sound.PlayGulp();
            food.Kill();
            critter.Ate();
            AddFoods(1);
        }

        private void Collide(Critter critter, Gift gift)
        {
            Sound.PlayYay();
            gift.Kill();
            critter.Scored();
            AddGifts(1);
        }

        private void Collide(Critter critter, EscapeHatch hatch)
        {
            Sound.PlayCheer();
            critter.Escaped();
            CritterEscaped?.Invoke(this, new SpriteEventArgs(critter));
        }

        private void Collide(object sender, SpriteCollisionEventArgs collision)
        {
            Sprite sprite1 = collision.Sprite1;
            Sprite sprite2 = collision.Sprite2;
            if (sprite1 is Critter critter_a1 && sprite2 is Critter critter_a2)
            {
                Collide(critter_a1, critter_a2);
            }
            else if (sprite1 is Critter critter_a3 && sprite2 is Terrain terrain)
            {
                Collide(critter_a3, terrain);
            }
            else if (sprite1 is Terrain terrain_a1 && sprite2 is Critter critter_a4)
            {
                Collide(critter_a4, terrain_a1);
            }
            else if (sprite1 is Critter critter_a5 && sprite2 is Bomb bomb_a1)
            {
                Collide(critter_a5, bomb_a1);
            }
            else if (sprite1 is Bomb bomb_a2 && sprite2 is Critter critter_a6)
            {
                Collide(critter_a6, bomb_a2);
            }
            else if (sprite1 is Critter critter_a7 && sprite2 is Food food_a1)
            {
                Collide(critter_a7, food_a1);
            }
            else if (sprite1 is Food food_a2 && sprite2 is Critter critter_a8)
            {
                Collide(critter_a8, food_a2);
            }
            else if (sprite1 is Critter critter_a9 && sprite2 is Gift gift_a1)
            {
                Collide(critter_a9, gift_a1);
            }
            else if (sprite1 is Gift gift_a2 && sprite2 is Critter critter_a10)
            {
                Collide(critter_a10, gift_a2);
            }
            else if (sprite1 is Critter critter_a11 && sprite2 is EscapeHatch escapeHatch_a1)
            {
                Collide(critter_a11, escapeHatch_a1);
            }
            else if (sprite1 is EscapeHatch escapeHatch_a2 && sprite2 is Critter critter_a12)
            {
                Collide(critter_a12, escapeHatch_a2);
            }
        }

        public Arena()
        {
            ResetLaunchPosition();

            InitializeComponent();

            spriteSurfaceMain.SpriteCollision += (sender, collisionEvent) => Collide(sender, collisionEvent);

            spriteSurfaceMain.WraparoundEdges = true;
        }
    }
}

