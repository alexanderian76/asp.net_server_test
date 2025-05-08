using System.Text;
using Microsoft.IdentityModel.Tokens;
using OtpNet;
using QRCoder;
using TestServer.Attributes;
using TestServer.Extensions;
using TestServer.Services.Captcha;

namespace TestServer.Services.Auth
{
    public interface ITotpService
    {
        public bool Validate(string code, string key);
        public string CreateKey();
        public Task GenerateNewTotpKeyAsync(User user, CancellationToken cancellationToken);
        public string GetQrCodeBase64(User login);
    }

    [DITransient]
    public class TotpService(IGenerateStringService _generateStringService,
        MobileContext _dbContext)
        : ITotpService
    {
        public string CreateKey()
        {
            var secretKey = _generateStringService.GenerateString();
            var base32String = Base32Encoding.ToString(Encoding.ASCII.GetBytes(secretKey));
            return base32String;
        }

        public async Task GenerateNewTotpKeyAsync(User user, CancellationToken cancellationToken)
        {
            var secretKey = _generateStringService.GenerateString();
            var base32String = Base32Encoding.ToString(Encoding.ASCII.GetBytes(secretKey));
            user.TotpKey = base32String;
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public string GetQrCodeBase64(User user)
        {
            if (user.TotpKey.IsNullOrEmpty())
                return "";
            var url = new OtpUri(OtpType.Totp, user.TotpKey, user.Login);
            MemoryStream stream;
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(url.ToString(), QRCodeGenerator.ECCLevel.Q))
                {
                    using (PngByteQRCode qrCode = new PngByteQRCode(qrCodeData))
                    {
                        byte[] qrCodeImage = qrCode.GetGraphic(20);
                        stream = new MemoryStream(qrCodeImage);
                    }
                }
            }
            return stream.ToBase64String();
        }


        public bool Validate(string code, string key)
        {
            var base32Bytes = Base32Encoding.ToBytes(key);
            var generator = new Totp(base32Bytes, mode: OtpHashMode.Sha1);
            long timeStep;
            var isValid = generator.VerifyTotp(code, out timeStep);
            return isValid;
        }
    }
}
