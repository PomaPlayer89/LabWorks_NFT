namespace nat.Services
{
    public interface ICryptographyService
    {
        public byte[] Encrypt(byte[] data, string algorithm, string password);
        public byte[] Decrypt(byte[] data, string password);
    }
}
