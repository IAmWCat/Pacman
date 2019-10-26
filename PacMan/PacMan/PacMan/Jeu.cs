using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NetProcessing.Sketch;


namespace PacMan
{
    public enum Pastilles { Petites, Grosses, Fantomes }; //les pastilles possibles

    class Jeu
    {
        public int nbFantomeTué { get; set; } = 0;  //nombre de fantome tués avec le meme bonbon
        public int Point { get; private set; }  // score du jeu
        public int Vies { get; private set; }   //nombre de vies de pacman
        public bool JeuTerminé { get; private set; }    //si le jeu est terminé ou non
        public bool JeuGagné { get; private set; }  //si le jeu est gagné ou perdu
        public static Jeu Instance = new Jeu(); //intance du jeu
        public Tableau Tableau { get; private set; }    //tableau de jeu
        private int cptFin; //compteur de fin
        /// <summary>
        /// contructeur privé de jeu
        /// </summary>
        private Jeu()
        {
            Tableau = Tableau.Instance;
            Point = 0;
            cptFin = 0;
            Vies = 3;
            JeuTerminé = false;
            JeuGagné = false;
        }

        /// <summary>
        /// Méthode qui fait perdre une vie
        /// </summary>
        public void perdreVie()
        {
            if (Vies != 0)
            {
                Jeu.Instance.EnleverPoint();
                Vies--;
                if (Vies == 0)
                    JeuTerminé = true;
                else
                {
                    PacMan.Instance.resetPacman();
                    foreach (Fantome f in Tableau.m_fantomes)
                    {
                        f.InitialiserFantome();
                    }
                }
            }
        }

        /// <summary>
        /// Méthode qui met fin au niveau
        /// </summary>
        public void finirNiveau()
        {
            JeuTerminé = true;
            JeuGagné = true;
            Background(0, 0, 0);
            TextAlign(CENTER);
            TextSize(30);
            if (cptFin++ % 60 < 30)
                Fill(80, 80, 255);
            ImageMode(CENTER);
            if (cptFin % 30 < 15)
                Image(PacMan.Instance.pacmanLEFT1, 200, 250, 40, 40);
            else
                Image(PacMan.Instance.pacmanLEFT2, 200, 250, 40, 40);

            Text("You Win!", 200, 200);
            Fill(255, 255, 255);
            Text($"Score : {Point}", 200, 320);
            Text("Press F1 to play next Level!", 200, 350);
        }

        /// <summary>
        /// Méthode qui affiche l'écran de fin de jeu
        /// </summary>
        public void dessinerEcranFin()
        {            Background(0, 0, 0);
            TextAlign(CENTER);
            TextSize(30);
            if (cptFin++ % 60 < 30)
                Fill(80, 80, 255);
            ImageMode(CENTER);
            if (cptFin % 30 < 15)
                Image(PacMan.Instance.pacmanLEFT1, 200, 250, 40, 40);
            else
                Image(PacMan.Instance.pacmanLEFT2, 200, 250, 40, 40);

            Text("Game Over!", 200, 200);
            Fill(255, 255, 255);
            Text($"Score : {Point}", 200, 320);
            Text("Press F1 to retry!", 200, 350);
        }

        /// <summary>
        /// Méthode qui ajoute des points
        /// </summary>
        /// <param name="p_pastille">le type de bouffe</param>
        public void AjouterPoint(Pastilles p_pastille)
        {
            if (p_pastille == Pastilles.Grosses)
                Point += 50;
            else if (p_pastille == Pastilles.Petites)
                Point += 10;
            else if (p_pastille == Pastilles.Fantomes)
                Point += 200 * (2 ^ nbFantomeTué++);
        }

        /// <summary>
        /// Enlève des points selon les vies restantes
        /// </summary>
        public void EnleverPoint()
        {
            Point -= 300 / Vies;
        }

        /// <summary>
        /// Réinitialise le jeu
        /// </summary>
        public void resetJeu()
        {
            PacMan.Instance.resetPacman();
            Tableau.resetTableau();
            Instance = new Jeu();
        }
    }
}
