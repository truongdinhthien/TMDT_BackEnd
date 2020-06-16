namespace MessageBus.Message
{
    public class AddAmountMessage
    {
        public int BookId {get;set;}
        public int Amount {get;set;}
        public bool isAdd{get;set;}
    }
}