using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientsXML
{
    [Serializable]
    public class Registrator
    {
        public static int idNumber = 1;
        public string Name { get; set; }

        public int Id { get; set; }

        public Registrator()
        {
            Id = idNumber;
        }
    }
}
