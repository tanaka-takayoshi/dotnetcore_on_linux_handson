using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCorehandson.Models;
using AspNetCorehandson.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AspNetCorehandson.Controllers
{
    public class Sample02Controller : Controller
    {
        [HttpGet]
        public ActionResult ListAuthors()
        {
            using (var pubs = new PubsEntities())
            {
                var query = pubs.Authors
                            .Select(a => new AuthorOverview
                            {
                                AuthorId = a.AuthorId,
                                AuthorName = a.AuthorFirstName + " " + a.AuthorLastName,
                                Phone = a.Phone,
                                State = a.State,
                                Contract = a.Contract
                            });
                var vm = new AuthorOverviewListViewModel
                {
                    Authors = query.ToArray()
                };
                return View(vm);
            }
        }

        [HttpGet]
        public ActionResult EditAuthor(string id)
        {
            // 当該著者 ID のデータを読み取る
            Author editAuthor = null;
            using (var pubs = new PubsEntities())
            {
                editAuthor = pubs.Authors.FirstOrDefault(a => a.AuthorId == id);
            }

            // View に引き渡すデータを準備する
            var vm = new AuthorEditViewModel()
            {
                AuthorId = editAuthor.AuthorId,
                AuthorFirstName = editAuthor.AuthorFirstName,
                AuthorLastName = editAuthor.AuthorLastName,
                Phone = editAuthor.Phone,
                State = editAuthor.State
            };

            // View にデータを引き渡すにあたり、入力データと周辺データを分けておく。
            // (ViewModel に周辺データを入れることで、ViewModel を完全にフォームモデルに一致させるように設計)
            using (var pubs = new PubsEntities())
            {
                var query = pubs.Authors.Select(a => a.State).Distinct();
                ViewData["AllStates"] = query.ToList();
            }

            return View(vm); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAuthor(string id, AuthorEditViewModel model)
        {
            using (var pubs = new PubsEntities())
            {
                var query = pubs.Authors.Select(a => a.State).Distinct();
                ViewData["AllStates"] = query.ToList();
            }

            // 送信されてきたデータを再チェック
            if (ModelState.IsValid == false)
            {
                // 前画面を返す
                // ID フィールドがロストしているので補完する
                model.AuthorId = id;
                return View(model);
            }

            model.AuthorId = id;

            // データベースに登録を試みる
            using (var pubs = new PubsEntities())
            {
                var target = pubs.Authors.Where(a => a.AuthorId == model.AuthorId).FirstOrDefault();
                target.AuthorFirstName = model.AuthorFirstName;
                target.AuthorLastName = model.AuthorLastName;
                target.Phone = model.Phone;
                target.State = model.State;

                pubs.SaveChanges();
            }
            // 一覧画面に帰る
            return RedirectToAction("ListAuthors");
        }
    }
}
