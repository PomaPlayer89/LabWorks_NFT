using Microsoft.AspNetCore.Http;
using nat.Storage.Entity;
using System.Threading.Tasks;

namespace nat.Services
{
    public interface ISettingEDSFileService
    {
        public Task<MyFileInfo> CreateСertificateAsync(int months);
        public Task<MyFileInfo> SignFileAsync(IFormFile uploadedFile, IFormFile privateKey, string password);
        public Task<MyFileInfo> GetOriginalFileAsync(IFormFile uploadedFile);
        public Task<bool> CheckSignFileAsync(IFormFile uploadedFile, IFormFile publicKey, string password);
    }
}
