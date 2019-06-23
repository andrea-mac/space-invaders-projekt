using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

namespace AndreaOtterProjekt
{
    /// <summary>
    /// Neprijatelj vrste Rak
    /// </summary>
    class NeprijateljZuti : Neprijatelj
    {
        static Image IkonaNeprijatelja = new Image("Slike/enemy2.png");

        /// <summary>
        /// Incijalizira novog protivnika Raka
        /// </summary>
        public NeprijateljZuti()
        {
            AddGraphic(IkonaNeprijatelja);
            Bodovi = 40;
        }
    }
}
