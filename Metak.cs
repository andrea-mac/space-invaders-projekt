using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

namespace AndreaOtterProjekt
{
    enum Oznake
    {
        Igrac,
        Neprijatelj,
        Stit,
        Ufo
    }

    /// <summary>
    /// Klasa koja upravlja mecima
    /// </summary>
    class Metak : Entity
    {
        static int BrojZivota = 3;
        public float BrzinaKretanja;

        /// <summary>
        /// Kreira novi metak
        /// </summary>
        /// <param name="brzinaKretanja">Brzina metka</param>
        /// <param name="pozicija">Pozicija metka</param>
        /// <param name="collider">collider</param>
        public Metak(float brzinaKretanja, Vector2 pozicija, BoxCollider collider)
        {
            Position = pozicija;
            BrzinaKretanja = brzinaKretanja;
            AddCollider(collider);
        }

        /// <summary>
        /// Ažurira metak
        /// </summary>
        public override void Update()
        {
            base.Update();

            if(Visible)
            {
                Y += BrzinaKretanja;
                ProvjeriGraniceMetka();
                ProvjeriSudaranjeMetka();

                // provjeri je li igrač ima 0 života, ako ima 0, završi igru
                GlavnaScena glavnaScena = Program.Igra.GetScene<GlavnaScena>();
                if (BrojZivota == 0)
                    Game.SwitchScene(new HighScoresScene(glavnaScena.Igrac.IznosBodova));
                    
            }
        }

        /// <summary>
        /// Provjeri je li metak u granicama igre, ako nije, ukloni ga sa scene
        /// </summary>
        void ProvjeriGraniceMetka()
        {
            // Provjeri je li igračev metak izvan granica i ukloni ga ako jeste
            if (Collider.Tags[0] == (int)Oznake.Igrac)
                if (Position.Y < 0)
                {
                    Visible = false;
                    Collidable = false;
                }

            // Provjeri je li neprijateljev metak izvan granica i ukloni ga ako jeste
            if (Collider.Tags[0] == (int)Oznake.Neprijatelj)
                if (Position.Y > Game.Height)
                    RemoveSelf();
        }

        /// <summary>
        /// Check if bullet collides with entities.
        /// </summary>
        void ProvjeriSudaranjeMetka()
        {
            GlavnaScena glavnaScena = Program.Igra.GetScene<GlavnaScena>();
            ProvjeriMetakIgraca(glavnaScena);
            ProvjeriNeprijateljskiMetak(glavnaScena);
            CheckBarricade(glavnaScena);
            CheckUfo(glavnaScena);
        }

        /// <summary>
        /// Check if player bullet collides with entities.
        /// </summary>
        /// <param name="glavnaScena">scene</param>
        void ProvjeriMetakIgraca(GlavnaScena glavnaScena)
        {
            // Check if bullet is player bullet
            if (Collider.Tags[0] == (int)Oznake.Igrac)
                // Provjeri je li se metak sudario s neprijateljima
                if (Collider.CollideEntity(X, Y, Oznake.Neprijatelj) != null)
                    // provjeri je li metak pogodio metak
                    if ((Collider.CollideEntities(X, Y, Oznake.Neprijatelj)[0].GetType() == typeof(Metak)))
                    {
                        Collider.CollideEntities(X, Y, Oznake.Neprijatelj)[0].RemoveSelf();
                        Visible = false;
                        Collidable = false;
                    }
                    // Check if bullet hits enemy
                    else
                    {
                        Neprijatelj neprijatelj = (Neprijatelj)Collider.CollideEntities(X, Y, Oznake.Neprijatelj)[0];
                        neprijatelj.RemoveSelf();
                        Visible = false;
                        Collidable = false;

                        if (glavnaScena.GetEntities<Neprijatelj>().Count <= 1)
                            glavnaScena.NextLevel();

                        glavnaScena.Igrac.IznosBodova += neprijatelj.Bodovi;
                        glavnaScena.TrenutniBodoviText.String = glavnaScena.Igrac.IznosBodova.ToString();
                        Program.TrenutniBodoviTekst = glavnaScena.Igrac.IznosBodova.ToString();
                        glavnaScena.TrenutniBodoviText.Refresh();
                    }
        }


        /// <summary>
        /// provjera je li neprijateljski metak se sudario
        /// </summary>
        /// <param name="scene">scene</param>
        void ProvjeriNeprijateljskiMetak(GlavnaScena scene)
        {
            // Provjera je li metak neprijateljski
            if (Collider.Tags[0] == (int)Oznake.Neprijatelj)
            {
                // Provjeri je li se sudara s objektima od igrača
                if (Collider.CollideEntity(X, Y, Oznake.Igrac) != null)
                {
                    // je li se sudario s igračem
                    if (!(Collider.CollideEntities(X, Y, Oznake.Igrac)[0].GetType() == typeof(Metak)))
                        if (scene.Igrac.JeZiv)
                        {
                            RemoveSelf();
                            scene.Igrac.ZivotiIgraca -= 1;
                            scene.Igrac.Smrt();
                            scene.PreostaloZivotaText.String = scene.Igrac.ZivotiIgraca.ToString();
                            scene.PreostaloZivotaText.Refresh();
                        }

                        // Check if enemy bullet collides with player bullet
                        else
                            RemoveSelf();
                }
            }


        }

        /// <summary>
        /// provjeri sudaranje sa štitom
        /// </summary>
        /// <param name="glavnaScena">scene</param>
        void CheckBarricade(GlavnaScena glavnaScena)
        {
            Stit stit = (Stit)Collider.CollideEntity(X, Y, Oznake.Stit);
            if (stit != null)
            {
                // provjera je li igračev metak pogodio štit
                if (Collider.Tags[0] == (int)Oznake.Igrac)
                {
                    stit.OstetiSe();
                    Visible = false;
                    Collidable = false;
                }

                // provjera je li neprijateljev metak pogodio štit
                else
                {
                    stit.OstetiSe();
                    RemoveSelf();
                }
            }
        }

        /// <summary>
        /// Provjera je li metak pogodio UFO neparijetelja
        /// </summary>
        /// <param name="glavnaScena">scene</param>
        void CheckUfo(GlavnaScena glavnaScena)
        {
            UFO ufo = (UFO)Collider.CollideEntity(X, Y, Oznake.Ufo);

            // ako se sudario s UFO, ukloni ga
            if (ufo != null)
            {
                ufo.Smrt();
                glavnaScena.Igrac.IznosBodova += ufo.Bodovi;
                glavnaScena.Igrac.metak.Visible = false;
                glavnaScena.Igrac.metak.Collidable = false;
            }
        }

    }
}
