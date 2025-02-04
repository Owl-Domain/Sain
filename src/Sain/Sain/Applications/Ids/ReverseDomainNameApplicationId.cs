namespace OwlDomain.Sain.Applications.Ids;

/// <summary>
///   Represents an application id in the reverse domain name format.
/// </summary>
public sealed class ReverseDomainNameApplicationId : IReverseDomainNameApplicationId
{
   #region Properties
   /// <inheritdoc/>
   public IReadOnlyList<string> Components { get; }

   /// <inheritdoc/>
   public string DisplayName { get; }
   #endregion

   #region Constructors
   /// <summary>Creates a new instance of the <see cref="ReverseDomainNameApplicationId"/>.</summary>
   /// <param name="components">The components that make up the reverse domain name.</param>
   /// <exception cref="ArgumentOutOfRangeException">Thrown if the given <paramref name="components"/> is an empty list.</exception>
   public ReverseDomainNameApplicationId(IReadOnlyList<string> components)
   {
      if (components.Count <= 0)
         throw new ArgumentOutOfRangeException(nameof(components), "Expected the component list to have at least one component.");

      Components = components;
      DisplayName = string.Join('.', components);
   }

   /// <summary>Creates a new instance of the <see cref="ReverseDomainNameApplicationId"/>.</summary>
   /// <param name="reverseDomain">The reverse domain name.</param>
   public ReverseDomainNameApplicationId(string reverseDomain)
   {
      DisplayName = reverseDomain;
      Components = reverseDomain.Split('.', StringSplitOptions.RemoveEmptyEntries);
   }
   #endregion
}

/// <summary>
///   Contains various extension methods related to the <see cref="IApplicationBuilder"/> and the <see cref="ReverseDomainNameApplicationId"/>.
/// </summary>
public static class ApplicationBuilderReverseDomainNameIdExtensions
{
   #region Methods
   /// <summary>Adds an application id that follows the reverse domain name format.</summary>
   /// <typeparam name="TBuilder">The type of the application <paramref name="builder"/>.</typeparam>
   /// <param name="builder">The application builder to use.</param>
   /// <param name="components">The components that make up the reverse domain name.</param>
   /// <returns>The used <paramref name="builder"/> instance.</returns>
   public static TBuilder WithReverseDomainNameApplicationId<TBuilder>(this TBuilder builder, params IReadOnlyList<string> components)
      where TBuilder : notnull, IApplicationBuilder<TBuilder>
   {
      ReverseDomainNameApplicationId id = new(components);
      return builder.WithApplicationId(id);
   }

   /// <summary>Adds an application id that follows the reverse domain name format.</summary>
   /// <typeparam name="TBuilder">The type of the application <paramref name="builder"/>.</typeparam>
   /// <param name="builder">The application builder to use.</param>
   /// <param name="reverseDomainName">The reverse domain name.</param>
   /// <returns>The used <paramref name="builder"/> instance.</returns>
   public static TBuilder WithReverseDomainNameApplicationId<TBuilder>(this TBuilder builder, string reverseDomainName)
      where TBuilder : notnull, IApplicationBuilder<TBuilder>
   {
      ReverseDomainNameApplicationId id = new(reverseDomainName);
      return builder.WithApplicationId(id);
   }
   #endregion
}
