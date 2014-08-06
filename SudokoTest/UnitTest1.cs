using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Sudoku.Objects;
using Sudoku.Solver;

namespace SudokuTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestEasy()
        {
            int?[,] setup = new int?[,]
            {
                { null, null, 9, 1, null, null, null, null, 3 },
                { null, 5, 6, 2, 8, 9, 7, 1, 4 },
                { null, 7, 8, 6, 4, 3, 2, null, null },
                { null, null, 2, null, null, null, 1, null, 9 },
                { null, 3, 7, 5, null, null, 8, null, null },
                { null, 1, 4, 9, null, null, null, 6, null },
                { 8, null, 5, null, null, null, null, null, 1 },
                { 4, 6, 3, null, null, null, null, 2, null },
                { 7, 9, null, null, 5, null, null, null, null }
            };

            Game game = new Game(setup);
            Solver solver = new Solver();
            solver.Solve(game);

            Console.Out.Write(game);
            Assert.IsTrue(game.IsSolved);
        }

        [TestMethod]
        public void TestExpert()
        {
            int?[,] setup = new int?[,]
            {
                { 5, null, 6, null, null, 4, null, null, null },
                { 4, null, null, null, null, null, null, 2, null },
                { null, 2, null, null, null, null, 5, null, 7 },
                { null, null, null, 7, null, 8, null, null, null },
                { null, null, null, null, 3, null, 2, null, 1 },
                { null, null, null, null, null, null, 3, null, null },
                { 1, null, null, 4, null, 2, null, null, 5 },
                { null, null, 4, null, null, 5, 8, null, null },
                { null, null, 3, 6, 8, null, null, null, 2 }
            };

            Game game = new Game(setup);
            Solver solver = new Solver();
            solver.Solve(game);

            Console.Out.Write(game);
            Assert.IsTrue(game.IsSolved);
        }
    }
}
