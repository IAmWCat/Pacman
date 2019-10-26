using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NetProcessing.Sketch;

namespace PacMan
{
    /// <summary>
    /// Classe parente pour les fantomes
    /// </summary>
    public abstract class Fantome
    {
        public IComportementDeplacement m_deplacement;  // la méthode de déplacement du fantome
        internal IComportementDeplacement m_deplacementDeBase;  // la méthode de déplacement de départ
        public Point m_position = new Point(0, 0);  // La position du fantome
        public PImage m_image; //une image du fantome
        public PImage m_image2; //une deuxième image du fantome
        private int cptDessinateur = 0; //compteur du nombre de fois que le fantome est dessiné
        public bool peutEtreMangé = false; //si le fantome peut être mangé
        public bool flash = false; //si le fantome flash
        public PImage m_imagePeur = LoadImage("../../Peur.png"); //image de peur du fantome

        /// <summary>
        /// Initialisation du fantomes
        /// </summary>
        public abstract void InitialiserFantome();

        /// <summary>
        /// Tue le fantome
        /// </summary>
        public void Meurt()
        {
            InitialiserFantome();
        }

        /// <summary>
        /// Inscrit le fantome au Pacman qui mange un bonbon
        /// </summary>
        /// <param name="p_pacman">Le Pacman du jeu</param>
        public void Subscribe(PacMan p_pacman)
        {
            p_pacman.MangerBonbon += new PacMan.BonbonHandler(AvoirPeur);
        }

        /// <summary>
        /// Méthode qui réagit au Pacman qui mange un bonbon selon le type de Pacman
        /// </summary>
        /// <param name="p_pacman">Le Pacman qui agit</param>
        /// <param name="p_type">Le type du Pacman</param>
        public void AvoirPeur(PacMan p_pacman, PacManType p_type)
        {
            switch (p_type)
            {
                case PacManType.PacManNormal:
                    peutEtreMangé = false;
                    flash = false;
                    resetDéplacement();
                    break;
                case PacManType.PacManMoyen:
                    flash = true;
                    break;
                case PacManType.SuperPacMan:
                    peutEtreMangé = true;
                    flash = false;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Méthode qui réinitialise la méthode de déplacement du fantome
        /// </summary>
        public void resetDéplacement()
        {
            if (m_deplacement.GetType() != m_deplacementDeBase.GetType())
                m_deplacement = m_deplacementDeBase;
        }

        /// <summary>
        /// Méthode qui change le déplacement du fantome
        /// </summary>
        /// <param name="p_deplacement">le nouveau déplacement</param>
        public void ChangerDéplacement(IComportementDeplacement p_deplacement)
        {
            m_deplacement = p_deplacement;
        }

        /// <summary>
        /// Méthode qui anime le fantome
        /// </summary>
        internal void Animer()
        {
            this.m_position.X += this.m_deplacement.Deplacement(m_position).X;
            this.m_position.Y += this.m_deplacement.Deplacement(m_position).Y;

            ImageMode(Parameter.Center);
            if (flash && peutEtreMangé)
            {
                if (FrameCount % 10 < 5)
                    Image(m_imagePeur, m_position.X, m_position.Y);
                else
                    Image(m_image, m_position.X, m_position.Y);
            }
            else if (peutEtreMangé)
                Image(m_imagePeur, m_position.X, m_position.Y);
            else if (cptDessinateur++ % 20 < 10)
                Image(m_image, m_position.X, m_position.Y);
            else
                Image(m_image2, m_position.X, m_position.Y);

        }
    }

    /// <summary>
    /// Blinky
    /// </summary>
    public class FantomeRouge : Fantome
    {
        public FantomeRouge()
        {
            InitialiserFantome();
        }

        /// <summary>
        /// Initialise Blinky 
        /// </summary>
        public override void InitialiserFantome()
        {
            m_position.X = 6 * 21 + 10;
            m_position.Y = 4 * 21 + 10;
            m_deplacement = new PoursuiteDirecte();
            m_deplacementDeBase = new PoursuiteDirecte();
            m_image = LoadImage("../../Blinky.png");
            m_image2 = LoadImage("../../Blinky.png");
            Subscribe(PacMan.Instance);
        }
    }

    /// <summary>
    /// Inky
    /// </summary>
    public class FantomeBleu : Fantome
    {
        public FantomeBleu()
        {
            InitialiserFantome();
        }

        /// <summary>
        /// Initialise Inky
        /// </summary>
        public override void InitialiserFantome()
        {
            m_position.X = 8 * 21 + 10;
            m_position.Y = 4 * 21 + 10;
            m_deplacement = new PoursuiteIndirecte();
            m_deplacementDeBase = new PoursuiteIndirecte();
            m_image = LoadImage("../../Inky.png");
            m_image2 = LoadImage("../../Inky2.png");
            Subscribe(PacMan.Instance);
        }
    }

    /// <summary>
    /// Pinky
    /// </summary>
    public class FantomeRose : Fantome
    {
        public FantomeRose()
        {
            InitialiserFantome();
        }

        /// <summary>
        /// Initialise Pinky
        /// </summary>
        public override void InitialiserFantome()
        {
            m_position.X = 11 * 21 + 10;
            m_position.Y = 4 * 21 + 10;
            m_deplacement = new PoursuiteInversée();
            m_deplacementDeBase = new PoursuiteInversée();
            m_image = LoadImage("../../Pinky.png");
            m_image2 = LoadImage("../../Pinky.png");
            Subscribe(PacMan.Instance);
        }
    }

    /// <summary>
    /// Clyde
    /// </summary>
    public class FantomeOrange : Fantome
    {
        public FantomeOrange()
        {
            InitialiserFantome();
        }

        /// <summary>
        /// Initialisation de Clyde
        /// </summary>
        public override void InitialiserFantome()
        {
            m_position.X = 13 * 21 + 10;
            m_position.Y = 4 * 21 + 10;
            m_deplacement = new PoursuiteAléatoire();
            m_deplacementDeBase = new PoursuiteAléatoire();
            m_image = LoadImage("../../Clyde.png");
            m_image2 = LoadImage("../../Clyde2.png");
            Subscribe(PacMan.Instance);
        }
    }

}
