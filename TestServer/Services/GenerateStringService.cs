using System.Text;
using TestServer.Attributes;


namespace TestServer.Services.Captcha
{
    public interface IGenerateStringService
    {
        public string GenerateString();
    }
    [DITransient]
    internal class GenerateStringService : IGenerateStringService
    {
        private const string Alpabet = "q0wer1ty2u3iop4asd5f6g7hjk8lzxcv9bnm";

        public string GenerateString()
        {
            var outStr = new StringBuilder();

            var rnd = new Random(DateTime.Now.Millisecond);

            var lenAlph = Alpabet.Length;

            var length = rnd.Next(5, 7);

            for (var i = 0; i < length; ++i)
            {
                outStr.Append(Alpabet[rnd.Next(lenAlph)]);
            }

            return outStr.ToString();
        }

    }
}