namespace EAutopay.Parsers
{
    public interface ITokenParser
    {
        string ExtractToken(string source);
    }
}
