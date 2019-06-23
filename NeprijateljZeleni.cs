using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

namespace AndreaOtterProjekt
{
    /// <summary>
    /// Neprijatelj vrste Hobotnica
    /// </summary>
    class Hobotnica : Neprijatelj
    {
        static Image IkonaNeprijatelja = new Image("Slike/enemy3.png");

        // Dodaje hobotnicu na scenu
        public Hobotnica()
        {
            AddGraphic(IkonaNeprijatelja);
            Bodovi = 10;
        }
    }
}
