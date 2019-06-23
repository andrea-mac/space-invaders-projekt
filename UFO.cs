using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

namespace AndreaOtterProjekt
{
    class UFO : Entity
    {
        static Image IkonaNeprijatelja = new Image("Slike/enemy4.png");
        AutoTimer IntervalPojavljivanja;
        Vector2 SmjerKretanja;
        public int Bodovi;

        //postavi slučajni timer kad se UFO pojavljuje
        public UFO()
        {
            Random rnd = new Random();

            IntervalPojavljivanja = new AutoTimer(rnd.Next(1000, 2000));
            if (Neprijatelj.SePomaknuo)
            {
                IntervalPojavljivanja.Start();
            }

            BoxCollider collider = new BoxCollider(24, 24, Oznake.Ufo);

            Visible = false;
            Collidable = false;

            Bodovi = rnd.Next(0, 3) * 50;

            AddCollider(collider);
            AddGraphic(IkonaNeprijatelja);
        }

        //UFO smrt
        public void Smrt()
        {
            Visible = false;
            Collidable = false;
        }

        //ako se UFO pojavi, nek se pokrene
        void ProvjeraPojavljivanja()
        {
            Random rnd = new Random();

            if (IntervalPojavljivanja.AtMax)
            {
                int smjer = rnd.Next(0, 2);
                Visible = true;
                Collidable = true;

                SetPosition(new Vector2(850 * smjer - 50, 64));

                if (smjer == 1)
                    SmjerKretanja = new Vector2(-2.0f, 0.0f);
                else
                    SmjerKretanja = new Vector2(2.0f, 0.0f);

                IntervalPojavljivanja.Reset();
            }
        }

        //Azuriraj kretanje UFO
        public void AzurirajKretanje()
        {
            SetPosition(Position + SmjerKretanja);
        }

        public override void Update()
        {
            base.Update();
            IntervalPojavljivanja.Update();
            ProvjeraPojavljivanja();
            AzurirajKretanje();
        }
    }
}
