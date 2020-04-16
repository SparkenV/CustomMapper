namespace UnitTests
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using CustomMapper;

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
            var mapFromSourceToInt = new Map(typeof(Source), typeof(int));

            var sourceObject = new Source()
            {
                IntToFloatProperty = value
            };

            mapFromSourceToInt.SetMappingAction((obj) =>
            {
                return ((Source)obj).IntToFloatProperty;
            });

            // Act
            var result = mapFromSourceToInt.MappingAction(sourceObject);

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
            var mapFromSourceToInt = new Map(typeof(Source), typeof(int));

            var sourceObject = new Source()
            {
                IntToFloatProperty = value
            };

            // Act
            var result = mapFromSourceToInt.MappingAction(sourceObject);

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
            var mapFromSourceToInt = new Map(typeof(Source), typeof(int));

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
            var result = mapFromSourceToInt.DefaultMappingAction(sourceObject);

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
                    var mapFromTestSourceToDestination = new Map(typeof(TestSource), typeof(Destination));
                    var sourceObject = new TestSource();
                    mapFromTestSourceToDestination.DefaultMappingAction(sourceObject);
                }));
        }

        /// <summary>
        /// Testing getting hash code of maps
        /// </summary>
        [Test]
        public void GetHashCode_CompareMaps_MapsAreEqual()
        {
            // Arrange
            var mapFromIntToFloat = new Map(typeof(int), typeof(float));
            var mapStringToSource = new Map(typeof(string), typeof(Source));

            var intToFloatPair = new KeyValuePair<Type, Type>(typeof(int), typeof(float));
            var stringToSourcePair = new KeyValuePair<Type, Type>(typeof(string), typeof(Source));
            var sourceToDestinationPair = new KeyValuePair<Type, Type>(typeof(Source), typeof(Destination));

            // Act
            var intToFloatHashCode = intToFloatPair.GetHashCode();
            var stringToSourceHashCode = stringToSourcePair.GetHashCode();
            var sourceToDestinationHashCode = sourceToDestinationPair.GetHashCode();

            var mapIntToFloatHashCode = mapFromIntToFloat.GetHashCode();
            var mapStringToSourceHashCode = mapStringToSource.GetHashCode();
            var mapSourceToDestinationHashCode = this.map.GetHashCode();

            // Assert
            Assert.AreEqual(intToFloatHashCode, mapIntToFloatHashCode);
            Assert.AreEqual(stringToSourceHashCode, mapStringToSourceHashCode);
            Assert.AreEqual(sourceToDestinationHashCode, mapSourceToDestinationHashCode);
        }

        /// <summary>
        /// Testing comparing of maps
        /// </summary>
        [Test]
        public void Equals_CompareMapsByMappingTypes_MapsAreEqualWhenMappingTypesAreSame()
        {
            // Arrange
            var mapFromIntToFloat = new Map(typeof(int), typeof(float));
            var mapFromSourceToDestination = new Map(typeof(Source), typeof(Destination));
            var mapFromStringToSource = new Map(typeof(string), typeof(Source));

            // Act
            bool isSameTypesEqual = this.map.Equals(mapFromSourceToDestination);
            bool isIntToFloatEqualsSourceToDestination = mapFromIntToFloat.Equals(this.map);
            bool isIntToFloatEqualsStringToSource = mapFromIntToFloat.Equals(mapFromStringToSource);

            // Assert
            Assert.IsTrue(isSameTypesEqual);
            Assert.IsFalse(isIntToFloatEqualsSourceToDestination);
            Assert.IsFalse(isIntToFloatEqualsStringToSource);
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
