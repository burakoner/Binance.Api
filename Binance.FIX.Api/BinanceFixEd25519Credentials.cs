using System;
using System.Security.Cryptography;
using System.Text;
using NSec.Cryptography;

namespace Binance.FIX.Api;

/// <summary>
/// Owns one Binance Spot FIX API key and its Ed25519 private key.
/// </summary>
/// <remarks>
/// The imported private key is retained by the cryptographic provider rather than as PEM text.
/// Dispose this instance when it is no longer needed.
/// </remarks>
public sealed class BinanceFixEd25519Credentials : IDisposable
{
    private static readonly SignatureAlgorithm Algorithm = SignatureAlgorithm.Ed25519;

    private readonly object gate = new();
    private readonly byte[] apiKeyBytes;
    private Key? privateKey;

    /// <summary>
    /// Imports an unencrypted PKCS#8 Ed25519 private key for one FIX API key.
    /// </summary>
    /// <param name="apiKey">The printable-ASCII Binance API key used as FIX tag 553.</param>
    /// <param name="pkcs8PrivateKeyPem">The unencrypted PKCS#8 Ed25519 private key in PEM form.</param>
    /// <exception cref="ArgumentException">The API key or private key is outside the required format.</exception>
    public BinanceFixEd25519Credentials(string apiKey, ReadOnlySpan<byte> pkcs8PrivateKeyPem)
    {
        if (!IsPrintableAscii(apiKey))
        {
            throw new ArgumentException("FIX API key must be non-empty printable ASCII.", nameof(apiKey));
        }

        if (pkcs8PrivateKeyPem.IsEmpty)
        {
            throw new ArgumentException("An unencrypted PKCS#8 Ed25519 private key is required.", nameof(pkcs8PrivateKeyPem));
        }

        var privateKeyCopy = pkcs8PrivateKeyPem.ToArray();
        try
        {
            privateKey = Key.Import(Algorithm, privateKeyCopy, KeyBlobFormat.PkixPrivateKeyText);
        }
        catch (Exception exception) when (exception is ArgumentException or FormatException or CryptographicException)
        {
            throw new ArgumentException(
                "Private key must be an unencrypted PKCS#8 Ed25519 PEM value.",
                nameof(pkcs8PrivateKeyPem));
        }
        finally
        {
            CryptographicOperations.ZeroMemory(privateKeyCopy);
        }

        apiKeyBytes = Encoding.ASCII.GetBytes(apiKey);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        lock (gate)
        {
            privateKey?.Dispose();
            privateKey = null;
            CryptographicOperations.ZeroMemory(apiKeyBytes);
        }

        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    public override string ToString() => $"{nameof(BinanceFixEd25519Credentials)} ([REDACTED])";

    internal string GetApiKey()
    {
        lock (gate)
        {
            ThrowIfDisposed();
            return Encoding.ASCII.GetString(apiKeyBytes);
        }
    }

    internal string SignBase64(ReadOnlySpan<byte> payload)
    {
        lock (gate)
        {
            ThrowIfDisposed();
            var signature = Algorithm.Sign(privateKey!, payload);
            try
            {
                return Convert.ToBase64String(signature);
            }
            finally
            {
                CryptographicOperations.ZeroMemory(signature);
            }
        }
    }

    private static bool IsPrintableAscii(string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        foreach (var character in value)
        {
            if (character is < ' ' or > '~')
            {
                return false;
            }
        }

        return true;
    }

    private void ThrowIfDisposed()
    {
        if (privateKey is null)
        {
            throw new ObjectDisposedException(nameof(BinanceFixEd25519Credentials));
        }
    }
}
