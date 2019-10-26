using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace PacMan
{
    class Program : NetProcessing.Sketch
    {
        static void Main(string[] args)
        {
            new Program().Start();
        }

        /// <summary>
        /// Méthode qui est appelé à chaque frame, dessine le jeu
        /// </summary>
        public override void Draw()
        {
            if (!Jeu.Instance.JeuTerminé)
            {
                Background(new Color(0, 0, 0));
                Jeu.Instance.Tableau.draw();
                PacMan.Instance.draw();

                foreach (var f in Jeu.Instance.Tableau.m_fantomes)
                {
                    f.Animer();
                }
                if (PacMan.Instance.premierMouvement)
                    PacMan.Instance.premierMouvement = false;
                TextSize(15);
                TextAlign(LEFT);
                Fill(255, 255, 255);
                Text($"Score : {Jeu.Instance.Point}", 10, 435);
                Text($"Lives : {Jeu.Instance.Vies}", 100, 435);
                for (int i = 1; i <= Jeu.Instance.Vies; i++)
                {
                    Image(PacMan.Instance.pacmanLEFT1, 150 + 15 * i, 430, 14, 14);
                }
            }
            else if (Jeu.Instance.JeuGagné)
            {
                Jeu.Instance.finirNiveau();
            }
            else
            {
                Jeu.Instance.dessinerEcranFin();
            }

        }

        /// <summary>
        /// Méthode qui est appelé une fois avant le draw pour setup le canva
        /// </summary>
        public override void Setup()
        {
            Size(420, 520);
        }

        /// <summary>
        /// Méthode qui capte les touches qui sont appuyées par l'utilisateur
        /// </summary>
        public override void KeyPressed()
        {
            if (Key == CODED)
            {
                switch (KeyCode)
                {
                    case KC_UP:
                        PacMan.Instance.setProchainDéplacement(Déplacements.Haut);
                        break;
                    case KC_DOWN:
                        PacMan.Instance.setProchainDéplacement(Déplacements.Bas);
                        break;
                    case KC_LEFT:
                        PacMan.Instance.setProchainDéplacement(Déplacements.Gauche);
                        break;
                    case KC_RIGHT:
                        PacMan.Instance.setProchainDéplacement(Déplacements.Droite);
                        break;
                    case KC_F1:
                        if (Jeu.Instance.JeuTerminé)
                        Jeu.Instance.resetJeu();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
