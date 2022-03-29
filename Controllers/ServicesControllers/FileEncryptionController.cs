using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using nat.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace nat.Controllers
{
    public class FileEncryptionController : Controller
    {
        private readonly ICryptographyService _cryptographyService;
        private readonly ISettingEDSFileService _settingEDSFileService;
        public FileEncryptionController(ICryptographyService cryptographyService, ISettingEDSFileService settingEDSFileService)
        {
            _cryptographyService = cryptographyService;
            _settingEDSFileService = settingEDSFileService;
        }

        [HttpGet]
        public IActionResult FileEncryption()
        {
            return View();
        }

        #region Encrypt/DeDecrypt File

        [HttpPost]
        public async Task<IActionResult> EncryptFile(IFormFile uploadedFile, string algorithm, string password)
        {
            if (CheckValidEncrypt(uploadedFile, password, algorithm))
            {
                return View(nameof(FileEncryption));
            }

            try
            {
                string fileName = uploadedFile!.FileName;
                string ext = Path.GetExtension(fileName);
                string fileType = "application/" + ext.Trim('.');
                fileName = "EncryptFile" + ext;

                byte[] data = new byte[uploadedFile.Length];
                using (var stream = uploadedFile.OpenReadStream())
                {
                    await stream.ReadAsync(data);
                }
                
                var encrypt = await Task.Run(() => _cryptographyService.Encrypt(data, algorithm!, password));
                if (encrypt.Length < data.Length)
                {
                    return View(nameof(FileEncryption));
                }

                return File(encrypt, fileType, fileName);
            }
            catch
            {
                return View(nameof(FileEncryption));
            }
        }

        [HttpPost]
        public async Task<IActionResult> DecryptFile(IFormFile uploadedFile, string password)
        {
            if (CheckValidEncrypt(uploadedFile, password, ""))
            {
                return View(nameof(FileEncryption));
            }

            try
            {
                string fileName = uploadedFile!.FileName;
                string ext = Path.GetExtension(fileName);
                string fileType = "application/" + ext.Trim('.');
                fileName = "DecryptFile" + ext;

                byte[] data = new byte[uploadedFile.Length];
                using (var stream = uploadedFile.OpenReadStream())
                {
                    await stream.ReadAsync(data);
                }

                var encrypt = await Task.Run(() => _cryptographyService.Decrypt(data, password));
                if(encrypt.Length == 0)
                {
                    return View(nameof(FileEncryption));
                }

                return File(encrypt, fileType, fileName);
            }
            catch
            {
                return View(nameof(FileEncryption));
            }
        }

        private bool CheckValidEncrypt(IFormFile uploadedFile, string password, string algorithm)
        {
            bool error = false;

            if (uploadedFile == null)
            {
                error = true;
            }

            if (algorithm == null)
            {
                error = true;
            }

            if (string.IsNullOrEmpty(password))
            {
                error = true;
            }

            if (!ModelState.IsValid)
            {
                error = true;
            }

            return error;
        }

        #endregion

        #region SignFiles 

        public IActionResult DigitalSignature()
        {
            return View();
        }

        public async Task<IActionResult> CreateСertificate(int months = 3)
        {
            if(!ModelState.IsValid)
            {
                return View(nameof(DigitalSignature));
            }

            var archive = await _settingEDSFileService.CreateСertificateAsync(months);
            if(archive == null)
            {
                return View(nameof(DigitalSignature));
            }

            return File(archive.File, $"application/{archive.Extension}", archive.FileName);
        }

        public async Task<IActionResult> SignFile(IFormFile fileForSign, IFormFile privateKey, string passwordForSign)
        {
            var operation = CheckValidSign(fileForSign, privateKey, passwordForSign);
            if (!operation)
            {
                return View(nameof(DigitalSignature));
            }

            var signFile = await _settingEDSFileService.SignFileAsync(fileForSign, privateKey, passwordForSign);
            if(signFile == null)
            {
                return View(nameof(DigitalSignature));
            }

            return File(signFile.File, $"application/{signFile.Extension}", signFile.FileName);
        }

        public async Task<IActionResult> CheckSignFile(IFormFile fileForCheck, IFormFile publicKey, string passwordForCheck)
        {
            var operation = CheckValidSign(fileForCheck, publicKey, passwordForCheck);
            if (!operation)
            {
                return View(nameof(DigitalSignature));
            }

            var checkSign = await _settingEDSFileService.CheckSignFileAsync(fileForCheck, publicKey, passwordForCheck);

            return View(nameof(DigitalSignature), operation);
        }

        public async Task<IActionResult> GetOriginalFile(IFormFile fileForDelete)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(DigitalSignature));
            }

            if (fileForDelete == null)
            {
                return View(nameof(DigitalSignature));
            }

            var originFile = await _settingEDSFileService.GetOriginalFileAsync(fileForDelete);
            if (originFile == null)
            {
                return View(nameof(DigitalSignature));
            }

            return File(originFile.File, $"application/{originFile.Extension}", originFile.FileName);
        }

        private bool CheckValidSign(IFormFile file, IFormFile key, string password)
        {
            bool Result = true;

            if (!ModelState.IsValid)
            {
                Result = false;
            }

            if (file == null)
            {
                Result = false;
            }

            if (key == null)
            {
                Result = false;
            }

            if (String.IsNullOrEmpty(password))
            {
                Result = false;
            }

            return Result;
        }

        #endregion
    }
}
