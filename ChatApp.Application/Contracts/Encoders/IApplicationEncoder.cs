namespace ChatApp.Application.Contracts.Encoders
{
    public interface IApplicationEncoder
    {
        string Encode(string input);
        string Decode(string input);
    }
}
