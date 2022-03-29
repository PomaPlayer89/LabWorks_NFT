namespace nat.Services
{
    public interface IAlgorithmsCryptography
    {
        public byte[] Encrypt(byte[] data, string password);
        public byte[] Decrypt(byte[] data, string password);
    }
}
