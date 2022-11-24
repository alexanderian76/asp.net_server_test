using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TestServer.Models;
using Microsoft.EntityFrameworkCore;
using iTextSharp.text.pdf;
using iTextSharp;
using iTextSharp.text;

namespace TestServer.Controllers;

public class HomeController : Controller
{
	private readonly ILogger<HomeController> _logger;
	private readonly IPhoneService _service;

	public HomeController(ILogger<HomeController> logger, IPhoneService service)
	{
		_logger = logger;
		_service = service;
	}

	public IActionResult Index()
	{
		return View();
	}

	public IActionResult Privacy()
	{
		return View();
	}

	public async Task<IActionResult> GetPdfFromHtml()
	{
        Byte[] bytes;

        //Boilerplate iTextSharp setup here
        //Create a stream that we can write to, in this case a MemoryStream
        using (var ms = new MemoryStream())
        {

            //Create an iTextSharp Document which is an abstraction of a PDF but **NOT** a PDF
            using (var doc = new Document())
            {

                //Create a writer that's bound to our PDF abstraction and our stream
                using (var writer = PdfWriter.GetInstance(doc, ms))
                {

                    //Open the document for writing
                    doc.Open();

                    //Our sample HTML and CSS
                    var example_html = @"<p>This <em>is </em><span class=""headline"" style=""text-decoration: underline;"">some</span> <strong>sample <em> text</em></strong><span style=""color: red;"">!!!</span></p>";
                    var example_css = @".headline{font-size:200%}";

                    /**************************************************
                     * Example #1                                     *
                     *                                                *
                     * Use the built-in HTMLWorker to parse the HTML. *
                     * Only inline CSS is supported.                  *
                     * ************************************************/

                    //Create a new HTMLWorker bound to our document
                    using (var htmlWorker = new iTextSharp.text.html.simpleparser.HTMLWorker(doc))
                    {

                        //HTMLWorker doesn't read a string directly but instead needs a TextReader (which StringReader subclasses)
                        using (var sr = new StringReader(example_html))
                        {

                            //Parse the HTML
                            htmlWorker.Parse(sr);
                        }
                    }
                    doc.Close();
                }
            }

            //After all of the PDF "stuff" above is done and closed but **before** we
            //close the MemoryStream, grab all of the active bytes from the stream
            bytes = ms.ToArray();
        }

		//Now we just need to do something with those bytes.
		//Here I'm writing them to disk but if you were in ASP.Net you might Response.BinaryWrite() them.
		//You could also write the bytes to a database in a varbinary() column (but please don't) or you
		//could pass them to another function for further PDF processing.
		var testFile = "text.pdf";
			//Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "test.pdf");
        System.IO.File.WriteAllBytes(testFile, bytes);
		var file = System.IO.File.Open(testFile, FileMode.Open);
		return Ok(file) ;
    }

	public async Task<IActionResult> Get()
	{
		var str = "";
		using (MobileContext db = new MobileContext())
		{
			str = db.Phones.First().Title;
		}
		return Ok(str);
	}


	[HttpGet]
	public async Task<IActionResult> GetPhoneById(int id)
	{
		var response = await _service.GetById(id);
		return Ok(response);
		
	}

	[HttpPost]
	public async Task<IActionResult> CreateRandomPhone(string title)
	{
		var rand = new Random().Next(100);
		var phone = new Phone() { Company = new Company() { Name = "testComp" + rand.ToString() }, Title = title, Price = rand};

        var response = await _service.Create(phone);
		return Ok(response);
	}

	public string GetString()
	{
		using (MobileContext db = new MobileContext())
		{
			db.Add(new Phone() { Company = new Company() { Name = "testComp1" }, Title = "TestTitle" });
			db.SaveChanges();
		}
		return "Hello World!";
	}

	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error()
	{
		return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}
}

