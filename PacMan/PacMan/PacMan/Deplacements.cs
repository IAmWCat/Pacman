using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacMan
{
    public enum Déplacements { Null, Haut, Bas, Gauche, Droite }; //les déplacements possibles

    /// <summary>
    /// Une Interface de deplacement
    /// </summary>
    public interface IComportementDeplacement
    {
        Point Deplacement(Point p_positionActuelle);
    }

    /// <summary>
    /// Méthode de déplacement de fuite
    /// </summary>
    /*public class Fuite : IComportementDeplacement
    {
        Déplacements dernierDeplacement = Déplacements.Null;
        public Point Deplacement(Point p_positionActuelle)
        {
            return Tableau.Instance.getProchainMouvementsPourSeRendre(new Point((PacMan.Instance.getDernierePosition().X - 10) / 21, (PacMan.Instance.getDernierePosition().Y - 10) / 21), p_positionActuelle);
        }
    }*/

    /// <summary>
    /// Méthode de déplacement
    /// </summary>
    public class PoursuiteDirecte : IComportementDeplacement
    {
        Déplacements dernierDeplacement = Déplacements.Null;
        public Point Deplacement(Point p_positionActuelle)
        {
            if (PacMan.Instance.premierMouvement)
            {
                return p_positionActuelle;
            }
            return UtilDéplacement.DéplacementEnPoint(dernierDeplacement = Tableau.Instance.getProchainMouvementsPourSeRendre(PacMan.Instance.getDernierePosition(), p_positionActuelle, dernierDeplacement));
        }
    }

    /// <summary>
    /// Méthode de déplacement àléatoire
    /// </summary>
    public class PoursuiteAléatoire : IComportementDeplacement
    {
        Random nombre = new Random(233);
        int ancienPastille = 1;
        Déplacements dernierDeplacement = Déplacements.Null;
        int compteur = 0;
        public Point Deplacement(Point p_positionActuelle)
        {
            if (PacMan.Instance.premierMouvement)
            {
                return p_positionActuelle;
            }
            if (Tableau.Instance.m_nombrePastilles != ancienPastille)
                nombre = new Random(Tableau.Instance.m_nombrePastilles);
            ancienPastille = Tableau.Instance.m_nombrePastilles;
            int chiffre = (nombre.Next(0, 200) % 4 + 1);
            Déplacements deplacementDepart;
            switch (chiffre)
            {
                case 1:
                    deplacementDepart = Déplacements.Bas;
                    break;
                case 2:
                    deplacementDepart = Déplacements.Droite;
                    break;
                case 3:
                    deplacementDepart = Déplacements.Gauche;
                    break;
                default:
                    deplacementDepart = Déplacements.Haut;
                    break;
            }
            if (Tableau.Instance.estDisponible(p_positionActuelle, deplacementDepart) && UtilDéplacement.EstDansMilieuCase(p_positionActuelle)
                && UtilDéplacement.DéplacementEnPoint(deplacementDepart).X * -1 != UtilDéplacement.DéplacementEnPoint(dernierDeplacement).X)
            {
                dernierDeplacement = deplacementDepart;
                return UtilDéplacement.DéplacementEnPoint(deplacementDepart);
            }
            if (Tableau.Instance.estDisponible(p_positionActuelle, dernierDeplacement))
                return UtilDéplacement.DéplacementEnPoint(dernierDeplacement);

            return new Point(0, 0);
        }
    }

    /// <summary>
    /// Méthode de déplacement indirectement vers Pacman
    /// </summary>
    public class PoursuiteIndirecte : IComportementDeplacement
    {
        Déplacements dernierDeplacement = Déplacements.Droite;
        public Point Deplacement(Point p_positionActuelle)
        {
            if (Tableau.Instance.estDisponible(p_positionActuelle, PacMan.Instance.GetDéplacements()) 
                && UtilDéplacement.EstDansMilieuCase(p_positionActuelle) && PacMan.Instance.GetDéplacements() != Déplacements.Null)
            {
                dernierDeplacement = PacMan.Instance.GetDéplacements();
                return UtilDéplacement.DéplacementEnPoint(dernierDeplacement);
            }
            if (Tableau.Instance.estDisponible(p_positionActuelle, dernierDeplacement))
                return UtilDéplacement.DéplacementEnPoint(dernierDeplacement);
            return new Point(0, 0);
        }
    }

    /// <summary>
    /// Méthode de déplacement inversé à Pacman
    /// </summary>
    public class PoursuiteInversée : IComportementDeplacement
    {
        Déplacements dernierDeplacement = Déplacements.Gauche;
        public Point Deplacement(Point p_positionActuelle)
        {
            Déplacements deplacementDepart = Déplacements.Droite;
            switch (PacMan.Instance.GetDéplacements())
            {
                case Déplacements.Null:
                    deplacementDepart = Déplacements.Null;
                    break;
                case Déplacements.Haut:
                    deplacementDepart = Déplacements.Bas;
                    break;
                case Déplacements.Bas:
                    deplacementDepart = Déplacements.Haut;
                    break;
                case Déplacements.Gauche:
                    deplacementDepart = Déplacements.Droite;
                    break;
                case Déplacements.Droite:
                    deplacementDepart = Déplacements.Gauche;
                    break;
                default:
                    deplacementDepart = Déplacements.Null;
                    break;
            }
            if (Tableau.Instance.estDisponible(p_positionActuelle, deplacementDepart) && UtilDéplacement.EstDansMilieuCase(p_positionActuelle)
                && PacMan.Instance.GetDéplacements() != Déplacements.Null)
            {
                dernierDeplacement = deplacementDepart;
                return UtilDéplacement.DéplacementEnPoint(deplacementDepart);
            }
            if (Tableau.Instance.estDisponible(p_positionActuelle, dernierDeplacement))
                return UtilDéplacement.DéplacementEnPoint(dernierDeplacement);

            return new Point(0, 0);
        }
    }
}
