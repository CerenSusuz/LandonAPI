using LandonWebAPI.Infrastructure.Attributes;
using LandonWebAPI.Infrastructure.Helper;
using LandonWebAPI.Infrastructure.Terms;
using System.Reflection;

namespace LandonWebAPI.Infrastructure.OptionProcessors;

public class SearchOptionsProcessor<T, TEntity>
{
    private readonly string[] _searchQuery;

    public SearchOptionsProcessor(string[] searchQuery)
    {
        _searchQuery = searchQuery;
    }

    public IEnumerable<SearchTerm> GetAllTerms()
    {
        if (_searchQuery == null)
        {
            yield break;
        }

        foreach (var expression in _searchQuery)
        {
            if (string.IsNullOrEmpty(expression))
            {
                continue;
            }

            var tokens = expression.Split(' ');

            if (tokens.Length == 0)
            {
                yield return new SearchTerm
                {
                    IsValidSyntax = true,
                    Name = expression
                };

                continue;
            }

            if (tokens.Length < 3)
            {
                yield return new SearchTerm
                {
                    IsValidSyntax = true,
                    Name = tokens[0]
                };

                continue;
            }

            yield return new SearchTerm
            {
                IsValidSyntax = true,
                Name = tokens[0],
                Operator = tokens[1],
                Value = String.Join(" ", tokens.Skip(2))
            };
        }
    }

    public IEnumerable<SearchTerm> GetValidTerms()
    {
        var queryTerms = GetAllTerms()
            .Where(x => x.IsValidSyntax)
            .ToArray();

        if (!queryTerms.Any())
        {
            yield break;
        }

        var declaredTerms = GetTermsFromModel();

        foreach (var term in queryTerms)
        {
            var declaredTerm = declaredTerms
                .SingleOrDefault(x => x.Name.Equals(term.Name, StringComparison.OrdinalIgnoreCase));
            if (declaredTerm == null)
            {
                continue;
            }

            yield return new SearchTerm
            {
                IsValidSyntax = term.IsValidSyntax,
                Name = declaredTerm.Name,
                Operator = term.Operator,
                Value = term.Value,
                ExpressionProvider = declaredTerm.ExpressionProvider
            };
        }
    }

    private static IEnumerable<SearchTerm> GetTermsFromModel()
        => typeof(T).GetTypeInfo()
        .DeclaredProperties
        .Where(property => property.GetCustomAttributes<SearchableAttribute>().Any())
        .Select(property => new SearchTerm
        {
            Name = property.Name,
            ExpressionProvider = property.GetCustomAttribute<SearchableAttribute>().ExpressionProvider
        });

    public IQueryable<TEntity> Apply(IQueryable<TEntity> query)
    {
        var terms = GetValidTerms().ToArray();

        if (!terms.Any())
        {
            return query;
        }

        var modifiedQuery = query;

        foreach (var term in terms)
        {
            var propertyInfo = ExpressionHelper
                .GetPropertyInfo<TEntity>(term.Name);
            var obj = ExpressionHelper.Parameter<TEntity>();

            // build up the LINQ expression backwards:
            // query = query.Where(x => x.Property == "Value")

            //x.Property
            var left = ExpressionHelper.GetPropertyExpression(obj, propertyInfo);

            //"Value"
            var right = term.ExpressionProvider.GetValue(term.Value);

            //x.Property == " Value "
            var comparisonExpression = term.ExpressionProvider
                .GetComparison(left, term.Operator, right);

            //x => x.Property == "Value"
            var lambdaExpression = ExpressionHelper
                .GetLambda<TEntity, bool>(obj, comparisonExpression);

            // query = query.Where...
            modifiedQuery = ExpressionHelper.CallWhere(modifiedQuery, lambdaExpression);
        }
        return modifiedQuery;
    }
}