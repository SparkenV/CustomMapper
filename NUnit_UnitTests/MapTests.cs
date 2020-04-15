namespace NUnit_UnitTests
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using Test_Task_Mapper;

    /// <summary>
    /// Unit tests of class Map
    /// </summary>
    [TestFixture]
    public class MapTests
    {
        /// <summary>
        /// / Instance of class Map
        /// </summary>
        private Map map;

        /// <summary>
        /// Initialize required objects
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.map = new Map(typeof(Source), typeof(Destination));
        }

        /// <summary>
        /// Testing of assigning custom mapping action instead of default action
        /// </summary>
        /// <param name="value">Value of processed property</param>
        [Test]
        [TestCase(15)]
        [TestCase(0)]
        [TestCase(-15)]
        [TestCase(123124)]
        [TestCase(-3312321)]
        public void SetMappingAction_SetCustomMappingAction_ActionReturnCorrectObject(int value)
        {
            // Arrange
            var map_source_int = new Map(typeof(Source), typeof(int));

            var sourceObject = new Source()
            {
                IntToFloatProperty = value
            };

            map_source_int.SetMappingAction((obj) =>
            {
                return ((Source)obj).IntToFloatProperty;
            });

            // Act
            var result = map_source_int.MappingAction(sourceObject);

            // Assert
            Assert.AreEqual(value, result);
        }

        /// <summary>
        /// Testing mapping of object of internal type to object of custom type. Custom mapping action is set
        /// </summary>
        /// <param name="value">Value of processed property</param>
        [Test]
        [TestCase(-15)]
        [TestCase(1)]
        [TestCase(15)]
        [TestCase(123124)]
        public void SetMappingAction_CustomTypeToInternalType_ReturnDefaultValueOfInternalType(int value)
        {
            // Arrange
            var map_source_int = new Map(typeof(Source), typeof(int));

            var sourceObject = new Source()
            {
                IntToFloatProperty = value
            };

            // Act
            var result = map_source_int.MappingAction(sourceObject);

            // Assert
            Assert.AreEqual(default(int), result);
            Assert.AreNotEqual(value, result);
        }

        /// <summary>
        /// Testing mapping of object of custom type to object of other custom type
        /// </summary>
        [Test]
        public void DefaultMappingAction_CustomTypeToCustomType_ReturnCorrectValues()
        {
            // Arrange
            var sourceObject = new Source()
            {
                IntToFloatProperty = 13,
                DoubleToIntProperty = 42.7,
                IntToStringProperty = 10,
                StringProperty = "Just a string",
                StringToInt = "22",
                ArrayProperty = new int[2] { -23, 21 },
                ListProperty = new List<int> { 12, -32 }
            };

            // Act
            var result = this.map.DefaultMappingAction(sourceObject);

            // Assert
            Assert.IsInstanceOf(typeof(Destination), result);
            Assert.AreEqual(13.0f, ((Destination)result).IntToFloatProperty);
            Assert.AreEqual(43, ((Destination)result).DoubleToIntProperty);
            Assert.AreEqual("10", ((Destination)result).IntToStringProperty);
            Assert.AreEqual("Just a string", ((Destination)result).StringProperty);
            Assert.AreEqual(22, ((Destination)result).StringToInt);
            Assert.AreEqual(new int[2] { -23, 21 }, ((Destination)result).ArrayProperty);
            Assert.AreEqual(new List<int> { 12, -32 }, ((Destination)result).ListProperty);
        }

        /// <summary>
        /// Testing mapping of object of custom type to object of internal type
        /// </summary>
        [Test]
        public void DefaultMappingAction_CustomTypeToInternalType_ReturnDefaultInternalValues()
        {
            // Arrange
            var map_Source_to_int = new Map(typeof(Source), typeof(int));

            var sourceObject = new Source()
            {
                IntToFloatProperty = 13,
                DoubleToIntProperty = 42.7,
                IntToStringProperty = 10,
                StringProperty = "Just a string",
                StringToInt = "22",
                ArrayProperty = new int[2] { -23, 21 },
                ListProperty = new List<int> { 12, -32 }
            };

            // Act
            var result = map_Source_to_int.DefaultMappingAction(sourceObject);

            // Assert
            Assert.IsInstanceOf(typeof(int), result);
            Assert.AreEqual(default(int), result);
        }

        /// <summary>
        /// Testing throwing of specific exception when property is in incorrect format
        /// </summary>
        [Test]
        public void DefaultMappingAction_IncorrectStringToInt_FormatExceptionIsThrown()
        {
            Assert.Throws(
                typeof(FormatException), 
                new TestDelegate(() => 
                {
                    var sourceObject = new Source()
                    {
                        StringToInt = "2sd2sd",
                    };

                    map.DefaultMappingAction(sourceObject);
                }));
        }

        /// <summary>
        /// Testing throwing of specific exception when properties with same name have incompatible types
        /// </summary>
        [Test]
        public void DefaultMappingAction_PropertyIsNull_InvalidCastExceptionIsThrown()
        {
            Assert.Throws(
                typeof(InvalidCastException), 
                new TestDelegate(() => 
                {
                    var map_dynamic_to_destination = new Map(typeof(TestSource), typeof(Destination));
                    var sourceObject = new TestSource();
                    map_dynamic_to_destination.DefaultMappingAction(sourceObject);
                }));
        }

        /// <summary>
        /// Testing getting hash code of maps
        /// </summary>
        [Test]
        public void GetHashCode_CompareMaps_MapsAreEqual()
        {
            // Arrange
            var map_int_to_float = new Map(typeof(int), typeof(float));
            var map_string_to_Source = new Map(typeof(string), typeof(Source));

            var int_to_float_pair = new KeyValuePair<Type, Type>(typeof(int), typeof(float));
            var string_to_Source_pair = new KeyValuePair<Type, Type>(typeof(string), typeof(Source));
            var source_to_destination_pair = new KeyValuePair<Type, Type>(typeof(Source), typeof(Destination));

            // Act
            var int_to_float_hash_code = int_to_float_pair.GetHashCode();
            var string_to_Source_hash_code = string_to_Source_pair.GetHashCode();
            var source_to_destination_hash_code = source_to_destination_pair.GetHashCode();

            var map_int_to_float_hash_code = map_int_to_float.GetHashCode();
            var map_string_to_Source_hash_code = map_string_to_Source.GetHashCode();
            var map_source_to_destination_hash_code = this.map.GetHashCode();

            // Assert
            Assert.AreEqual(int_to_float_hash_code, map_int_to_float_hash_code);
            Assert.AreEqual(string_to_Source_hash_code, map_string_to_Source_hash_code);
            Assert.AreEqual(source_to_destination_hash_code, map_source_to_destination_hash_code);
        }

        /// <summary>
        /// Testing comparing of maps
        /// </summary>
        [Test]
        public void Equals_CompareMapsByMappingTypes_MapsAreEqualWhenMappingTypesAreSame()
        {
            // Arrange
            var map_int_to_float = new Map(typeof(int), typeof(float));
            var map_Source_to_Destination = new Map(typeof(Source), typeof(Destination));
            var map_string_to_Source = new Map(typeof(string), typeof(Source));

            // Act
            bool same_types = this.map.Equals(map_Source_to_Destination);
            bool int_to_float_equals_Source_to_Destination = map_int_to_float.Equals(this.map);
            bool int_to_float_equals_string_to_Source = map_int_to_float.Equals(map_string_to_Source);

            // Assert
            Assert.IsTrue(same_types);
            Assert.IsFalse(int_to_float_equals_Source_to_Destination);
            Assert.IsFalse(int_to_float_equals_string_to_Source);
        }

        /// <summary>
        /// Class which helps test situation when properties with same name have incompatible types
        /// </summary>
        public class TestSource
        {
            /// <summary>
            /// Gets property of array type
            /// </summary>
            public List<string> ArrayProperty { get; } = new List<string>() { "13d", "2s" };
        }
    }
}
