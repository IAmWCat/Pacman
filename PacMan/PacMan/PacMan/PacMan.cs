using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NetProcessing.Sketch;

namespace PacMan
{
    public enum PacManType { PacManNormal, PacManMoyen, SuperPacMan }; //les types pour le pacman

    /// <summary>
    /// Classe du Pacman
    /// </summary>
    public class PacMan
    {
        /// <summary>
        /// constructeur privé d'un Pacman
        /// </summary>
        private PacMan()
        {
            DernièrePosition = new Point(Tableau.XDepart, Tableau.YDepart);
            PositionActuelle = new Point(Tableau.XDepart, Tableau.YDepart);
            m_dernierDeplacement = Déplacements.Null;
            m_prochainDéplacement = Déplacements.Null;
            AMangéBonbon = false;
            tempsPasséEnBonbon = 0;
            cptDessinateur = 0;
        }

        public static PacMan Instance { get; private set; } = new PacMan();// l'instance de pacman
        public Déplacements m_dernierDeplacement; //dernier déplacement de pacman
        static Déplacements m_prochainDéplacement;  //prochain déplacement de pacman

        public const int TempsBonbon = 5;   //temps de durée du bonbon
        private const int m_vitesse = 1;    //vitesse de pacman
        private int cptDessinateur; //compteur de nombre de fois qu'il est dessiné

        public bool AMangéBonbon { get; set; } //si le pacman a mangé un bonbon
        private int tempsPasséEnBonbon; //le temps que pacman a passé en bonbon
        private Point DernièrePosition { get; set; } //la derniere position de pacman
        private Point PositionActuelle { get; set; } //la position actuelle de pacman   

        //Images de PacMan : 
        public PImage pacmanLEFT1 = LoadImage("../../img/pacmanLEFT.png");
        public PImage pacmanLEFT2 = LoadImage("../../img/pacman2LEFT.png");
        private PImage pacmanUP1 = LoadImage("../../img/pacmanUP.png");
        private PImage pacmanUP2 = LoadImage("../../img/pacman2UP.png");
        private PImage pacmanDOWN1 = LoadImage("../../img/pacmanDOWN.png");
        private PImage pacmanDOWN2 = LoadImage("../../img/pacman2DOWN.png");
        private PImage pacmanRIGHT1 = LoadImage("../../img/pacmanRIGHT.png");
        private PImage pacmanRIGHT2 = LoadImage("../../img/pacman2RIGHT.png");

        public bool premierMouvement = false;   //si c'est le premier déplacement de Pacman

        // l'évenement lorsque Pacman mange un bonbon :
        public event BonbonHandler MangerBonbon;
        public delegate void BonbonHandler(PacMan p_pacman, PacManType p_type);

        /// <summary>
        /// Modifie le prochain déplacement
        /// </summary>
        /// <param name="p_deplacement">le prochain déplacement</param>
        public void setProchainDéplacement(Déplacements p_deplacement)
        {
            m_prochainDéplacement = p_deplacement;
        }

        /// <summary>
        /// Obtient de le dernier déplacement
        /// </summary>
        /// <returns>le dernier déplacement</returns>
        public Déplacements GetDéplacements()
        {
            return m_dernierDeplacement;
        }

        /// <summary>
        /// Obtient la position actuelle
        /// </summary>
        /// <returns>la position actuelle</returns>
        public Point getPosition()
        {
            return PositionActuelle;
        }

        /// <summary>
        /// Obtient la vitesse de pacman et gère l'exitation du bonbon
        /// </summary>
        /// <returns>vitesse</returns>
        public int getVitesse()
        {
            if (!AMangéBonbon)
                return m_vitesse;
            if (TempsBonbon - (tempsPasséEnBonbon++ / 60) == 0)
            {
                MangerBonbon?.Invoke(this, PacManType.PacManNormal);
                tempsPasséEnBonbon = 0;
                AMangéBonbon = false;
                Jeu.Instance.nbFantomeTué = 0;
            }
            else if (TempsBonbon - (tempsPasséEnBonbon / 60) <= 1)
            {
                MangerBonbon?.Invoke(this, PacManType.PacManMoyen);
            }
            return m_vitesse * 3;
        }

