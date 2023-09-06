using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMaze.Models.ClassicMaze
{
    public class DisjointSets
    {
        private readonly int[] _cells;

        public DisjointSets(int cells)
        {
            _cells = new int[cells];
            for (var id = 0; id < cells; id++)
            {
                _cells[id] = -1;
            }
        }

        public int Find(int cell)
        {
            var parent = _cells[cell];
            return parent < 0 ? cell : Find(parent);
        }

        public int Size(int set) => _cells[Find(set)] * -1;

        public void Union(int set1, int set2)
        {
            set1 = Find(set1);
            set2 = Find(set2);

            if (Size(set1) > Size(set2))
            {
                _cells[set2] = set1;
            }
            else _cells[set1] = set2;
        }

        public int Count => _cells.Count(cell => cell < 0);
    }
}
