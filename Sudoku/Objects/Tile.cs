using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sudoku.Objects
{
    /// <summary>
    /// Represents a single tile.
    /// </summary>
    public class Tile
    {
        public Tile(int? number)
        {
            _number = number;
        }

        private int? _number;

        public int? Number
        {
            get { return _number; }
            set { _number = value; }
        }

        /// <summary>
        /// Returns the row this tile belongs to.
        /// </summary>
        public TileCollection Row { get; set; }

        /// <summary>
        /// Returns the column this tile belongs to.
        /// </summary>
        public TileCollection Column { get; set; }

        /// <summary>
        /// Get or set the square this tile belongs to.
        /// </summary>
        public TileCollection Square { get; set; }

        /// <summary>
        /// Get or set the x-coordinate for this tile.
        /// </summary>
        public int XIndex { get; set; }

        /// <summary>
        /// Returns the y-coordinate for this tile.
        /// </summary>
        public int YIndex { get; set; }

        public bool IsSolved
        {
            get { return Number != null; }
        }

        /// <summary>
        /// Return all possible candidate numbers for this tile.
        /// </summary>
        public IEnumerable<int> CandidateNumbers
        {
            get
            {
                if (IsSolved)
                    return new List<int>();

                IEnumerable<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                return numbers.Except(Row.Numbers)
                              .Except(Column.Numbers)
                              .Except(Square.Numbers);
            }
        }
    }
}
