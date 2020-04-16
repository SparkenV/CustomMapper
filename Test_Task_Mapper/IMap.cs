namespace CustomMapper
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Describes functional elements of map
    /// </summary>
    public interface IMap
    {
        /// <summary>
        /// Gets or sets pair which identifies map
        /// </summary>
        public KeyValuePair<Type, Type> MappingTypes { get; set; }

        /// <summary>
        /// Default action which is used to map objects
        /// </summary>
        /// <param name="sourceObject">Object which will be mapped</param>
        /// <returns>Object which is already mapped</returns>
        public object DefaultMappingAction(object sourceObject);
    }
}
