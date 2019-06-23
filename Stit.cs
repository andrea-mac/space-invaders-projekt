using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Otter;

namespace AndreaOtterProjekt
{

    public class Tocka
    {
        public int X { get; set; }
        public int Y { get; set; }

        public static int Z;

        public Tocka(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    class Stit : Entity
    {
        static List<Image> Slike = new List<Image>();
        int TrenutnaSlika = 0;

        public Stit()
        {
            Collider collider = new BoxCollider(Slike[0].Width, Slike[0].Height, Oznake.Stit);
            AddGraphic(Slike[0]);
            AddCollider(collider);
        }

        /// <summary>
        /// Smanjuje zdravlje od štita
        /// </summary>
        public void OstetiSe()
        {
            TrenutnaSlika++;
            if (TrenutnaSlika > 3)
            {
                RemoveSelf();
                return;
            }

            // Promijeni sliku
            RemoveGraphic(Slike[TrenutnaSlika - 1]);
            AddGraphic(Slike[TrenutnaSlika]);
        }

        /// <summary>
        /// Postavljanje slika i štitova
        /// </summary>
        public static void Postavljanje()
        {
            PostaviSlike();
            PostaviStitove();
        }


        /// <summary>
        /// Postavi slike
        /// </summary>
        static void PostaviSlike()
        {
            Image stit100 = new Image("Slike/wall100.png");
            Image stit75 = new Image("Slike/wall75.png");
            Image stit50 = new Image("Slike/wall50.png");
            Image stit25 = new Image("Slike/wall25.png");

            Slike.Add(stit100);
            Slike.Add(stit75);
            Slike.Add(stit50);
            Slike.Add(stit25);
        }

        /// <summary>
        /// Postavlja stitove
        /// </summary>
        static void PostaviStitove()
        {

            List<Tocka> tocke = new List<Tocka>()
            {
                new Tocka(50, 450),
                new Tocka(50, 474),
                new Tocka(50, 498),
                new Tocka(74, 450),
                new Tocka(98, 450),
                new Tocka(122, 450),
                new Tocka(122, 474),
                new Tocka(122, 498)
            };

            foreach (Tocka t in tocke)
            {
                for (int i = 0; i < 4; i++)
                {
                    Stit stit = new Stit();
                    stit.Position = new Vector2(t.X + i * 200, t.Y);
                    Program.Igra.GetScene<GlavnaScena>().Add(stit);
                }
            }

        }
    }
}
