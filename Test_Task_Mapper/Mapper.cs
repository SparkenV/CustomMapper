namespace Test_Task_Mapper
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Maps object from one type to other by maps
    /// </summary>
    public class Mapper : IMapper
    {
        /// <summary>
        /// Gets unique collection of maps
        /// </summary>
        public HashSet<Map> Maps { get; } = new HashSet<Map>();

        /// <summary>
        /// Creates map and saves it to unique collection
        /// </summary>
        /// <typeparam name="TScource">Type of source object</typeparam>
        /// <typeparam name="TDestination">Type of destination object</typeparam>
        /// <returns>Created map</returns>
        public Map CreateMap<TScource, TDestination>()
        {
            var newMap = new Map(typeof(TScource), typeof(TDestination));

            if (this.Maps.Contains(newMap))
            {
                this.Maps.Remove(newMap);
            }

            this.Maps.Add(newMap);

            return newMap;
        }

        /// <summary>
        /// Maps source object to destination type
        /// </summary>
        /// <typeparam name="TDestination">Destination type</typeparam>
        /// <param name="sourceObject">Object which will be mapped</param>
        /// <returns>Object which is mapped to destination type</returns>
        public TDestination Map<TDestination>(object sourceObject) where TDestination : new()
        {
            return (TDestination)this.Map(sourceObject, typeof(TDestination));
        }

        /// <summary>
        /// Maps source object to destination type
        /// </summary>
        /// <param name="sourceObject">Object which will be mapped</param>
        /// <param name="destinationType">Destination type</param>
        /// <returns>Object which is mapped to destination type</returns>
        public object Map(object sourceObject, Type destinationType)
        {
            var neededMap = new Map(sourceObject.GetType(), destinationType);

            if (this.Maps.Contains(neededMap))
            {
                this.Maps.TryGetValue(neededMap, out neededMap);
            }

            var destinationObject = neededMap.MappingAction(sourceObject);

            return destinationObject;
        }
    }
}
