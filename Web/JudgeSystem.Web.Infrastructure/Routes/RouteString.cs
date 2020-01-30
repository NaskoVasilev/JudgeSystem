using JudgeSystem.Common;
using JudgeSystem.Web.Infrastructure.Extensions;

namespace JudgeSystem.Web.Infrastructure.Routes
{
    public class RouteString
    {
        private const char Slash = '/';

        public int QueryStringPairsCount { get; private set; }

        public string Value { get; private set; }

        public RouteString(string controller, string action)
        {
            Value = $"{Slash}{controller.ToControllerName()}{Slash}{action}";
        }

        public RouteString(string area, string controller, string action) : this(controller, action)
        {
            Value = $"{Slash}{area}{Value}";
        }

        public RouteString AppendId(object id)
        {
            Value += $"{Slash}{id}";
            return this;
        }

        public RouteString Append(string key, object value)
        {
            if(QueryStringPairsCount == 0)
            {
                AppendQueryStringSymbol();
            }

            string pair = $"{key}={value}";
            QueryStringPairsCount++;

            if(QueryStringPairsCount > 1)
            {
                pair = GlobalConstants.QueryStringDelimiter + pair;
            }

            Value += pair;
            return this;
        }

        public RouteString AppendPaginationPlaceholder() => Append(GlobalConstants.PageKey, "{0}");

        private void AppendQueryStringSymbol() => Value += "?";

        public static implicit operator string(RouteString route) => route.Value;
    }
}
