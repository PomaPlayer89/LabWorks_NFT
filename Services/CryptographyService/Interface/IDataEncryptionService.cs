namespace nat.Services
{
    public interface IDataEncryptionService
    {
        bool CheckPassword(string hashPassword);
        string HashPassword(string password);
        string Encrypt_Aes(string plainText);
        string Decrypt_Aes(string _cipherText, string hashpassword);
    }
}
