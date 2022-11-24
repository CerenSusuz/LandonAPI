using LandonWebAPI.Infrastructure.OptionProcessors;
using System.ComponentModel.DataAnnotations;

namespace LandonWebAPI.Models.Options;

public class SortOptions<T, TEntity> : IValidatableObject
{
    public string[] OrderBy { get; set; }

    //calls this validate incoming parameters
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var processor = new SortOptionsProcessor<T, TEntity>(OrderBy);

        var validTerms = processor.GetValidTerms().Select(term => term.Name);

        var invalidTerms = processor.GetAllTerms().Select(term => term.Name)
            .Except(validTerms, StringComparer.OrdinalIgnoreCase);

        foreach (var term in invalidTerms)
        {
            yield return new ValidationResult(
                $"Invalid sort term '{term}'.",
                new[] { nameof(OrderBy) });
        }
    }

    // the service code will call this to apply these sort options to a database query
    public IQueryable<TEntity> Apply(IQueryable<TEntity> query)
    {
        var processor = new SortOptionsProcessor<T, TEntity>(OrderBy);

        return processor.Apply(query);

    }
}
