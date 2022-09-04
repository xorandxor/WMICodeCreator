using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMICodeCreator
{
    public static class TypeConvert
    {
        public static string CimTypeToSystemType(string LowerCaseCimType)
        {
            string output = "";
            if (LowerCaseCimType.ToLower() == "string")
                output = "string";

            if(LowerCaseCimType.ToLower().Contains("int"))
            {
                //some kinda int bullshit
                output = "int";
            }
            if (LowerCaseCimType.ToLower().Contains("bool"))
            {
                output = "bool";
            }
            return output;






        }



    }
}
