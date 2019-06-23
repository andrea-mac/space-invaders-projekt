using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Otter;

namespace AndreaOtterProjekt
{
    /// <summary>
    /// Klasa koja definira neprijatelja
    /// </summary>
    class Neprijatelj : Entity
    {
        static float VelicinaNeprijatelja = 32.0f;
        static float BrzinaKretanja = 0.8f;
        static Vector2 SmjerKretanja = new Vector2(BrzinaKretanja, 0.0f);
        static Vector2 SljedeciSmjerKretanja;
        static IDictionary<string, Func<Neprijatelj>> SviNeprijatelji = new Dictionary<string, Func<Neprijatelj>>();
        static AutoTimer IntervalPucanja = new AutoTimer(new Random().Next(700, 1500));
        static float VisinaPomaka = 24.0f;
        public static bool SePomaknuo = false;
        static Image NeprijateljskiMetak = new Image("Slike/enemyBullet.png");
        public int Bodovi;

        /// <summary>
        /// Inicijalizira neprijatelja
        /// </summary>
        public Neprijatelj()
        {
            BoxCollider collider = new BoxCollider(24, 24, Oznake.Neprijatelj);
            AddCollider(collider);
            IntervalPucanja.Start();
        }

        /// <summary>
        /// Loads enemies.
        /// </summary>
        public static void Postavljanje()
        {
            PostaviSveNeprijatelje();
            UcitajNeprijatelje("level1");
        }

        /// <summary>
        /// Adds enemies to list so they can be loaded.
        /// </summary>
        public static void PostaviSveNeprijatelje()
        {
            SviNeprijatelji.Add("lignja", () => { return new NeprijateljCrveni(); });
            SviNeprijatelji.Add("rak", () => { return new NeprijateljZuti(); });
            SviNeprijatelji.Add("hobotnica", () => { return new Hobotnica(); });
        }

        /// <summary>
        /// Ažuriraj kretanje
        /// </summary>
        void AzurirajKretanje()
        {
            GlavnaScena glavnaScena = Program.Igra.GetScene<GlavnaScena>();
            List<Neprijatelj> neprijatelji = Scene.GetEntities<Neprijatelj>();

            SetPosition(Position + SmjerKretanja);

            // Provjeri je li se neprijatelj pomakao 24p po Y osi, ako jeste pomakni ga u X os
            if (VisinaPomaka <= 0)
            {
                SmjerKretanja = SljedeciSmjerKretanja;
                VisinaPomaka = 24;
                SePomaknuo = true;
            }

            // Provjeri je li neprijatelj na krajnjoj desnoj strani ekrana, ako jeste, spusti ga dole
            if (Position.X > glavnaScena.GetPlayArea().X)
            {
                SetPosition(Position - SmjerKretanja);
                SmjerKretanja = new Vector2(0.0f, BrzinaKretanja);
                SljedeciSmjerKretanja = new Vector2(-1.0f * BrzinaKretanja, 0.0f);
            }

            // Provjeri je li neprijatelj na krajnjoj lijevoj strani ekrana, ako jeste, spusti ga dole
            if (Position.X < glavnaScena.PlayPosition.X)
            {
                SetPosition(Position - SmjerKretanja);
                SmjerKretanja = new Vector2(0.0f, BrzinaKretanja);
                SljedeciSmjerKretanja = new Vector2(1.0f * BrzinaKretanja, 0.0f);
            }

            // Provjeri je li neprijatelj na najnižoj točki, ako jeste, završi igru
            
            if (Position.Y >= glavnaScena.GetPlayArea().Y - 100)
                Game.SwitchScene(new HighScoresScene(glavnaScena.Igrac.IznosBodova));
            
            // Pomiče neprijatelja
            VisinaPomaka -= SmjerKretanja.Y / neprijatelji.Count;
        }

        /// <summary>
        /// Ažuriraj pucanje
        /// </summary>
        void AzurirajPucanje()
        {
            // Check if ShootingCooldown is at max
            if (IntervalPucanja.AtMax)
            {
                IntervalPucanja.Stop();
                Random rnd = new Random();
                // Chooses the enemy that shoots
                List<Neprijatelj> neprijatelji = Scene.GetEntities<Neprijatelj>();
                int RedniBrojNeprijatelja = rnd.Next(1, neprijatelji.Count);

                // Create bullet
                BoxCollider collider = new BoxCollider(NeprijateljskiMetak.Width, NeprijateljskiMetak.Height, Oznake.Neprijatelj);
                Metak metak = new Metak(6.0f, neprijatelji[RedniBrojNeprijatelja - 1].Position, collider);
                metak.AddGraphic(NeprijateljskiMetak);
                Scene.Add(metak);

                // Reset ShootingCooldown
                IntervalPucanja.Max = rnd.Next(2000, 5000);
                IntervalPucanja.Start();
            }
        }

        /// <summary>
        /// Ažuriranje
        /// </summary>
        public override void Update()
        {
            base.Update();
            AzurirajPucanje();
            AzurirajKretanje();
            IntervalPucanja.Update();
        }

        public static List<string> DohvatiNeprijatelje(string level)
        {
            List<string> neprijateljiLevel = new List<string>();
            switch (level)
            {
                case "level1":
                case "level2":
                case "level3":
                case "level4":
                case "level5":
                    {
                    for (int i = 0; i < 13; i++)
                    {
                        neprijateljiLevel.Add("lignja");
                    }
                    for (int i = 0; i < 26; i++)
                    {
                        neprijateljiLevel.Add("rak");
                    }
                    for (int i = 0; i < 26; i++)
                    {
                        neprijateljiLevel.Add("hobotnica");
                    }
                    break;
                }
            }

            return neprijateljiLevel;
        }

        /// <summary>
        /// Učitava sve neprijatelje po levelu
        /// </summary>
        /// <param level="level">level koji treba učitati</param>
        public static void UcitajNeprijatelje(string level)
        {
            GlavnaScena glavnaScena = Program.Igra.GetScene<GlavnaScena>();

            List<string> neprijateljiLevel = DohvatiNeprijatelje(level);

            // Current enemy position to load
            Vector2 trenutnaPozicija = new Vector2(glavnaScena.PlayPosition.X, glavnaScena.PlayPosition.Y);

            foreach (var n in neprijateljiLevel)
            {
                Neprijatelj neprijatelj = SviNeprijatelji[n]();
                neprijatelj.Position = trenutnaPozicija;
                glavnaScena.Add(neprijatelj);

                // Set enemy position
                trenutnaPozicija.X += VelicinaNeprijatelja;
                if (trenutnaPozicija.X > 420)
                {
                    trenutnaPozicija.X = glavnaScena.PlayPosition.X;
                    trenutnaPozicija.Y += VelicinaNeprijatelja;
                }
            }

        }
    }
}
