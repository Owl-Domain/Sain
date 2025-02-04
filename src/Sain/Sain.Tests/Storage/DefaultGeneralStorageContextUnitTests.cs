namespace Sain.Tests.Storage;

[TestClass]
public sealed class DefaultGeneralStorageContextUnitTests
{
   #region Tests
   [DataRow("file.txt", ".txt", DisplayName = "Regular extension")]
   [DataRow("file", "", DisplayName = "No extension")]
   [DataRow("file.txt.log", ".log", DisplayName = "Multipart extension")]
   [DataRow(".gitignore", ".gitignore", DisplayName = "Dot file")]
   [DataRow("file..txt", ".txt", DisplayName = "Multiple dots")]
   [DataRow("file.", "", DisplayName = "Ended in dot")]
   [TestMethod]
   public void GetExtension_SinglePart_ExpectedResult(string input, string expectedResult)
   {
      // Arrange
      DefaultGeneralStorageContextUnit sut = new(null);

      // Act
      ReadOnlySpan<char> span = sut.GetExtension(input, false);
      string result = span.ToString();

      // Assert
      Assert.That.AreEqual(result, expectedResult);
   }

   [DataRow("file.txt", ".txt", DisplayName = "Regular extension")]
   [DataRow("file", "", DisplayName = "No extension")]
   [DataRow("file.txt.log", ".txt.log", DisplayName = "Multipart extension")]
   [DataRow(".gitignore", ".gitignore", DisplayName = "Dot file")]
   [DataRow("file..txt", "..txt", DisplayName = "Multiple dots")]
   [DataRow("file.", "", DisplayName = "Ended in dot")]
   [TestMethod]
   public void GetExtension_MultiPart_ExpectedResult(string input, string expectedResult)
   {
      // Arrange
      DefaultGeneralStorageContextUnit sut = new(null);

      // Act
      ReadOnlySpan<char> span = sut.GetExtension(input, true);
      string result = span.ToString();

      // Assert
      Assert.That.AreEqual(result, expectedResult);
   }
   #endregion
}
