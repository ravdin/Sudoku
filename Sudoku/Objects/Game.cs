using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sudoku.Objects
{
    /// <summary>
    /// Represents a Sudoku game, with all tiles.
    /// </summary>
    public class Game
    {
        private Tile[,] _tiles = new Tile[9, 9];
        private TileCollection[] _rows = new TileCollection[9];
        private TileCollection[] _columns = new TileCollection[9];
        private TileCollection[] _squares = new TileCollection[9];

        /// <summary>
        /// Set up a game.
        /// </summary>
        /// <param name="setup">The numbers in this game, as a two dimensional array.</param>
        public Game(int?[,] setup)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Tile tile = new Tile(setup[i, j]);
                    tile.XIndex = i;
                    tile.YIndex = j;
                    _tiles[i, j] = tile;
                }
            }

            for (int i = 0; i < 9; i++)
            {
                _rows[i] = new TileCollection(GetRow(_tiles, i));
                _columns[i] = new TileCollection(GetColumn(_tiles, i));

                foreach (Tile tile in _rows[i].Tiles)
                    tile.Row = _rows[i];

                foreach (Tile tile in _columns[i].Tiles)
                    tile.Column = _columns[i];
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    int index = i * 3 + j;
                    _squares[index] = new TileCollection(GetSquare(_tiles, i, j));

                    foreach (Tile tile in _squares[index].Tiles)
                        tile.Square = _squares[index];
                }
            }
        }

        /// <summary>
        /// Return a single tile, based on the x and y coordinates.
        /// </summary>
        public Tile this[int x, int y]
        {
            get { return _tiles[x, y]; }
        }

        /// <summary>
        /// Create a deep copy of the current game.
        /// </summary>
        public Game Copy()
        {
            return new Game(Numbers);
        }

        /// <summary>
        /// Returns true if the game is solved, false otherwise.
        /// </summary>
        public bool IsSolved
        {
            get { return GetFlattenedTiles().All(t => t.IsSolved); }
        }

        /// <summary>
        /// Returns a representation of the game as a two dimensional array of integers.
        /// </summary>
        public int?[,] Numbers
        {
            get
            {
                int?[,] result = new int?[9, 9];
                for (int i = 0; i < 9; i++)
                    for (int j = 0; j < 9; j++)
                        result[i, j] = _tiles[i, j].Number;

                return result;
            }
        }

        /// <summary>
        /// Returns an enumeration of all rows.
        /// </summary>
        public IEnumerable<TileCollection> Rows
        {
            get { return _rows; }
        }

        /// <summary>
        /// Returns an enumeration of all columns.
        /// </summary>
        public IEnumerable<TileCollection> Columns
        {
            get { return _columns; }
        }

        /// <summary>
        /// Returns an enumeration of all squares.
        /// </summary>
        public IEnumerable<TileCollection> Squares
        {
            get { return _squares; }
        }

        /// <summary>
        /// Flattens all tiles in the game to a single list.
        /// </summary>
        public IEnumerable<Tile> GetFlattenedTiles()
        {
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                    yield return _tiles[i, j];
        }

        /// <summary>
        /// Serializes the game, for debugging purposes.
        /// </summary>
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < 9; i++)
            {
                if (i == 3 || i == 6)
                    result.AppendLine("-------|-------|-------");

                string[] rowNumbers = _rows[i].Tiles.Select(t => t.Number).Select(n => n == null ? " " : n.ToString()).ToArray();
                result.AppendFormat(" {0} {1} {2} | {3} {4} {5} | {6} {7} {8}", rowNumbers);
                result.AppendLine("");
            }

            return result.ToString();
        }

        private static IEnumerable<Tile> GetRow(Tile[,] tiles, int rowIndex)
        {
            for (int i = 0; i < 9; i++)
                yield return tiles[rowIndex, i];
        }

        private static IEnumerable<Tile> GetColumn(Tile[,] tiles, int colIndex)
        {
            for (int i = 0; i < 9; i++)
                yield return tiles[i, colIndex];
        }

        private static IEnumerable<Tile> GetSquare(Tile[,] tiles, int vertIndex, int horzIndex)
        {
            int startRow = vertIndex * 3;
            int startColumn = horzIndex * 3;

            for (int i = startRow; i < startRow + 3; i++)
            {
                for (int j = startColumn; j < startColumn + 3; j++)
                {
                    yield return tiles[i, j];
                }
            }
        }
    }
}
