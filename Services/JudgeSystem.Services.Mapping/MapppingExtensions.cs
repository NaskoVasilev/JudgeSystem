using System;
using System.Linq;
using System.Linq.Expressions;

using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace JudgeSystem.Services.Mapping
{
    public static class MappingExtensions
    {
        public static IQueryable<TDestination> To<TDestination>(
            this IQueryable source,
            params Expression<Func<TDestination, object>>[] membersToExpand)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.ProjectTo(membersToExpand);
        }

        public static IQueryable<TDestination> To<TDestination>(
            this IQueryable source,
            object parameters)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.ProjectTo<TDestination>(parameters);
        }

		public static Destination To<Destination>(this object source)
		{
			var destination = Mapper.Map(source, source.GetType(), typeof(Destination));
			return (Destination)destination;
		}
	}
}
