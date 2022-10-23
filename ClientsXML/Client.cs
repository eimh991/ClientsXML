using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientsXML
{
    [Serializable]
    public class Client
    {
        public string FIO { get; set; }
        public short RegNumber { get; set; }
        public long DiasoftID { get; set; }
        public Registrator RegistratorInfo { get; set; }

        public Client(string fullName,short regNumber,long diasoftID, Registrator registrator)
        {
            FIO = fullName;
            RegNumber = regNumber;
            DiasoftID = diasoftID;
            RegistratorInfo = registrator;
        }
        public Client() 
        {
            FIO = null;
            RegNumber = 0;
            DiasoftID = 0;
        }
    }

    //<FIO>Белова Валентина Петровна</FIO>
    //<RegNumber>739</RegNumber>
    //<DiasoftID>2010000009136</DiasoftID>
    //<Registrator>Зигаева А.</Registrator>
}
