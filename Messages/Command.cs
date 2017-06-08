namespace BB.Bus.Messages
{
    public class Command : Message
    {
        public Command(string Name) : base(Name)
        {
        }
    }
}