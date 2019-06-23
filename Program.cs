using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Otter;

namespace AndreaOtterProjekt
{

    class Program
    {
        public static Game Igra;
        public static string TrenutniBodoviTekst;

        //za sakriti konzolu
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool FreeConsole();

        static void Main(string[] args)
        {
            // Creates game window
            Igra = new Game("Andrea Space Invader", 800, 600, 60, false);
            Igra.Color = Color.Blue;
            FreeConsole();

            // Kreiranje scene igre
            GlavnaScena glavnaScena = new GlavnaScena();
            Igra.AddScene(glavnaScena);
            glavnaScena.Initialize();

            Igra.WindowResize = false;
            Igra.SetIcon("Slike/windowPicture.png");
            Igra.Start();
        }
    }
}
