using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMaze.Models.ClassicMaze
{
    public class Edge : IEquatable<Edge>
    {
        public int Cell1 { get; }
        public int Cell2 { get; }

        public Edge(int cell1, int cell2)
        {
            Cell1 = cell1;
            Cell2 = cell2;
        }

        public bool Equals(Edge other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Cell1 == other.Cell1 && Cell2 == other.Cell2;
        }

        public static bool operator ==(Edge left, Edge right) => Equals(left, right);
        public static bool operator !=(Edge left, Edge right) => !Equals(left, right);
    }
}
