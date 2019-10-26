using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacMan
{
    /// <summary>
    /// classe de fonction utiles à tout les déplacements du jeu
    /// </summary>
    public class UtilDéplacement
    {
        /// <summary>
        /// Obtient point de déplacement
        /// </summary>
        /// <param name="p_déplacement">le déplacement à faire</param>
        /// <returns>Le point deplacement</returns>
        public static Point DéplacementEnPoint(Déplacements p_déplacement)
        {
            Point Déplacement = new Point(0, 0);
            switch (p_déplacement)
            {
                case Déplacements.Bas:
                    Déplacement.X = 0;
                    Déplacement.Y = 1;
                    break;
                case Déplacements.Droite:
                    Déplacement.X = 1;
                    Déplacement.Y = 0;
                    break;
                case Déplacements.Gauche:
                    Déplacement.X = -1;
                    Déplacement.Y = 0;
                    break;
                case Déplacements.Haut:
                    Déplacement.X = 0;
                    Déplacement.Y = -1;
                    break;
                default:
                    Déplacement.X = 0;
                    Déplacement.Y = 0;
                    break;
            }
            return Déplacement;
        }

        public static bool EstDansMilieuCase(Point p_positionActuelle)
        {
            return ((p_positionActuelle.X - 10) % 21 == 0
                && (p_positionActuelle.Y - 10) % 21 == 0);
        }
    }
}
