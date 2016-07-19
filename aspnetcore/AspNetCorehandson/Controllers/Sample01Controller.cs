using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AspNetCorehandson.ApiModels;
using AspNetCorehandson.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AspNetCorehandson.Controllers
{
    public class Sample01Controller : Controller
    {
        // GET: /<controller>/ShowAllAuthors
        [HttpGet]
        public IActionResult ShowAllAuthors()
        {
            return View();
        }

        // GET: /<controller>/ShowAuthorsByState
        [HttpGet]
        public IActionResult ShowAuthorsByState()
        {
            return View();
        }

        [HttpGet]
        public IList<AuthorOverview> GetAllAuthors()
        {
            using (var pubs = new PubsEntities())
            {
                return pubs.Authors
                    .Select(a => new AuthorOverview
                    {
                        AuthorId = a.AuthorId,
                        AuthorName = a.AuthorFirstName + " " + a.AuthorLastName,
                        Phone = a.Phone,
                        State = a.State,
                        Contract = a.Contract
                    })
                    .ToArray();
            }
        }

        [HttpGet]
        public string[] GetStates()
        {
            using (var pubs = new PubsEntities())
            {
                var query = pubs.Authors.Select(a => a.State).Distinct();
                return query.ToArray();
            }
        }

        [HttpGet]
        public IList<AuthorOverview> GetAuthorsByState(string state)
        {
            if (Regex.IsMatch(state, "^[A-Z]{2}$") == false) throw new ArgumentOutOfRangeException(nameof(state));

            using (var pubs = new PubsEntities())
            {
                var query = pubs.Authors.Where(a => a.State == state)
                            .Select(a => new AuthorOverview()
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
    }
}
