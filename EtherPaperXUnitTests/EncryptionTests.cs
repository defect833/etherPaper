using System;
using Xunit;
using EtherPaper;

namespace EtherPaperXUnitTests
{
    public class EncryptionTests
    {
        [Theory]
        [InlineData("E546C8DF278CD5931069B522E695D4F2", "2272dfd6afe8af62a8ab7d15ae9045536ce659552deea107c561b2ba415a1687", "Example Phrase")]
        public void EncryptionReliability(string key, string salt, string testPhrase)
        {
            // Arrange

            // Act

            // Assert
        }
    }
}
