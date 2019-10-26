using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacMan
{
    /// <summary>
    /// Classe de Point cardinaux dans le tableau
    /// </summary>
    public class Point : IEquatable<Point>
    {
        public int X { get; set; }  //position en x
        public int Y { get; set; }  //position en y
        /// <summary>
        /// Constructeur d'un point
        /// </summary>
        /// <param name="p_x">position en x</param>
        /// <param name="p_y">position en y</param>
        public Point(int p_x, int p_y)
        {
            X = p_x;
            Y = p_y;
        }

        /// <summary>
        /// Retourne si un point est égal à un autre
        /// </summary>
        /// <param name="other">point en comparaison</param>
        /// <returns>égale ou non égale</returns>
        public bool Equals(Point other)
        {
            return (other.X.Equals(X) && other.Y.Equals(Y));
        }
    }
}
