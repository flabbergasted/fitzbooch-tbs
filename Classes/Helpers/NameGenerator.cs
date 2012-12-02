using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TurnBasedStrategy
{
    class NameGenerator
    {
        static StreamReader srReader;
        static Array NameCategories;
        static Array SpecificNames;
        static Random ran;
  
        public static String GenerateName(Species species)
        {
            string result = "";
            string[] split = new string[1] { "\r\n" };
            char[] splitchar = new char[1] { ':' };
            
            switch (species)
            {
                case Species.Human:
                    ran = new Random();
                    srReader = new StreamReader("Data/HumanNames.txt");                 
                    break;
                default:
                    return "TEST";
                    break;
            }

            NameCategories = srReader.ReadToEnd().Split(splitchar, System.StringSplitOptions.RemoveEmptyEntries);

            for (int i = 1; i <= NameCategories.Length - 1; i = i + 2)
            {
                SpecificNames = ((String)NameCategories.GetValue(i)).Split(split, System.StringSplitOptions.RemoveEmptyEntries);

                result = result + SpecificNames.GetValue(ran.Next(0, SpecificNames.Length - 1));
            }
            result = result.Replace("%", string.Empty);

            return result;
                    
        }
    }
}
