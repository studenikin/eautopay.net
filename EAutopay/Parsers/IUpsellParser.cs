using System.Collections.Generic;

using EAutopay.Upsells;

namespace EAutopay.Parsers
{
    public interface IUpsellParser
    {
        UpsellSettings ExtractSettings(string source);

        List<Upsell> ExtractUpsells(int productId, string source);
    }
}
