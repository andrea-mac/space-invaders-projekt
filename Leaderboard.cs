using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AndreaOtterProjekt
{
    /// <summary>
    /// Leaderboard class
    /// </summary>
    public static class Leaderboard
    {
        public static string SaveDirectory = "savefile.xml";
        public static XmlElement prevElement;

        public static void AddScore(string Name, string Score)
        {
            //Writes the XML elements and attributes
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement root;
            if (!System.IO.File.Exists(SaveDirectory))
            {
                root = xmlDoc.CreateElement("leaderboard");
                xmlDoc.AppendChild(root);
            }

            else
            {
                xmlDoc.Load(SaveDirectory);
                root = xmlDoc.DocumentElement;
            }

            if (prevElement != null)
            {
                if (prevElement.Attributes["name"].Value == Name &&
                    prevElement.Attributes["score"].Value == Score)
                {
                    return;
                }
            }


            XmlElement playerNode = xmlDoc.CreateElement("player");

            playerNode.SetAttribute("name", Name);
            playerNode.SetAttribute("score", Score);

            root.AppendChild(playerNode);

            prevElement = playerNode;

            xmlDoc.Save(SaveDirectory);
        }
    }
}
