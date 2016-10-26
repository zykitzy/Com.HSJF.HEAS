namespace Com.HSJF.HEAS.BLL.Other.Dto
{
    public class RelationStateBLLModel
    {
        public string Number { get; set; }
        public string Name { get; set; }
        public string SalesID { get; set; }
        public string Type { get; set; }
        public string Desc { get; set; }
        public string TextName { get; set; }
    }

    public class Error
    {
        public Error()
        {
        }

        public Error(string key, string message)
        {
            Key = key;
            Message = message;
        }

        public string Key { get; set; }
        public string Message { get; set; }
    }
}