        /// <summary>
        /// Démarre l'exitation du bonbon
        /// </summary>
        public void bonbonTime()
        {
            MangerBonbon?.Invoke(this, PacManType.SuperPacMan);
            AMangéBonbon = true;
            tempsPasséEnBonbon = 0;
        }

        /// <summary>
        /// Déplace Pacman si possible
        /// </summary>
        /// <returns>La nouvelle position</returns>
        private Point effectuerDeplacement()
        {
            Point déplacement = UtilDéplacement.DéplacementEnPoint(m_dernierDeplacement);
            Tableau.Instance.PacManMange(PositionActuelle);
            int vitesse = getVitesse();
            if (Tableau.Instance.estPacmanMort(PositionActuelle))
            {
                Jeu.Instance.perdreVie();
                return PositionActuelle;
            }
            return new Point(DernièrePosition.X + déplacement.X * vitesse, DernièrePosition.Y + déplacement.Y * vitesse);
        }

        /// <summary>
        /// Obtient la dernière position de Pacman
        /// </summary>
        /// <returns>la derniere position</returns>
        public Point getDernierePosition()
        {
            return DernièrePosition;
        }

        /// <summary>
        /// Obtient la bonne image de pacman selon son déplacement
        /// </summary>
        /// <param name="p_déplacement">le déplacement de Pacman</param>
        /// <returns>l'image de pacman</returns>
        private PImage choisirImage(Déplacements p_déplacement)
        {
            if (cptDessinateur++ % 20 < 10)
            {
                switch (p_déplacement)
                {
                    case Déplacements.Bas:
                        return pacmanDOWN1;
                    case Déplacements.Haut:
                        return pacmanUP1;
                    case Déplacements.Gauche:
                        return pacmanRIGHT1;
                    case Déplacements.Droite:
                        return pacmanLEFT1;
                    default:
                        return pacmanLEFT1;
                }
            }
            switch (p_déplacement)
            {
                case Déplacements.Bas:
                    return pacmanDOWN2;
                case Déplacements.Haut:
                    return pacmanUP2;
                case Déplacements.Gauche:
                    return pacmanRIGHT2;
                case Déplacements.Droite:
                    return pacmanLEFT2;
                default:
                    return pacmanLEFT2;
            }
        }

        /// <summary>
        /// Dessine Pacman
        /// </summary>
        public void draw()
        {
            PImage image = choisirImage(m_dernierDeplacement);

            if (Tableau.Instance.estDisponible(PositionActuelle, m_prochainDéplacement) && m_prochainDéplacement != m_dernierDeplacement)
            {
                if (UtilDéplacement.DéplacementEnPoint(m_dernierDeplacement).X * -1 == UtilDéplacement.DéplacementEnPoint(m_prochainDéplacement).X
                    && UtilDéplacement.DéplacementEnPoint(m_dernierDeplacement).Y * -1 == UtilDéplacement.DéplacementEnPoint(m_prochainDéplacement).Y)
                {
                    m_dernierDeplacement = m_prochainDéplacement;
                    DernièrePosition = PositionActuelle;
                    PositionActuelle = effectuerDeplacement();
                }
                else
                {
                    if ((PositionActuelle.X - 10) % 21 == 0 && (PositionActuelle.Y - 10) % 21 == 0)
                    {
                        m_dernierDeplacement = m_prochainDéplacement;
                        DernièrePosition = PositionActuelle;
                        PositionActuelle = effectuerDeplacement();
                    }
                    if (Tableau.Instance.estDisponible(PositionActuelle, m_dernierDeplacement))
                    {
                        DernièrePosition = PositionActuelle;
                        PositionActuelle = effectuerDeplacement();
                    }
                }
            }
            else
            {
                if (Tableau.Instance.estDisponible(PositionActuelle, m_dernierDeplacement))
                {
                    DernièrePosition = PositionActuelle;
                    PositionActuelle = effectuerDeplacement();
                }
            }
            ImageMode(CENTER);
            Image(image, PositionActuelle.X, PositionActuelle.Y, 18, 18);
            if (Tableau.Instance.estPacmanMort(PositionActuelle))
                Jeu.Instance.perdreVie();
        }

        /// <summary>
        /// Réinitialise le Pacman
        /// </summary>
        public void resetPacman()
        {
            Instance = new PacMan();
        }

    }
}
