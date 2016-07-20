using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AspNetCorehandson.Models;
using AspNetCorehandson.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AspNetCorehandson.Controllers
{
    public class Sample03Controller : Controller
    {
        [HttpGet]
        public ActionResult ListAuthors()
        {
            return View();
        }

        [HttpGet]
        public IList<AuthorOverview> GetAuthors()
        {
            using (var pubs = new PubsEntities())
            {
                var query = pubs.Authors.Select(a => new AuthorOverview
                {
                    AuthorId = a.AuthorId,
                    AuthorName = a.AuthorFirstName + " " + a.AuthorLastName,
                    Phone = a.Phone,
                    State = a.State,
                    Contract = a.Contract
                });
                return query.ToArray();
            }
        }

        [HttpGet]
        public IList<string> GetAllStates()
        {
            using (var pubs = new PubsEntities())
            {
                return pubs.Authors.Select(a => a.State).Distinct().ToArray();
            }
        }

        [HttpGet]
        public ActionResult EditAuthor(string id)
        {
            if (Regex.IsMatch(id, @"^[0-9]{3}-[0-9]{2}-[0-9]{4}$") == false) throw new ArgumentOutOfRangeException(nameof(id));
            ViewData["AuthorId"] = id;
            return View(new UpdateAuthorRequest());
        }


        [HttpGet]
        public Author GetAuthorByAuthorId(string authorId)
        {
            if (Regex.IsMatch(authorId, @"^[0-9]{3}-[0-9]{2}-[0-9]{4}$") == false) throw new ArgumentOutOfRangeException(nameof(authorId));

            // 当該著者 ID のデータを読み取る
            using (var pubs = new PubsEntities())
            {
                return pubs.Authors.FirstOrDefault(a => a.AuthorId == authorId);
            }
        }

        [HttpPost]
        public void UpdateAuthor(UpdateAuthorRequest request)
        {
            if (ModelState.IsValid == false) throw new ArgumentException();

            // データベースに登録する
            using (var pubs = new PubsEntities())
            {
                var target = pubs.Authors.FirstOrDefault(a => a.AuthorId == request.AuthorId);
                target.AuthorFirstName = request.AuthorFirstName;
                target.AuthorLastName = request.AuthorLastName;
                target.Phone = request.Phone;
                target.State = request.State;
                pubs.SaveChanges();
            }
        }
    }
}
