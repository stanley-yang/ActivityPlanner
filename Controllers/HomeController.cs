using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ActivityCenter.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace ActivityCenter.Controllers
{
    public class HomeController : Controller
    {
        private ActivityContext dbContext;
        public HomeController(ActivityContext context)
        {
            dbContext = context;
        }

        [Route("")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Route("login")]
        [HttpGet]
        public IActionResult login()
        {
            System.Console.WriteLine("Login Route");
            return View("index");
        }

        [HttpPost("logging")]
        public IActionResult logging(LoginUser userSub)
        {
            if(ModelState.IsValid)
            {
                var userInDb = dbContext.Users.FirstOrDefault(u => u.Email == userSub.logEmail);
                if (userInDb == null)
                {
                    ModelState.AddModelError("logEmail","Invalid Email/Password");
                    return View("index");
                }
                var hasher = new PasswordHasher<LoginUser>();
                var result = hasher.VerifyHashedPassword(userSub, userInDb.Password, userSub.logPassword);
                if (result ==0)
                {
                    ModelState.AddModelError("logEmail","Invalid Email/Password");
                    return View("index");
                }
                HttpContext.Session.SetInt32("User",userInDb.UserId);
                HttpContext.Session.SetString("Name",userInDb.Name);
                
                // Deletes Events in the past
                List<ActivityCenter.Models.Activity> todayremove = dbContext.Activities.Where(date => date.Datetime == System.DateTime.Now && date.time <= DateTime.Now.TimeOfDay).ToList();
                List<ActivityCenter.Models.Activity> activityToRemove = dbContext.Activities.Where(date => date.Datetime <= System.DateTime.Now).ToList();
                dbContext.Activities.RemoveRange(activityToRemove);
                dbContext.Activities.RemoveRange(todayremove);
                dbContext.SaveChanges();
                return RedirectToAction("home");
            }
            else
            {
                System.Console.WriteLine("Return to Login");
                return View("index");
            }

        }
       

        [HttpPost("create")]
        public IActionResult Create(User user)
        {
            if(ModelState.IsValid)
            {
                bool accept = true;
                var exists = dbContext.Users.FirstOrDefault(e => e.Email == user.Email);
                if(exists != null)
                {
                    System.Console.WriteLine("Email Exists");
                    ModelState.AddModelError("Email","Email already exists");
                    accept = false;
                }
                if(user.Password != user.Confirm)
                    {
                        ModelState.AddModelError("Confirm","Passwords do not match");
                        accept = false;
                    }

                if (accept == false)
                {
                    System.Console.WriteLine("Bad Password");
                    return View("index");
                }
                
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                user.Password = Hasher.HashPassword(user, user.Password);                
                dbContext.Add(user);
                dbContext.SaveChanges();
                HttpContext.Session.SetInt32("User",user.UserId);
                System.Console.WriteLine(user.UserId);
                HttpContext.Session.SetString("Name",user.Name);
                
                
                return RedirectToAction("home");
            }
            else
            {
                System.Console.WriteLine("Return to Create");
                return View("index");
            }
        }

        [HttpPost("postactivity")]
        public IActionResult postactivity(ActivityCenter.Models.Activity activity)
        {
            activity.UserId = HttpContext.Session.GetInt32("User").GetValueOrDefault();
            activity.Planner = dbContext.Users.FirstOrDefault(user => user.UserId == HttpContext.Session.GetInt32("User"));
            if(ModelState.IsValid)
            {
                DateTime combined = activity.Datetime.Add(activity.time);
                System.Console.WriteLine(combined);
                if (combined < DateTime.Now)
                {
                    ModelState.AddModelError("Datetime", "Date and Time must be in the future");
                    return View("addactivity");
                }
                
                
                dbContext.Add(activity);
                dbContext.SaveChanges();

                ViewBag.UserId = HttpContext.Session.GetInt32("User");
                
                Rsvp thisrsvp = new Rsvp
            {
                UserId = ViewBag.UserId,
                ActivityId = activity.ActivityId
            };
                dbContext.Add(thisrsvp);
                dbContext.SaveChanges();
                return RedirectToAction("home");
            }
            else
            {
                System.Console.WriteLine("Return to Create");
                return View("addactivity");
            }
        }

        [HttpGet("addactivity")]
        public IActionResult addactivity()
        {
            return View();
        }

        [HttpGet("home")]
        public IActionResult home()
        {
            if(HttpContext.Session.GetInt32("User") == null)
            {
                System.Console.WriteLine("Not Logged In");
                ModelState.AddModelError("Email","Not Logged In");
                return View("index");
            }
            else 
            {
                System.Console.WriteLine("User Logged In");
                ViewBag.Name = HttpContext.Session.GetString("Name");
                ViewBag.UserId = HttpContext.Session.GetInt32("User");
                System.Console.WriteLine(ViewBag.Name);
                List<ActivityCenter.Models.Activity> acts = dbContext.Activities
                .Include(n => n.Planner)
                .Include(w => w.Rsvps)
                .ToList();
                return View(acts);
            }
        }

        [HttpGet("logout")]
        public IActionResult logout()
        {
            HttpContext.Session.Clear();
            
            
            return RedirectToAction("index");
        }

        [HttpGet("delete/{activityId}")]
        public IActionResult delete(int activityId)
        {
            
            ActivityCenter.Models.Activity thisactivity = dbContext.Activities.SingleOrDefault(act => act.ActivityId == activityId);
            

             if(HttpContext.Session.GetInt32("User") == thisactivity.UserId)
            {
                dbContext.Activities.Remove(thisactivity);
            dbContext.SaveChanges();
            }
            
            
            return RedirectToAction("home");
        }

        [HttpGet("rsvp/{activityId}")]
        public IActionResult rsvp(int activityId)
        {
            int id = (HttpContext.Session.GetInt32("User")).GetValueOrDefault();
            Rsvp thisrsvp = new Rsvp
            {
                UserId = id,
                ActivityId = activityId
            };
        
            dbContext.Add(thisrsvp);
            dbContext.SaveChanges();
            return RedirectToAction("home");
        }

        [HttpGet("rsvpinside/{activityId}")]
        public IActionResult rsvpinside(int activityId)
        {
            int id = (HttpContext.Session.GetInt32("User")).GetValueOrDefault();
            Rsvp thisrsvp = new Rsvp
            {
                UserId = id,
                ActivityId = activityId
            };
            int newId = activityId;
            dbContext.Add(thisrsvp);
            dbContext.SaveChanges();
            return RedirectToAction("home");
        }

        [HttpGet("leave/{activityId}")]
        public IActionResult rsvplist(int activityId)
        {
            int id = (HttpContext.Session.GetInt32("User")).GetValueOrDefault();
            Rsvp thisrsvp = dbContext.Rsvps.SingleOrDefault(rsvp => rsvp.ActivityId == activityId && rsvp.UserId == id);
            dbContext.Rsvps.Remove(thisrsvp);
            dbContext.SaveChanges();
            return RedirectToAction("home");
        }

        [HttpGet("activity/{activityId}")]
        public IActionResult activityinfo(int activityId)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("User");
            ActivityCenter.Models.Activity thisactivity = dbContext.Activities.Include(act => act.Rsvps)
            .ThenInclude(u => u.user).Include(n => n.Planner).SingleOrDefault(a => a.ActivityId == activityId);
            ViewBag.Activity = thisactivity;
            return View("showactivity");
        }

 
    }
}
