using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebHostLab4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : Controller
    {
        private readonly IWebHostEnvironment _appEnvironment;
        public FileController(IWebHostEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }

        [HttpGet("{filename}")]
        public IActionResult GetFile(string filename)
        {
            string file_name = filename + ".txt";
            string file_path = Path.Combine(_appEnvironment.ContentRootPath, "File/" + file_name);
            string file_type = "application/pdf";


            return PhysicalFile(file_path, file_type, file_name);
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            string file_path = Path.Combine(_appEnvironment.ContentRootPath, "File/");
              string[] files = Directory.GetFiles(file_path);
            
            for (int i = 0; i < files.Length; i++)
            {
                files[i] = Regex.Match(files[i], @"[^/]*$").Value;
            }

            return files;
        }   
    }
}
