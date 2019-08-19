﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using AutoMapper;
using AutoMapper.Configuration;

namespace JudgeSystem.Services.Mapping
{
    public static class AutoMapperConfig
    {
        private static bool initialized;

        public static void RegisterMappings(params Assembly[] assemblies)
        {
            if (initialized)
            {
                return;
            }

            initialized = true;

            var types = assemblies.SelectMany(a => a.GetExportedTypes()).ToList();

            var config = new MapperConfigurationExpression();
            config.CreateProfile("ReflectionProfile", configuration =>
            {
                // IMapFrom<>
                foreach (TypesMap map in GetFromMaps(types))
                {
                    configuration.CreateMap(map.Source, map.Destination);
                }

                // IMapTo<>
                foreach (TypesMap map in GetToMaps(types))
                {
                    configuration.CreateMap(map.Source, map.Destination);
                }

                // IHaveCustomMappings
                foreach (IHaveCustomMappings map in GetCustomMappings(types))
                {
                    map.CreateMappings(configuration);
                }
            });

            Mapper.Initialize(config);
        }

        private static IEnumerable<TypesMap> GetFromMaps(IEnumerable<Type> types)
        {
            IEnumerable<TypesMap> fromMaps = from t in types
                           from i in t.GetTypeInfo().GetInterfaces()
                           where i.GetTypeInfo().IsGenericType &&
                                 i.GetGenericTypeDefinition() == typeof(IMapFrom<>) &&
                                 !t.GetTypeInfo().IsAbstract &&
                                 !t.GetTypeInfo().IsInterface
                           select new TypesMap
                           {
                               Source = i.GetTypeInfo().GetGenericArguments()[0],
                               Destination = t,
                           };

            return fromMaps;
        }

        private static IEnumerable<TypesMap> GetToMaps(IEnumerable<Type> types)
        {
            IEnumerable<TypesMap> toMaps = from t in types
                         from i in t.GetTypeInfo().GetInterfaces()
                         where i.GetTypeInfo().IsGenericType &&
                               i.GetTypeInfo().GetGenericTypeDefinition() == typeof(IMapTo<>) &&
                               !t.GetTypeInfo().IsAbstract &&
                               !t.GetTypeInfo().IsInterface
                         select new TypesMap
                         {
                             Source = t,
                             Destination = i.GetTypeInfo().GetGenericArguments()[0],
                         };

            return toMaps;
        }

        private static IEnumerable<IHaveCustomMappings> GetCustomMappings(IEnumerable<Type> types)
        {
            IEnumerable<IHaveCustomMappings> customMaps = from t in types
                             from i in t.GetTypeInfo().GetInterfaces()
                             where typeof(IHaveCustomMappings).GetTypeInfo().IsAssignableFrom(t) &&
                                   !t.GetTypeInfo().IsAbstract &&
                                   !t.GetTypeInfo().IsInterface
                             select (IHaveCustomMappings)Activator.CreateInstance(t);

            return customMaps;
        }

        private class TypesMap
        {
            public Type Source { get; set; }

            public Type Destination { get; set; }
        }
    }
}
