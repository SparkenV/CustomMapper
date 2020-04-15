namespace Test_Task_Mapper
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Describes functional elements of mapper
    /// </summary>
    public interface IMapper
    {
        /// <summary>
        /// Gets unique collection of maps
        /// </summary>
        public HashSet<Map> Maps { get; }

        /// <summary>
        /// Maps source object to destination type
        /// </summary>
        /// <typeparam name="TDestination">Destination type</typeparam>
        /// <param name="sourceObject">Object which will be mapped</param>
        /// <returns>Object which is mapped to destination type</returns>
        TDestination Map<TDestination>(object sourceObject) where TDestination : new();

        /// <summary>
        /// Maps source object to destination type
        /// </summary>
        /// <param name="sourceObject">Object which will be mapped</param>
        /// <param name="destinationType">Destination type</param>
        /// <returns>Object which is mapped to destination type</returns>
        public object Map(object sourceObject, Type destinationType);
    }
}