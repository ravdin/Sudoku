using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sudoku.Objects
{
    public class TileCollection
    {
        private Tile[] _tiles;

        public TileCollection(IEnumerable<Tile> tiles)
        {
            _tiles = tiles.ToArray();
        }

        public Tile[] Tiles
        {
            get { return _tiles; }
        }

        public bool IsSolved
        {
            get { return _tiles.All(t => t.IsSolved); }
        }

        public IEnumerable<int> Numbers
        {
            get { return _tiles.Where(t => t.IsSolved).Select(t => t.Number.Value); }
        }

        public IEnumerable<int> MissingNumbers
        {
            get
            {
                IEnumerable<int> allNumbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                return allNumbers.Except(Numbers);
            }
        }
    }
}
