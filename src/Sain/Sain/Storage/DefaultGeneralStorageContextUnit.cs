namespace OwlDomain.Sain.Storage;

/// <summary>
///   Represents the default implementation for the application context unit that is responsible for general storage operations.
/// </summary>
/// <param name="provider">The context provider that this unit came from.</param>
public class DefaultGeneralStorageContextUnit(IContextProviderUnit? provider) : BaseGeneralStorageContextUnit(provider), IGeneralStorageContextUnit
{
   #region Methods
   /// <inheritdoc/>
   public override string GetDuplicateFileName(ReadOnlySpan<char> fileName, ReadOnlySpan<char> extension, int attempt)
   {
      // Note(Nightowl): ToString() is explicitly called on the spans as there are apparently problems with it on .NET Standard;

      if (extension.Length > 0 && extension[0] is not '.')
         return $"{fileName.ToString()} ({attempt:n0}).{extension.ToString()}";

      return $"{fileName.ToString()} ({attempt:n0}){extension.ToString()}";
   }

   /// <inheritdoc/>
   public override ReadOnlySpan<char> GetExtension(ReadOnlySpan<char> fileName, bool multipartExtension = true)
   {
      if (multipartExtension)
         return GetMultipartExtension(fileName);

      return GetSingleExtension(fileName);
   }

   /// <inheritdoc/>
   public override ReadOnlySpan<char> GetFileName(ReadOnlySpan<char> path) => Path.GetFileName(path);

   /// <inheritdoc/>
   public override ReadOnlySpan<char> GetFileNameWithoutExtension(ReadOnlySpan<char> path, bool multipartExtension = true)
   {
      ReadOnlySpan<char> fileName = GetFileName(path);
      ReadOnlySpan<char> extension = GetExtension(fileName, multipartExtension);

      if (extension.Length is 0 && fileName.Length > 0 && fileName[^1] == '.')
         return fileName[..^(extension.Length + 1)];

      return fileName[..^extension.Length];
   }
   #endregion

   #region Helpers
   private static ReadOnlySpan<char> GetSingleExtension(ReadOnlySpan<char> fileName)
   {
      int length = fileName.Length;

      for (int i = length - 1; i >= 0; i--)
      {
         char ch = fileName[i];

         if (ch is '.')
         {
            if (i == length - 1)
               return [];

            return fileName[i..];
         }

         if (ch == Path.DirectorySeparatorChar || ch == Path.AltDirectorySeparatorChar)
            break;
      }

      return [];
   }
   private static ReadOnlySpan<char> GetMultipartExtension(ReadOnlySpan<char> fileName)
   {
      int length = fileName.Length;
      int earliestDot = length;

      for (int i = length - 1; i >= 0; i--)
      {
         char ch = fileName[i];

         if (ch is '.')
            earliestDot = i;
         else if (ch == Path.DirectorySeparatorChar || ch == Path.AltDirectorySeparatorChar)
            break;
      }

      if (earliestDot == length - 1)
         return [];

      return fileName[earliestDot..];
   }
   #endregion
}
