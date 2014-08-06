using System;
using System.Collections.Generic;
using System.Linq;

using Sudoku.Objects;

namespace Sudoku.Solver
{
    /// <summary>
    /// Solves a Sudoku puzzle, or throws InvalidGameException if the puzzle can't be solved.
    /// </summary>
    public class Solver
    {
        public void Solve(Game game)
        {
            bool progress = true;

            while (progress && !game.IsSolved)
            {
                //Console.Out.Write(game);
                //Console.Out.WriteLine("");

                progress = SolveIteration(game);

                if (!progress && !game.IsSolved)
                {
                    progress = SearchSolution(game);
                }
            }
        }

        private bool SolveIteration(Game game)
        {
            bool tileSolved = false;
            IEnumerable<int> allNumbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            // The main idea here: first search all tiles and determine if any unsolved tiles are solvable (i.e. we can narrow the candidates to a single number).
            // Then walk through all of the numbers and determine if there are any rows, columns, or squares for which that number can fit in only one of the nine tiles.
            // Return true if at least one tile is solved this way (so we can try again), false otherwise.

            foreach (Tile tile in game.GetFlattenedTiles())
            {
                if (tile.IsSolved)
                    continue;

                IEnumerable<int> numbers = allNumbers.Except(tile.Row.Numbers)
                                                     .Except(tile.Column.Numbers)
                                                     .Except(tile.Square.Numbers);

                if (numbers.Count() == 0)
                {
                    throw new InvalidGameException();
                }

                if (numbers.Count() == 1)
                {
                    tile.Number = numbers.First();
                    tileSolved = true;
                }
            }

            foreach (int number in allNumbers)
            {
                tileSolved |= SolveForNumber(game.Rows, number);
                tileSolved |= SolveForNumber(game.Columns, number);
                tileSolved |= SolveForNumber(game.Squares, number);
            }

            return tileSolved;
        }

        private bool SearchSolution(Game game)
        {
            // Plan B if SearchIteration doesn't make progress.
            // Make a copy of the game, and do a depth first search on possible permutations, until a feasible solution is found.

            Stack<Game> gameStack = new Stack<Game>();
            gameStack.Push(game);

            bool result = SearchSolutionRecursive(gameStack);

            Game copy = gameStack.Peek();
            var unsolvedTiles = game.GetFlattenedTiles().Where(t => !t.IsSolved);
            foreach (Tile tile in unsolvedTiles)
            {
                tile.Number = copy[tile.XIndex, tile.YIndex].Number;
            }

            return result;
        }

        private bool SearchSolutionRecursive(Stack<Game> gameStack)
        {
            Game game = gameStack.Peek();

            IEnumerable<Tile> testTiles = game.GetFlattenedTiles().Where(t => !t.IsSolved).OrderBy(t => t.CandidateNumbers.Count());
            var testMatrix = testTiles.Select(t => new { XIndex = t.XIndex, YIndex = t.YIndex, CandidateNumbers = t.CandidateNumbers });

            foreach (var item in testMatrix)
            {
                IEnumerable<int> candidateNumbers = new List<int>(item.CandidateNumbers);
                foreach (int testNumber in item.CandidateNumbers)
                {
                    Game copy = game.Copy();
                    copy[item.XIndex, item.YIndex].Number = testNumber;
                    gameStack.Push(copy);

                    bool progress = true;

                    while (progress && !copy.IsSolved)
                    {
                        try
                        {
                            progress = SolveIteration(copy);
                        }
                        catch (InvalidGameException)
                        {
                            break;
                        }

                        if (copy.IsSolved)
                            return true;

                        if (!progress && !copy.IsSolved)
                        {
                            if (SearchSolutionRecursive(gameStack))
                                return true;
                        }
                    }

                    gameStack.Pop();
                }

                return false;
            }

            return false;
        }

        private bool SolveForNumber(IEnumerable<TileCollection> collections, int number)
        {
            bool result = false;

            foreach (TileCollection collection in collections)
            {
                if (collection.Numbers.Contains(number))
                    continue;

                IEnumerable<Tile> tilesForNumber = collection.Tiles.Where(t => t.CandidateNumbers.Contains(number));

                if (tilesForNumber.Count() == 0)
                {
                    throw new InvalidGameException();
                }

                if (tilesForNumber.Count() == 1)
                {
                    tilesForNumber.First().Number = number;
                    result = true;
                }
            }

            return result;
        }
    }
}
