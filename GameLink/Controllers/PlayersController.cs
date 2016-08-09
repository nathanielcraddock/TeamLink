using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GameLink;
using GameLink.Models;

namespace GameLink.Controllers
{
    public class PlayersController : Controller
    {
        private PlayerDBEntities db = new PlayerDBEntities();
        SearchingPlayerModel search = new SearchingPlayerModel();
        public ActionResult Index()
        {
            return View();
        }
        // GET: Players
        public ActionResult Results(SearchingPlayerModel search)
        {
            var players = from p in db.Players
                              select p
                              ;
            var searchResult = players.Where(p => p.GameID == search.gameID && p.GamerStyleID == search.gamerstyleid && p.PlatformID == search.platformid && p.RegionID == search.regionid);
            //deleteplayer();                                          
            return View(searchResult.ToList());
        }


        // GET: Players/Create
        public ActionResult Search()
        {
            
            ViewBag.GameID = new SelectList(db.Games, "GameID", "GameName");
            ViewBag.GamerStyleID = new SelectList(db.GamerStyles, "GamerStyleID", "GamerStyle1");
            ViewBag.PlatformID = new SelectList(db.Platforms, "PlatformID", "PlatformName");
            ViewBag.RegionID = new SelectList(db.Regions, "RegionID", "Region1");
            
            return View();
        }

        // POST: Players/Search
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search([Bind(Include = "PlayerId,GamerName,GameID,GamerStyleID,RegionID,PlatformID")] Player player)
        {
            if (ModelState.IsValid)

            {
                
                search.gameID = player.GameID;
                search.platformid = player.PlatformID;
                search.regionid = player.RegionID;
                search.gamerstyleid = player.GamerStyleID;
                db.Players.Add(player);
                db.SaveChanges();
                return RedirectToAction("Results", search);
            }

            ViewBag.GameID = new SelectList(db.Games, "GameID", "GameName", player.GameID);
            ViewBag.GamerStyleID = new SelectList(db.GamerStyles, "GamerStyleID", "GamerStyle1", player.GamerStyleID);
            ViewBag.PlatformID = new SelectList(db.Platforms, "PlatformID", "PlatformName", player.PlatformID);
            ViewBag.RegionID = new SelectList(db.Regions, "RegionID", "Region1", player.RegionID);
            return View(player);
        }

        // GET: Players/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Player player = db.Players.Find(id);
            if (player == null)
            {
                return HttpNotFound();
            }
            return View(player);
        }

        // POST: Players/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Player player = db.Players.Find(id);
            db.Players.Remove(player);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public void deleteplayer()
        {
            search.gameID = '0';
            search.gamerstyleid = '0';
            search.platformid = '0';
            search.regionid = '0';
            return;
        }
        public ActionResult Success()
        {
            return View();
        }
    }
}
