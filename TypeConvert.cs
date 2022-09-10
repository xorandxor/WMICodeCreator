namespace WMICodeCreator
{
    public static class TypeConvert
    {
        public static string CimTypeToSystemType(string LowerCaseCimType)
        {
            string output = "ERROR";

            if (LowerCaseCimType.ToLower() == "string")
            {
                output = "string";
            }
            if (LowerCaseCimType.ToLower().Contains("int"))
            {
                output = "int";
            }
            if (LowerCaseCimType.ToLower().Contains("bool"))
            {
                output = "bool";
            }
            if (LowerCaseCimType.ToLower().Contains("datetime"))
            {
                output = "DateTime";
            }

            return output;
        }
        public static string CimTypeToMSSQLType(string LowerCaseCimType)
        {
            string output = "ERROR";

            if (LowerCaseCimType.ToLower() == "string")
            {
                output = "nvarchar";
            }
            if (LowerCaseCimType.ToLower().Contains("int"))
            {
                output = "bigint";
            }
            if (LowerCaseCimType.ToLower().Contains("bool"))
            {
                output = "bit";
            }
            if (LowerCaseCimType.ToLower().Contains("datetime"))
            {
                output = "datetime";
            }
            


            return output;
        }
    }
}