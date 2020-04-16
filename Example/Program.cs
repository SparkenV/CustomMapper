using CustomMapper;
using System;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            // Creates source object and mapper
            var sourceObject = new Source();
            var mapper = new Mapper();

            // Creates map for required types
            var mapFromSourceToDestination = mapper.CreateMap<Source, Destination>();

            // Maps object with default mapping action
            var destinationObject = mapper.Map<Destination>(sourceObject);

            // Prints properties for checking result
            PrintAllPublicPropertiesOfObject(destinationObject);

            // Creates map for converting custom type to internal type
            var mapFromSourceToInt = mapper.CreateMap<Source, int>();

            // Adds custom mapping action for mapping special types
            mapFromSourceToInt.SetMappingAction((obj) =>
            {
                return ((Source)obj).IntToFloatProperty;
            });

            // Maps object of custom type to object of internal type
            var intObject = mapper.Map<int>(sourceObject);

            // Prints value of converted variable
            Console.WriteLine($"Int object: {intObject}");
            Console.ReadKey();
        }

        public static void PrintAllPublicPropertiesOfObject(object inputObject)
        {
            var properties = inputObject.GetType().GetProperties();

            foreach (var property in properties)
            {
                Console.WriteLine($"Property: {property.Name} equals \"{property.GetValue(inputObject)}\"");
            }
        }
    }
}
