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
    public class HomeController : Controller
    {
        // GET: /<controller>/
        [HttpGet]
        public IActionResult Index()
        {
            var vm = new AuthorListViewModel();
            using(var pubs = new PubsEntities())
            {
                vm.Authors = pubs.Authors.ToArray();
            }
            return View(vm);
        }
    }
}
