using System.Text;

namespace Backend.WebApi.Configuration;

public class JwtSettings
{
    public string Key { get; }
    public byte[] KeyBytes { get; }

    public JwtSettings(string key)
    {
        Key = key;
        KeyBytes = Encoding.ASCII.GetBytes(key);
    }
}
