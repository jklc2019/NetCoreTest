using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using NetCore.MyService;
using NetCore.Web.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.Encodings.Web;

namespace NetCore.Web.Controllers
{
    public class HomeController : Controller
    {
        //private TestService testservice;
        private IAdminUserService adminuserservice;
        private IUserService userservice;
        private IHostingEnvironment hostevn;
        private IMemoryCache _cache;
        private ILogger<HomeController> logger;



        //public HomeController(TestService testservice)
        //{
        //    this.testservice = testservice;

        //}
        public HomeController(IAdminUserService adminuserservice, IUserService userservice, IHostingEnvironment hostevn, IMemoryCache memoryCache, ILogger<HomeController> logger)
        {
            this.adminuserservice = adminuserservice;
            this.userservice = userservice;
            this.hostevn = hostevn;
            _cache = memoryCache;
            this.logger = logger;
        }
        public IActionResult Index()
        {
            //string s = null;
            //s.ToString();//手动产生异常
            //return View();
            logger.LogDebug("调试信息");
            try
            {
                string s = null;
                s.ToString();//手动产生异常
            }
            catch (Exception ex)
            {
                logger.LogError(new EventId(), ex, ex.Message);
            }
            logger.LogInformation("Index Page syas Hello");


            string contentPath = hostevn.ContentRootPath;
            string wwwPath = hostevn.WebRootPath;
            bool IsDevelopment = hostevn.IsDevelopment();

            string appsettingsPath = Path.Combine(contentPath, "appsettings.json");
            string sitePath = Path.Combine(wwwPath, @"js\site.js");

            return Content("IsDevelopment:"+ IsDevelopment +"  "+ contentPath + " " + wwwPath + "  " + "  " + appsettingsPath + "  " + sitePath);


            //return Content(this.adminuserservice.GetPwd("aa") +","+userservice.GetUserCount());
        }
        public IActionResult EncodTest()
        {
            HtmlEncoder encoder = HtmlEncoder.Default;
            string s= encoder.Encode("<p></p>");
            return Content("Encod");
        }

        #region Session
        public IActionResult SessionTest()
        {
            HttpContext.Session.SetString("myuserName", "刘备");
            return Content("Session Set Successful!");
        }
        public IActionResult SessionGet()
        {
            string userName= HttpContext.Session.GetString("myuserName");
            return Content(userName);
        }
        #endregion

        #region Cache
        public IActionResult CacheSet()
        {
            _cache.Set<string>("name", "yzk" + DateTime.Now, TimeSpan.FromSeconds(10));
            return Content("缓存设置成功");
        }
        public IActionResult CacheGet()
        {
            string getcachestring = _cache.Get<string>(CacheKeys.Entry);

            return Content("获取缓存成功:" + getcachestring);
        }
        public IActionResult CacheTryGetValueSet()
        {

            DateTime cacheEntry;

            // Look for cache key.
            if (!_cache.TryGetValue(CacheKeys.Entry, out cacheEntry))
            {
                // Key not in cache, so get data.
                cacheEntry = DateTime.Now;

                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // Keep in cache for this time, reset time if accessed.
                    .SetSlidingExpiration(TimeSpan.FromSeconds(3));

                // Save data in cache.
                _cache.Set(CacheKeys.Entry, cacheEntry, cacheEntryOptions);
            }

            return Content("Cache:" + cacheEntry);
        } 
        #endregion
        public IActionResult About()
        {
            //ViewData["Message"] = "Your application description page.";
            IAdminUserService service = (IAdminUserService)HttpContext.RequestServices.GetService(typeof(IAdminUserService));
            service.GetPwd("aa");
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public static class CacheKeys
    {
        public static string Entry { get { return "_Entry"; } }
        public static string CallbackEntry { get { return "_Callback"; } }
        public static string CallbackMessage { get { return "_CallbackMessage"; } }
        public static string Parent { get { return "_Parent"; } }
        public static string Child { get { return "_Child"; } }
        public static string DependentMessage { get { return "_DependentMessage"; } }
        public static string DependentCTS { get { return "_DependentCTS"; } }
        public static string Ticks { get { return "_Ticks"; } }
        public static string CancelMsg { get { return "_CancelMsg"; } }
        public static string CancelTokenSource { get { return "_CancelTokenSource"; } }
    }
}
