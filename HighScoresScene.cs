using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

namespace AndreaOtterProjekt
{
    /// <summary>
    /// Highscore Scene, happens after the game ends
    /// </summary>
    class HighScoresScene : Scene
    {

        // Stuff in the HighScores Scene
        public HighScoresScene(int bodovi) : base()
        {
            OnBegin = delegate
            {

                var txtConfig = new RichTextConfig()
                {
                    TextAlign = TextAlign.Center,
                    CharColor = Color.Green,
                    FontSize = 25,
                    SineAmpX = 1,
                    SineAmpY = 2,
                    SineRateX = 1,
                    Font = new Font("Slike/VCR_OSD_MONO.ttf"),
                };

                var curScoreTxtLabel = new RichText("Cestitamo, osvojili ste " + bodovi + " bodova!", txtConfig);
                curScoreTxtLabel.SetPosition(150, 255);
                AddGraphic(curScoreTxtLabel);

                //Adds Textbox and button and highscore leaderboard (hslb)
                Program.Igra.MouseVisible = true;


            };
            Program.Igra.AddScene(this);
        }
    }
}
