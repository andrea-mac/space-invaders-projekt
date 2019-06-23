using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

namespace AndreaOtterProjekt
{
    /// <summary>
    /// Neprijatelj vrste Lignja
    /// </summary>
    class NeprijateljCrveni : Neprijatelj
    {
        static Image IkonaNeprijatelja = new Image("Slike/enemy1.png");

        // Dodaje lignju na scenu
        public NeprijateljCrveni()
        {
            AddGraphic(IkonaNeprijatelja);
            Bodovi = 20;
        }

        //Postavlja poziciju
        public NeprijateljCrveni(int X, int Y)
        {
            AddGraphic(IkonaNeprijatelja);
            SetPosition(X, Y);
        }
    }
}
