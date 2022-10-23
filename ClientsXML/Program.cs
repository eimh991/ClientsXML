using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;


namespace ClientsXML
{
    internal class Program
    {
        
        static void Main(string[] args)
        {
            XmlDocument xmlDocument = new XmlDocument();
            Dictionary<string,int> countUncorrectClients = new Dictionary<string, int>();
            List<Client> allListClients = new List<Client>();
            List<Client> corecctClients = new List<Client>();
            List<string> registratorsName = new List<string>();
            List<Registrator> registratorsList = new List<Registrator>();


            xmlDocument.Load("Clients.xml");
            XmlElement clients = xmlDocument.DocumentElement;
            registratorsName = CreateRegistratorNameList(clients);
            registratorsList = CreateRegistratorList(registratorsName);
            allListClients = CreateClientsList(clients, registratorsList);
            countUncorrectClients = ALlCountUncorrectClient(allListClients);
            corecctClients = CurrectClients(allListClients);
            CreateCurrentClientsXML(corecctClients);
            CreateCurrentRegistratorXML(registratorsList);
            SortMistakes(countUncorrectClients);
            Console.WriteLine(allListClients.Count);
            Console.WriteLine(corecctClients.Count);
        }
        public static List<string> CreateRegistratorNameList(XmlElement clientsRoot)
        {
            List<string> allRegistratorsName = new List<string>();
            foreach (XmlElement clientElem in clientsRoot)
            {
                foreach (XmlNode child in clientElem.ChildNodes)
                {
                    if (child.Name == "Registrator")
                    {
                        bool test = false;
                        if(allRegistratorsName.Count == 0)
                        {
                            allRegistratorsName.Add(child.InnerText);
                        }
                        for(int i = 0; i < allRegistratorsName.Count; i++)
                        {
                            if (child.InnerText == allRegistratorsName[i])
                            {
                                test = true;
                            }
                        }
                        if (!test)
                        {
                            allRegistratorsName.Add(child.InnerText);
                        }
                        
                    }
                }
                
            }
            return allRegistratorsName;
        }
        public static List<Registrator> CreateRegistratorList(List<string> name)
        {
            List<Registrator> registratorList = new List<Registrator>();

            for (int i = 0; i < name.Count; i++)
            {
                registratorList.Add(new Registrator { Name = name[i] });
                Registrator.idNumber++;
            }
            return registratorList;
        }

        public static List<Client> CreateClientsList(XmlElement clientsRoot, List<Registrator> registrators)
        {
            List<Client> AllClients = new List<Client>();
            foreach (XmlElement clientElem in clientsRoot)
            {
                Client client = new Client();
                foreach (XmlNode child in clientElem.ChildNodes)
                {
                    
                    if (child.Name == "FIO")
                    {
                        client.FIO = child.InnerText;
                    }
                    if (child.Name == "RegNumber")
                    {
                        client.RegNumber = short.Parse(child.InnerText);
                    }
                    if (child.Name == "DiasoftID")
                    {
                        client.DiasoftID = long.Parse(child.InnerText);
                    }
                    if (child.Name == "Registrator")
                    {
                        if (child.InnerText != " " || child.InnerText != null)
                        {
                            foreach (var reg in registrators)
                            {
                                if (reg.Name == child.InnerText)
                                {
                                    client.RegistratorInfo = reg;
                                }
                            }
                        }
                    }
                }
                AllClients.Add(client);
            }
            return AllClients;
        }
        public static Dictionary<string, int> ALlCountUncorrectClient(List<Client> allClients)
        {
            Dictionary<string, int> countUncorrectClients = new Dictionary<string, int>();
            int fioUncorrect = 0;
            int regNumberUncorrect = 0;
            int diasoftId = 0;
            int registrator = 0;
            foreach (Client client in allClients)
            {
                if (client.FIO == null || client.FIO=="")
                {
                    fioUncorrect++;
                }
                else if (client.RegNumber == 0)
                {
                    regNumberUncorrect++;
                }
                else if (client.DiasoftID == 0)
                {
                    diasoftId++;
                }
                else if (client.RegistratorInfo.Name == "")
                {
                    registrator++;
                }
            }
            countUncorrectClients.Add("Не указано ФИО", fioUncorrect);
            countUncorrectClients.Add("Не указан Регистрационный номер",regNumberUncorrect);
            countUncorrectClients.Add("Не указан DiasoftID", diasoftId);
            countUncorrectClients.Add("Не указан Регистратор" ,registrator);
            return countUncorrectClients;
        }
        public static List<Client> CurrectClients(List<Client> allClients)
        {
            List<Client> currectClients = new List<Client>();

            foreach(Client client in allClients)
            {
                if(client.FIO != null && client.FIO != "" && client.RegNumber != 0
                    && client.DiasoftID !=0 && client.RegistratorInfo != null && client.RegistratorInfo.Name != "")
                {
                    currectClients.Add(client);
                }
            }
            return currectClients;
        }

        public static void CreateCurrentClientsXML(List<Client> corecctClients)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(List<Client>));
            using (FileStream fs = new FileStream("CurrentClients.xml", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, corecctClients);
            }
        }

        public static void CreateCurrentRegistratorXML(List<Registrator> registratorsList)
        {
            List<Registrator> reg = new List<Registrator>();
            foreach(var registrator in registratorsList)
            {
                if(registrator.Name != "")
                {
                    reg.Add(registrator);
                }
            }
            XmlSerializer formatter = new XmlSerializer(typeof(List<Registrator>));
            using (FileStream fs = new FileStream("Registrator.xml", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, reg);
            }
        }

        public static void SortMistakes(Dictionary<string, int> countUncorrectClients)
        {
            string text = "";
            int allMistake = 0;
            foreach (var pair in countUncorrectClients.OrderBy(pair => pair.Value))
            {
                if (pair.Value != 0)
                {
                    text += pair.Key + ": " + pair.Value.ToString()+"\n";
                    allMistake += pair.Value;
                }
            }
            text += "Всего ошибочных записей : " + allMistake.ToString();
            CreateFileWithMistakes(text);
        }
        public static void CreateFileWithMistakes(string text)
        {
            StreamWriter sw = new StreamWriter("Mistake.txt",true, Encoding.UTF8);
            sw.WriteLine(text);
            sw.Close();
        }

    }

}
