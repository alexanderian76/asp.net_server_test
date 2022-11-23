using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TestServer.Models;
using Microsoft.EntityFrameworkCore;

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

