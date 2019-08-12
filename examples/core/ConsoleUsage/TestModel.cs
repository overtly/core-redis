using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleUsage
{
    [Serializable]
    public class TestModel
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public List<decimal> Items { get; set; }
    }
}
