namespace CustomMapper
{
    using System.Collections.Generic;

    /// <summary>
    /// Class which is used as source type
    /// </summary>
    public class Source
    {
        public string StringProperty { get; set; } = "Str";

        public string StringToInt { get; set; } = "100";

        public int IntToStringProperty { get; set; } = 15;

        public double DoubleToIntProperty { get; set; } = 2.7;

        public int IntToFloatProperty { get; set; } = 8;

        public int[] ArrayProperty { get; set; } = { 1, 3, 4, 5, 6 };

        public List<int> ListProperty { get; set; } = new List<int> { 1, 3, 5, 3, 5 };
    }
}