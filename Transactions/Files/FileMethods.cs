using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Transactions.Files{
    public static class FileMethods{
        public async static Task<string> GetFilePath(IFormFile file){
            var filePath = Path.GetTempFileName();

            var stream = System.IO.File.Create(filePath);

            await file.CopyToAsync(stream);

            stream.Close();

            return filePath;
        }
    }
}