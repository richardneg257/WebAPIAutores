using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using WebAPIAutores.Dtos;

namespace WebAPIAutores.Services;

public class HashService
{
    public HashResultDto Hash(string planeText)
    {
        var salt = new byte[16];
        using(var random = RandomNumberGenerator.Create())
        {
            random.GetBytes(salt);
        }

        return Hash(planeText, salt);
    }

    public HashResultDto Hash(string planeText, byte[] salt)
    {
        var derivedKey = KeyDerivation.Pbkdf2(password: planeText,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 10000,
            numBytesRequested: 32);

        var hash = Convert.ToBase64String(derivedKey);

        return new HashResultDto()
        {
            Hash = hash,
            Salt = salt
        };
    }
}
