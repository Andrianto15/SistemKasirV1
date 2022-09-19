using Microsoft.AspNetCore.Mvc;
using SistemKasir.Models;
using SistemKasir.ViewModels;
using System.Diagnostics;

namespace SistemKasir.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        //public IActionResult Privacy()
        //{
        //    return View();
        //}

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}

        //public ActionResult MainView()
        //{
        //    return View(); //this is main page.We will display  "_AddMoreProdukPartialView" partial page on this main page
        //}
        //public ActionResult AddMoreProdukPartialView()
        //{
        //    //this  action page is support cal the partial page.
        //    //We will call this action by view page.This Action is return partial page
        //    AddMoreProdukViewModel model = new AddMoreProdukViewModel();
        //    return PartialView("_AddMoreProdukPartialView", model);
        //    //^this is actual partical page we have 
        //    //create on this page in Home Controller as given below image
        //}
        //public ActionResult PostAddMore(AddMoreProdukViewModel model)
        //{
        //    //Here,Post addmore value from view page and get multiple values from view page
        //    return View();
        //}
    }
}