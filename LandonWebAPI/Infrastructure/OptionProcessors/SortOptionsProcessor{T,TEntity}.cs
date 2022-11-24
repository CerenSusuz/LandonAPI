using System.Reflection;
using LandonWebAPI.Infrastructure.Attributes;
using LandonWebAPI.Infrastructure.Helper;
using LandonWebAPI.Infrastructure.Terms;

namespace LandonWebAPI.Infrastructure.OptionProcessors;

public class SortOptionsProcessor<T, TEntity>
{
    private readonly string[] _orderBy;

    public SortOptionsProcessor(string[] orderBy)
    {
        _orderBy = orderBy;
    }

    public IEnumerable<SortTerm> GetAllTerms()
    {
        if (_orderBy == null)
        {
            yield break;
        }

        foreach (var term in _orderBy)
        {
            if (string.IsNullOrEmpty(term))
            {
                continue;
            }

            var tokens = term.Split(' ');

            if (tokens.Length == 0)
            {
                yield return new SortTerm { Name = term };
                continue;
            }

            var descending = tokens.Length > 1 && tokens[1]
                .Equals("desc", StringComparison.OrdinalIgnoreCase);

            yield return new SortTerm
            {
                Name = tokens[0],
                Descending = descending
            };
        }
    }

    private static IEnumerable<SortTerm> GetTermsFromModel() =>
        typeof(T).GetTypeInfo()
        .DeclaredProperties
        .Where(property => property.GetCustomAttributes<SortableAttribute>().Any())
        .Select(property => new SortTerm
        {
            Name = property.Name,
            Default = property.GetCustomAttribute<SortableAttribute>().Default
        });

    public IEnumerable<SortTerm> GetValidTerms()
    {
        var queryTerms = GetAllTerms().ToArray();

        if (!queryTerms.Any())
        {
            yield break;
        }

        var declaredTerms = GetTermsFromModel();

        foreach (var term in declaredTerms)
        {
            var declaredTerm = declaredTerms
                .SingleOrDefault(item => item.Name.Equals(term.Name,
                StringComparison.OrdinalIgnoreCase));

            if (declaredTerm == null)
            {
                continue;
            }

            yield return new SortTerm
            {
                Name = declaredTerm.Name,
                Descending = term.Descending,
                Default = declaredTerm.Default
            };
        }
    }

    public IQueryable<TEntity> Apply(IQueryable<TEntity> query)
    {
        var terms = GetValidTerms().ToArray();

        if (!terms.Any())
        {
            terms = GetTermsFromModel().Where(term => term.Default).ToArray();
        }

        if (!terms.Any())
        {
            return query;
        }

        var modifiedQuery = query;
        var useThenBy = false;

        foreach (var term in terms)
        {
            var propertyInfo = ExpressionHelper
                .GetPropertyInfo<TEntity>(term.Name);
            var obj = ExpressionHelper.
                Parameter<TEntity>();

            var key = ExpressionHelper
                .GetPropertyExpression(obj, propertyInfo);
            var keySelector = ExpressionHelper
                .GetLambda(typeof(TEntity), propertyInfo.PropertyType, obj, key);

            modifiedQuery = ExpressionHelper
                .CallOrderByOrThenBy(modifiedQuery, useThenBy, term.Descending, propertyInfo.PropertyType, keySelector);

            useThenBy = true;
        }

        return modifiedQuery;
    }
}