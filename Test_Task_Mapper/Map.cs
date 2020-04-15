namespace Test_Task_Mapper
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Saves information about mapping
    /// </summary>
    public class Map : IEquatable<Map>, IMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref = "Map"/> class
        /// </summary>
        /// <param name="source">Type of source object</param>
        /// <param name="destination">Destination type of object</param>
        public Map(Type source, Type destination)
        {
            this.MappingTypes = new KeyValuePair<Type, Type>(source, destination);
            this.MappingAction = this.DefaultMappingAction;
        }

        /// <summary>
        /// Describes signature of action which is invoked for mapping
        /// </summary>
        /// <param name="sourceObject">Object which will be mapped</param>
        /// <returns>Object which is mapped to destination type</returns>
        public delegate object MappingDelegate(object sourceObject);

        /// <summary>
        /// Gets or sets pair which identifies map
        /// </summary>
        public KeyValuePair<Type, Type> MappingTypes { get; set; }

        /// <summary>
        /// Gets or sets action which is invoked for mapping
        /// </summary>
        public MappingDelegate MappingAction { get; set; }

        /// <summary>
        /// Changes action which is invoked for mapping
        /// </summary>
        /// <param name="mappingAction">Action which is invoked for mapping</param>
        public void SetMappingAction(MappingDelegate mappingAction)
        {
            this.MappingAction = mappingAction;
        }

        /// <summary>
        /// Default action which is invoked for mapping
        /// </summary>
        /// <param name="sourceObject">Object which will be mapped</param>
        /// <returns>Object which is mapped to destination type</returns>
        public object DefaultMappingAction(object sourceObject)
        {
            var destinationType = this.MappingTypes.Value;
            var sourceObjProperties = this.MappingTypes.Key.GetProperties();
            var destinationObjProperties = destinationType.GetProperties();

            var destinationObj = Activator.CreateInstance(destinationType);

            foreach (var sourceObjectProperty in sourceObjProperties)
            {
                foreach (var destinationOnjProperty in destinationObjProperties)
                {
                    if (sourceObjectProperty.Name.Equals(destinationOnjProperty.Name))
                    {
                        var value = sourceObjectProperty.GetValue(sourceObject);
                        try
                        {
                            if (sourceObjectProperty.PropertyType.IsEquivalentTo(destinationOnjProperty.PropertyType))
                            {
                                destinationType.GetProperty(destinationOnjProperty.Name).SetValue(destinationObj, value, default);
                            }
                            else
                            {
                                var compatibleValue = Convert.ChangeType(value, destinationOnjProperty.PropertyType);
                                destinationType.GetProperty(destinationOnjProperty.Name).SetValue(destinationObj, compatibleValue, default);
                            }
                        }
                        catch (InvalidCastException e)
                        {
                            Console.WriteLine($"InvalidCastException is thrown. Property \"{destinationOnjProperty.Name}\" is initialized with default value.");
                            destinationType.GetProperty(destinationOnjProperty.Name).SetValue(destinationObj, default);
                            throw e;
                        }
                        catch (FormatException e)
                        {
                            Console.WriteLine($"FormatException is thrown. Property \"{destinationOnjProperty.Name}\" is initialized with default value.");
                            destinationType.GetProperty(destinationOnjProperty.Name).SetValue(destinationObj, default);
                            throw e;
                        }
                    }
                }
            }

            return destinationObj;
        }

        /// <summary>
        /// Compares this object with other
        /// </summary>
        /// <param name="other">Other compared object</param>
        /// <returns>Result to comparison</returns>
        public bool Equals([AllowNull] Map other)
        {
            var equals = other.MappingTypes.Equals(this.MappingTypes);

            return equals;
        }

        /// <summary>
        /// Overrides getting hash code of this object
        /// </summary>
        /// <returns>Hash code of this object</returns>
        public override int GetHashCode()
        {
            return this.MappingTypes.GetHashCode();
        }
    }
}
