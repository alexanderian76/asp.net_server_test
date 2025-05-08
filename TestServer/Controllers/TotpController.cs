using Microsoft.AspNetCore.Mvc;
using TestServer.Services;
using TestServer.Services.Auth;



namespace TestServer.Controllers
{
	public class TotpController(ILogger<TotpController> _logger, IUserService _userService, ITotpService _totpService) : Controller
	{

		[HttpGet]
		public async Task<IActionResult> Validate(string code, CancellationToken cancellationToken)
		{
			var user = await _userService.GetByIdAsync(1);
			var res = _totpService.Validate(code, user.Data!.TotpKey!);
			return Ok(res);
		}

		[HttpGet]
		public async Task<IActionResult> CreateQrCode(CancellationToken cancellationToken)
		{
			return Ok();
		/*	var user = new User {
				Login = "TotpLogin",
				TotpKey = _totpService.CreateKey("1234"),
				
			};
			await _userService.CreateAsync(user);
			var qrCode = _totpService.GetQrCodeBase64(user);
			return base.Content($"<img src='data:image/png;base64,{qrCode}'/>", "text/html");*/
		}





		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return BadRequest("Error captcha");
		}
	}

}