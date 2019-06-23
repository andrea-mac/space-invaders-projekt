using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

namespace AndreaOtterProjekt
{
    /// <summary>
    /// Igrač class
    /// </summary>
    class Igrac : Entity
    {
        float BrzinaKretanja = 5.0f;
        public Metak metak;
        public int IznosBodova = 0;
        public int ZivotiIgraca = 3;
        public bool JeZiv = true;
        Image SlikaIgraca = new Image("Slike/player.png");
        Image SlikaUnistenogIgraca = new Image("Slike/playerDead.png");
        AutoTimer Ozivljavanje = new AutoTimer(50.0f);

        public Igrac()
        {
            GlavnaScena glavnaScena = Program.Igra.GetScene<GlavnaScena>();

            // Set position
            SetPosition(new Vector2(glavnaScena.PlayPosition.X + glavnaScena.PlayWidth.X,
                              glavnaScena.PlayPosition.Y + glavnaScena.PlayWidth.Y));

            // Set image 
            AddGraphic(SlikaIgraca);

            // Add collider
            BoxCollider Collider = new BoxCollider(30, 30, Oznake.Igrac);
            AddCollider(Collider);

            // Initialize bullet
            Image metakIgraca = new Image("Slike/playerBullet.png");
            BoxCollider bulletCollider = new BoxCollider(metakIgraca.Width, metakIgraca.Height, Oznake.Igrac);
            metak = new Metak(-6.0f, new Vector2(0, 0), bulletCollider);
            metak.Visible = false;
            metak.Collidable = false;
            metak.AddGraphic(metakIgraca);
            glavnaScena.Add(metak);
        }

        /// <summary>
        /// poziva svaki frame
        /// </summary>
        public override void Update()
        {
            base.Update();
            Ozivljavanje.Update();

            if (!JeZiv && Ozivljavanje.AtMax)
            {
                GlavnaScena glavnaScena = Program.Igra.GetScene<GlavnaScena>();
                JeZiv = true;
                RemoveGraphic(SlikaUnistenogIgraca);
                AddGraphic(SlikaIgraca);
                glavnaScena.Igrac.SetPosition(new Vector2(glavnaScena.PlayPosition.X + glavnaScena.PlayWidth.X,
                        glavnaScena.PlayPosition.Y + glavnaScena.PlayWidth.Y));
            }

            AzurirajKretanje();

            //If playerLives are 0, then switch to highscore screen
            
            if (ZivotiIgraca == 0)
                Game.SwitchScene(new HighScoresScene(IznosBodova));
                
        }

        /// <summary>
        /// Azuriraj pokrete igrača
        /// </summary>
        void AzurirajKretanje()
        {
            GlavnaScena glavnaScena = Program.Igra.GetScene<GlavnaScena>();

            if (JeZiv)
            {
                // provjeri jel se igrač kreće lijevo
                if (Input.KeyDown(Key.A) || Input.KeyDown(Key.Left))
                    X -= BrzinaKretanja;

                // provjeri jel se igrač kreće desno
                if (Input.KeyDown(Key.D) || Input.KeyDown(Key.Right))
                    X += BrzinaKretanja;

                // provjeri jel igrač puca
                if (Input.KeyDown(Key.Space) && !metak.Visible)
                    Pucaj();
            }

            // Check if player is in play area
            if (X < glavnaScena.PlayPosition.X)
                X = glavnaScena.PlayPosition.X;
            else if (X > glavnaScena.PlayPosition.X + glavnaScena.PlayWidth.X)
                X = glavnaScena.PlayPosition.X + glavnaScena.PlayWidth.X;
        }

        // Igrač puca
        void Pucaj()
        {
            metak.Collidable = true;
            metak.Visible = true;
            metak.Position = Position;
        }

        // Igrač umire
        public void Smrt()
        {
            JeZiv = false;
            Ozivljavanje.Reset();
            Ozivljavanje.Start();
            RemoveGraphic(SlikaIgraca);
            AddGraphic(SlikaUnistenogIgraca);
        }
    }
}
