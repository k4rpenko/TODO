namespace Core.Hash
{
    public interface IArgon2Hasher
    {
        string Hash(string password, string key);
        string GenerateKey();
    }
}