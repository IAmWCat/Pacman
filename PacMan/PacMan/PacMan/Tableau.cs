using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NetProcessing.Sketch;


namespace PacMan
{
    /// <summary>
    /// Classe du Tableau de jeu
    /// </summary>
    class Tableau
    {
        public static Tableau Instance { get; private set; } = new Tableau();   //instance du tableau
        public const int XDepart = 4 * 21 + 10; //position de départ en x
        public const int YDepart = 10 * 21 + 10;    //position de départ en y

        private List<Point> m_cheminFait = new List<Point>();
        private List<List<int>> m_tableau;  //le tableau en format XparY
        public List<Fantome> m_fantomes;    //la liste des fantomes
        private int m_cptNbDessin = 0;  //le compteur du nombre de fois dessiné
        public int m_nombrePastilles = 0;   //le nombre de pastilles sur le jeu
        private List<Déplacements> m_chemin = new List<Déplacements>();
        /// <summary>
        /// Constructeur de tableau de jeu
        /// </summary>
        private Tableau()
        {
            m_fantomes = new List<Fantome>();
            m_fantomes.Add(new FantomeRouge());
            m_fantomes.Add(new FantomeBleu());
            m_fantomes.Add(new FantomeRose());
            m_fantomes.Add(new FantomeOrange());
            m_tableau = new List<List<int>>();
            initialiserTableau();
        }

        /// <summary>
        /// Affichage du tableau
        /// </summary>
        public void draw()
        {
            m_cptNbDessin++;
            for (int i = 0; i < 20; i++)
            {
                for (int j = 1; j < 21; j++)
                {
                    if (m_tableau[i][j - 1] == 0)
                    {
                        Stroke(25, 0, 255);
                        Rect(i * 21, (j - 1) * 21, 21, 21);
                    }
                    else if (m_tableau[i][j - 1] == 3)
                    {
                        Stroke(205, 0, 0);
                        Rect(i * 21, (j - 1) * 21, 21, 21);
                    }
                    else if (m_tableau[i][j - 1] == 2)
                    {
                        Stroke(225, 225, 0);
                        StrokeWeight(5);
                        Point(i * 21 + 11, (j - 1) * 21 + 11);
                        StrokeWeight(2);
                    }
                    else if (m_tableau[i][j - 1] == 5)
                    {
                        if (m_cptNbDessin % 60 < 30)
                            Stroke(200, 200, 255);
                        else
                            Stroke(230, 230, 255);
                        StrokeWeight(10);
                        Point(i * 21 + 11, (j - 1) * 21 + 11);
                        StrokeWeight(2);
                    }
                }
            }
        }

