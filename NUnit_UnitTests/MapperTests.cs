namespace NUnit_UnitTests
{
    using NUnit.Framework;
    using Test_Task_Mapper;

    /// <summary>
    /// Unit tests of class Mapper
    /// </summary>
    [TestFixture]
    public class MapperTests
    {
        /// <summary>
        /// Instance of class Mapper
        /// </summary>
        private Mapper mapper;

        /// <summary>
        /// Initialize required objects
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.mapper = new Mapper();
        }

        /// <summary>
        /// Testing creation of maps
        /// </summary>
        [Test]
        public void CreateMap_TryToCreateFewSameMaps_SavedOnlyDifferentMapsAndLatestOfTheSameMaps()
        {
            // Act
            var map1 = this.mapper.CreateMap<Source, Destination>();
            var map2 = this.mapper.CreateMap<Source, Destination>();
            var map3 = this.mapper.CreateMap<Source, Destination>();
            var map4 = this.mapper.CreateMap<int, Destination>();

            // Assert
            Assert.IsNotNull(map1);
            Assert.IsNotNull(map2);
            Assert.IsNotNull(map3);
            Assert.IsNotNull(map4);
            Assert.AreEqual(map1, map2);
            Assert.AreEqual(map1, map3);
            Assert.AreEqual(map3, map2);
            Assert.AreNotEqual(map1, map4);
            Assert.AreEqual(2, this.mapper.Maps.Count);
        }

        /// <summary>
        /// Testing mapping of object to object of destination type
        /// </summary>
        [Test]
        public void Map_MapSourceObjecToObjectOfDestinationType_CorrectObjectIsCreated()
        {
            // Arrange
            var sourceObj = new Source();
            this.mapper.CreateMap<Source, Destination>();

            // Act
            var resultObject = this.mapper.Map<Destination>(sourceObj);

            // Assert
            Assert.IsNotNull(resultObject);
            Assert.IsInstanceOf(typeof(Destination), resultObject);
        }
    }
}
