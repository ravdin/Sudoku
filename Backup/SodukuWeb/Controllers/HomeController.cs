using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading;

using Soduku.Objects;
using Soduku.Solver;

namespace SodukuWeb.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(int k = 0)
        {
            int?[,] numbers = new int?[9, 9];

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    string key = string.Format("f{0}{1}", i, j);
                    int number;
                    if (int.TryParse(Request[key], out number))
                        numbers[i, j] = number;
                }
            }

            Game game = new Game(numbers);
            Solver solver = new Solver();

            try
            {
                DateTime start = DateTime.Now;
                solver.Solve(game);
                DateTime end = DateTime.Now;

                ViewBag.Interval = string.Format("Solved in {0} ms.", end.Subtract(start).Milliseconds);
            }
            catch (InvalidGameException)
            {
                ViewBag.Message = "Invalid entry.  There is no solution for the game!";
            }

            ViewBag.Numbers = game.Numbers;
            return View();
        }
    }
}
