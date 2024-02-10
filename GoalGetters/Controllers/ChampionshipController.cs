using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoalGetters.Controllers
{
    public class ChampionshipController : Controller
    {
        // GET: ChampionshipController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ChampionshipController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ChampionshipController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ChampionshipController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ChampionshipController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ChampionshipController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ChampionshipController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ChampionshipController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
