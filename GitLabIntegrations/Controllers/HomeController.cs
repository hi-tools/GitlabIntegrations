using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TestGitLabHook.Models;

namespace TestGitLabHook.Controllers
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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> GetBody()
        {
            string body = "";
            using (StreamReader stream = new StreamReader(Request.Body))
            {
                body = await stream.ReadToEndAsync();
            }
            return Json(body);
        }
        public async Task<IActionResult> gitlab(int id)
        {
            var t = Request.Headers.FirstOrDefault(a => a.Key.ToLower().Equals("x-gitlab-token")).Value;
            string body = "0";
            try
            {
                using (StreamReader stream = new StreamReader(Request.Body))
                {
                    body = await stream.ReadToEndAsync();
                }
            }
            catch (Exception ex)
            {
                body = ex.Message;
            }

            string tt = "not exists " + id.ToString();
            if (t.ToString().Length > 0)
            {
                tt = t.ToString();
            }
            //if (t != null)
            //{
            //    string tt = t.Value ?? "not exists";

            //}
            //else
            //{

            //}
            int res = -1;
            var cnn = new System.Data.SqlClient.SqlConnection("Data source = vms; initial catalog = Erp_ws; user =mis ; password = mis");
            using (cnn)
            {
                cnn.Open();
                var cmd = new SqlCommand("INSERT INTO PROPERTIES(ID, PROPERTY_TYPE,TITLE, DESCRIPTION)VALUES((SELECT MAX(ID) + 1 FROM PROPERTIES), 32, '" + tt + "','" + body.Replace("'", "") + "')", cnn);
                res = cmd.ExecuteNonQuery();
            }
            return Json(res);
        }
    }
}