        /// <summary>
        /// Initialisation du tableau de jeu
        /// </summary>
        private void initialiserTableau()
        {
            for (int i = 0; i < 20; i++)
            {
                List<int> ligne = new List<int>();
                for (int j = 0; j < 20; j++)
                {
                    ligne.Add(0);
                }
                m_tableau.Add(ligne);
            }
            string a =
                "XXXXXXXXXXXXXXXXXXXX" +
                "X5000000XXXX0000005X" +
                "X0XXXXX000000XXXXX0X" +
                "X0XXXXX0XXXX0XXXXX0X" +
                "X000000000000000000X" +
                "X0XX0XXXXXXXXXX0XX0X" +
                "X0XX0XXXXXXXXXX0XX0X" +
                "X00X0000XXXX0000X00X" +
                "XX0X0XX000000XX0X0XX" +
                "XX000XXXXXXXXXX000XX" +
                "XXXX100000000000XXXX" +
                "XX000XX0XXXX0XX000XX" +
                "X00X0XX0XXXX0XX0X00X" +
                "X0XX0XX0XXXX0XX0XX0X" +
                "X0XX0XX0XXXX0XX0XX0X" +
                "X000000000000000000X" +
                "X0XXXXX0XXXX0XXXXX0X" +
                "X0XXXXX000000XXXXX0X" +
                "X5000000XXXX0000005X" +
                "XXXXXXXXXXXXXXXXXXXX";
            a = a.Replace('0', '2');
            a = a.Replace('X', '0');
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    m_tableau[j][i] = int.Parse(a.ElementAt((20 * i) + j).ToString());
                }
            }
            foreach (char c in a)
            {
                if (c == '2' || c == '5')
                    m_nombrePastilles++;
            }
        }

        /// <summary>
        /// Obtient si une position est disponible
        /// </summary>
        /// <param name="p_position">position</param>
        /// <param name="p_deplacement">déplacement</param>
        /// <returns>oui ou non</returns>
        public bool estDisponible(Point p_position, Déplacements p_deplacement)
        {
            Point deplacement = UtilDéplacement.DéplacementEnPoint(p_deplacement);

            if (p_position.X + deplacement.X <= 13 || p_position.X + deplacement.X >= 390 ||
                p_position.Y + deplacement.Y <= 13 || p_position.Y + deplacement.Y >= 390)
                return false;
            if (m_tableau[(p_position.X + (deplacement.X * 11)) / 21][(p_position.Y + deplacement.Y * 11) / 21] != 0
                && m_tableau[(p_position.X + deplacement.X * 11) / 21][(p_position.Y + deplacement.Y * 11) / 21] != 3)
                return true;
            else
                return false;
        }

        /// <summary>
        /// structure interne d'un emplacement
        /// </summary>
        internal struct Emplacement
        {
            internal Point point;
            internal int valeur;
        }

        /// <summary>
        /// Méthode qui renvoit le prochain point à aller pour se rendre à une position donnée
        /// </summary>
        /// <param name="p_destination">destination</param>
        /// <param name="p_depart">départ</param>
        /// <returns>la position du prochain mouvement</returns>
        public Déplacements getProchainMouvementsPourSeRendre(Point p_destination, Point p_depart, Déplacements p_dernierDéplacement)
        {
            if (p_destination.X < p_depart.X && estDisponible(p_depart, Déplacements.Gauche) && UtilDéplacement.EstDansMilieuCase(p_depart))
            {
                return Déplacements.Gauche;
            }
            if (p_destination.X > p_depart.X && estDisponible(p_depart, Déplacements.Droite) && UtilDéplacement.EstDansMilieuCase(p_depart))
            {
                return Déplacements.Droite;
            }
            if (p_destination.Y < p_depart.Y && estDisponible(p_depart, Déplacements.Haut) && UtilDéplacement.EstDansMilieuCase(p_depart))
            {
                return Déplacements.Haut;
            }
            if (p_destination.Y > p_depart.Y && estDisponible(p_depart, Déplacements.Bas) && UtilDéplacement.EstDansMilieuCase(p_depart))
            {
                return Déplacements.Bas;
            }
            if (estDisponible(p_depart, p_dernierDéplacement))
            {
                return p_dernierDéplacement;
            }
            return Déplacements.Null;
        }

        /// <summary>
        /// Méthode qui fait manger pacman
        /// </summary>
        /// <param name="p_position">position de pacman</param>
        public void PacManMange(Point p_position)
        {
            if (m_tableau[p_position.X / 21][p_position.Y / 21] == 2)
            {
                m_tableau[p_position.X / 21][p_position.Y / 21] = 1;
                m_nombrePastilles--;
                if (m_nombrePastilles == 0)
                {
                    Jeu.Instance.finirNiveau();
                    return;
                }
                Jeu.Instance.AjouterPoint(Pastilles.Petites);
            }
            else
            {
                if (m_tableau[p_position.X / 21][p_position.Y / 21] == 5 && (p_position.X - 10) % 21 == 0 && (p_position.Y - 10) % 21 == 0)
                {
                    m_tableau[p_position.X / 21][p_position.Y / 21] = 1;
                    m_nombrePastilles--;
                    if (m_nombrePastilles == 0)
                    {
                        Jeu.Instance.finirNiveau();
                        return;
                    }
                    Jeu.Instance.AjouterPoint(Pastilles.Grosses);
                    PacMan.Instance.bonbonTime();
                }
            }
        }

        /// <summary>
        /// Méthode qui effectue la mort de Pacman
        /// </summary>
        /// <param name="p_position">position de pacman</param>
        /// <returns>mort ou pasmort</returns>
        public bool estPacmanMort(Point p_position)
        {
            foreach (Fantome f in m_fantomes)
            {
                if (Math.Abs(f.m_position.X - p_position.X) <= 12 && Math.Abs(f.m_position.Y - p_position.Y) <= 12)
                {
                    if (PacMan.Instance.AMangéBonbon && f.peutEtreMangé)
                    {
                        f.Meurt();
                        Jeu.Instance.AjouterPoint(Pastilles.Fantomes);
                        f.peutEtreMangé = false;
                        return false;
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Réinitialise le tableau de jeu
        /// </summary>
        public void resetTableau()
        {
            Instance = new Tableau();
        }

    }
}
