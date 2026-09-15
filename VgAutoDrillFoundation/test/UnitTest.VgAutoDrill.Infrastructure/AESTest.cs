using VgAutoDrill.Fundation.Utils;
using VgAutoDrill.Infrastructure;

namespace UnitTest.VgAutoDrill.Infrastructure;

public class AESTest
{
    [Fact]
    public void EncryptDecryptTest()
    {
        string original = "Here is some data to encrypt!";
        string encryped = AES.Encrypt(original, "a".Repeat(32));
        Assert.NotEmpty(encryped);

        string decrypted = AES.Decrypt(encryped, "a".Repeat(32));
        Assert.Equal(original, decrypted);
    }
}
