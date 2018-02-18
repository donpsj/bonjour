using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bonjour.Models;

namespace bonjour.Controllers
{
    public class HelloController : Controller
    {
        // by using static here the dictionary is initialized at the very first creationg of the 
        // HelloController class. on subsequent creations (when routes are accessed) it will
        // check the memory for the existing Greetings dictionary and not bother creating a new
        // dictionary because it already exists
        private static Dictionary <string, string> Greetings = new Dictionary<string,   string>() {
            {"English", "Hello {0}"},
            {"Spanish", "Hola {0}"},
            {"Turkish", "Merhaba {0}"},
            {"Hindi", "Namaste {0}"},
            {"French", "Salut {0}"},
        };

        // why the hell do we use static here?
            // by creating the static variable it will persist in memory
            // the HelloController class is initialized EVERY TIME a route is accessed
            // if the counter variable is not static then it will be reinitialized every time
        private static int TotalCounter = 0;

        public string CreateMessage(string name, string language) {
            // user chose English --> returns language = "English"
            // Greetings["English"] --> "Hello {0}"
            return string.Format(Greetings[language], name);
        }

        // two approaches to populating a dictionary
            // use the Add() method
            // initialize with values using the { {key, value} } syntax

        // default GET
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost] // form submit POST
        [Route("hello")] // describe route [absolute path]
        public IActionResult Greeting(string name, string language) {
// TODO: WORK ON THE COOKIE BUSINESS
            // if the user does not have a 'counter' property in there cookie - set one
            if (!Request.Cookies.ContainsKey("counter")) {
                Response.Cookies.Append("counter", 1.ToString());
                ViewBag.localCounter = 1;
            } else {
                Request.Values["counter"] = (int.Parse(Request.Cookies["counter"]) + 1).ToString(); 
                ViewBag.localCounter = Request.Cookies["counter"];
            }
            // what is the scope of the ViewBag?
                // each route has its own ViewBag
            ViewBag.greetingMessage = CreateMessage(name, language);
            ViewBag.totalCounter = ++TotalCounter;
            return View();
        }
    }
}
