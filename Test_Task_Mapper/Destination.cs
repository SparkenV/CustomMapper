namespace CustomMapper
{
    using System.Collections.Generic;

    /// <summary>
    /// Class which is used as destination type
    /// </summary>
    public class Destination
    {
        public string StringProperty { get; set; }

        public int StringToInt { get; set; }

        public string IntToStringProperty { get; set; }

        public float IntToFloatProperty { get; set; }

        public int DoubleToIntProperty { get; set; }

        public int[] ArrayProperty { get; set; }

        public List<int> ListProperty { get; set; } = new List<int>();
    }
}