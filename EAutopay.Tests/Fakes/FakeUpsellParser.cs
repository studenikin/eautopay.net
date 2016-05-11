using System.Collections.Generic;

using EAutopay.Upsells;
using EAutopay.Parsers;

namespace EAutopay.Tests.Fakes
{
    public class FakeUpsellParser : IUpsellParser
    {
        public UpsellSettings ExtractSettings(string source)
        {
            return new UpsellSettings();
        }

        public List<Upsell> ExtractUpsells(int productId, string source)
        {
            var ret = new List<Upsell>();

            var u1 = new Upsell();
            u1.ID = 1;
            u1.OriginID = 10;
            u1.ParentID = 100;
            u1.Price = 999.00;
            u1.Title = "title 1";
            u1.SuccessUri = "success1.com";
            u1.ClientUri = "client1.com";

            var u2 = new Upsell();
            u2.ID = 2;
            u2.OriginID = 20;
            u2.ParentID = 200;
            u2.Price = 1234.00;
            u2.SuccessUri = "success2.com";
            u2.Title = "title 2";
            u2.ClientUri = "client2.com";

            var u3 = new Upsell();
            u3.ID = 3;
            u3.OriginID = 30;
            u3.ParentID = 200;
            u3.Price = 777.00;
            u3.Title = "title 3";
            u3.SuccessUri = "success3.com";
            u3.ClientUri = "client3.com";
            
            ret.Add(u1);
            ret.Add(u2);
            ret.Add(u3);

            return ret;
        }
    }
}
