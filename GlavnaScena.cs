using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

namespace AndreaOtterProjekt
{
    /// <summary>
    /// Glavna scena igre
    /// </summary>
    class GlavnaScena : Scene
    {
        public Vector2 PlayPosition = new Vector2(20, 60);
        public Vector2 PlayWidth = new Vector2(736, 500);
        public Igrac Igrac;
        public RichText PreostaloZivotaText;
        public RichText TrenutniBodoviText;
        public int TrenutniLevel = 1;
        UFO Ufo;

        /// <summary>
        /// Inicijalizacija igre
        /// mora se dodati u prvu scenu prije pozivanja
        /// </summary>
        public void Initialize()
        {
            OnBegin = delegate
            {
                Neprijatelj.Postavljanje();
                Stit.Postavljanje();
                GlavnaScena scene = Program.Igra.GetScene<GlavnaScena>();

                Ufo = new UFO();

                Program.Igra.GetScene<GlavnaScena>().Add(Ufo);

                // Create player and add to scene
                Igrac = new Igrac();
                Add(Igrac);
                // Gametext for the entire game pretty much
                #region gameText

                var background = new Image("Slike/background.png");
                background.Alpha = 0.4f;
                scene.AddGraphic(background);

                //Setting a default config file for the RichText to use
                var txtConfig = new RichTextConfig()
                {
                    TextAlign = TextAlign.Center,
                    CharColor = Color.Green,
                    FontSize = 18,
                    SineAmpX = 1,
                    SineAmpY = 2,
                    SineRateX = 1,
                    Font = new Font("Slike/VCR_OSD_MONO.ttf"),
                };

                // Writing the text graphics and setting position
                var livesLeftTxtLabel = new RichText("Zivoti", txtConfig);
                livesLeftTxtLabel.SetPosition(50, 16);

                scene.PreostaloZivotaText = new RichText(scene.Igrac.ZivotiIgraca.ToString(), txtConfig);
                scene.PreostaloZivotaText.Name = "livesLeftTxt";
                scene.PreostaloZivotaText.SetPosition(70, 32);

                var curScoreTxtLabel = new RichText("Bodovi", txtConfig);
                curScoreTxtLabel.SetPosition(650, 15);

                scene.TrenutniBodoviText = new RichText(scene.Igrac.IznosBodova.ToString(), txtConfig);
                scene.TrenutniBodoviText.Name = "curScoreTxt";
                scene.TrenutniBodoviText.SetPosition(670, 32);

                // Adds Graphic to Scene
                scene.AddGraphic(livesLeftTxtLabel);
                scene.AddGraphic(curScoreTxtLabel);

                scene.AddGraphic(scene.PreostaloZivotaText);
                scene.AddGraphic(scene.TrenutniBodoviText);

                #endregion gameText
            };
        }

        //Update scene

        public override void Update()
        {
            base.Update();

            //Debug - Switches Scene if input is H
            
            if (Input.KeyPressed(Key.H))
            {
                Game.SwitchScene(new HighScoresScene(Igrac.IznosBodova));
            }
        }

        /// <summary>
        /// Sets next level, if CurLevel == 6 then ends the game
        /// </summary>
        public void NextLevel()
        {
            TrenutniLevel++;

            
            if (TrenutniLevel == 5)
                Game.SwitchScene(new HighScoresScene(Igrac.IznosBodova));
            
            Neprijatelj.UcitajNeprijatelje("level" + TrenutniLevel.ToString());
        }
        //Gets play area
        public Vector2 GetPlayArea()
        {
            return PlayPosition + PlayWidth;
        }
    }
}
