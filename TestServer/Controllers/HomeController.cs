using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TestServer.Models;
using Microsoft.EntityFrameworkCore;
using iTextSharp.text.pdf;
using iTextSharp;
using iTextSharp.text;
using System.Net.WebSockets;
using Org.BouncyCastle.Asn1.Ocsp;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System;
using System.Net.Sockets;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static iTextSharp.text.pdf.AcroFields;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TestServer.Services.Auth;
using TestServer.Services;

namespace TestServer.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IPhoneService _service;
    private readonly ICompanyService _serviceCompany;
    private readonly IChatService _chatService;
    private readonly MobileContext _dbContext;
    private readonly ITotpService _totpService;
    private readonly IUserService _userService;

    public HomeController(ILogger<HomeController> logger,
        IPhoneService service,
        IChatService chatService,
        ICompanyService companyService,
        MobileContext dbContext,
        ITotpService totpService,
        IUserService userService)
    {
        _logger = logger;
        _service = service;
        _chatService = chatService;
        _serviceCompany = companyService;
        _dbContext = dbContext;
        _totpService = totpService;
        _userService = userService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }


    [Authorize]
    [Route("/ws")]
    public async System.Threading.Tasks.Task GetSocket(string login)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Login == login);
        if (user != null)
            await _chatService.WebSocketRequest(user, HttpContext);
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
                    var example_html = @"<p>
This <em>is </em><span class=""headline"" style=""text-decoration: underline;"">some</span> <strong>sample <em> text</em></strong><span style=""color: red;"">!!!</span>
<table style=""border: 1px;"">
	<tr style=""border: 1px;""><th>1 column</th><th>2 column</th><th>3 column</th></tr>
	<tr style=""border: 1px;""><td>1</td><td>2</td><td>3</td></tr>
</table>
</p>";
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
        //System.IO.File.WriteAllBytes(testFile, bytes);
        //var file = System.IO.File.Open(testFile, FileMode.Open);
        var stream = new MemoryStream(bytes);

        return Ok(stream);
    }


    [Authorize]
    [Route("/users")]
    [HttpGet]
    public async Task<IActionResult> GetUsersOnline()
    {
        var data = await _chatService.GetUsers();
        foreach (var u in data.Data)
        {
            Console.WriteLine(u.Login);
        }

        return Ok(data.Data);
    }

    [Route("login")]
    [HttpGet]
    public async Task<IActionResult> Login(string userName, string? totpCode)
    {
        var claims = new List<Claim> { new Claim(ClaimTypes.Name, userName) };
        var data = await _chatService.GetUsers();
        var user = _dbContext.Users.FirstOrDefault(u => u.Login == userName);
        if (user != null)
        {
            if (_totpService.Validate(totpCode, user.TotpKey))
            {
                var jwt = new JwtSecurityToken(
                                    issuer: AuthOptions.ISSUER,
                            audience: AuthOptions.AUDIENCE,
                            claims: claims,
                                    expires: DateTime.UtcNow.Add(TimeSpan.FromDays(365)),
                                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

                return Ok(new JwtSecurityTokenHandler().WriteToken(jwt));
            }
            else
            {
                return BadRequest("Wrong totp code");
            }
        }
        else
        {
            var totpKey = _totpService.CreateKey();
            var newUser = new User() { Login = userName, TotpKey = totpKey };
            await _userService.CreateAsync(newUser);
            var qrCode = _totpService.GetQrCodeBase64(newUser);

            var jwt = new JwtSecurityToken(
                               issuer: AuthOptions.ISSUER,
                       audience: AuthOptions.AUDIENCE,
                       claims: claims,
                               //expires: DateTime.UtcNow.Add(TimeSpan.FromDays(2)),
                               signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));


            return Ok(new { code = qrCode, token = new JwtSecurityTokenHandler().WriteToken(jwt) });
            if (data.Data.Where(u => u.Login == userName).Count() > 0)
            {
                // return new BadRequestObjectResult("User exists");
            }
            // создаем JWT-токен
           /* var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            return Ok(new JwtSecurityTokenHandler().WriteToken(jwt));*/
        }
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
        //response.Data.Company = await _service.GetById
        return Ok(response);

    }

    [HttpPost]
    public async Task<IActionResult> CreateRandomPhone(string title)
    {
        var rand = new Random().Next(100);

        Company company = _serviceCompany.GetById(1).Result.Data;

        var phone = new Phone() { Company = company == null ? new Company() { Name = "testComp" + title } : company, Title = title, Price = rand };

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

