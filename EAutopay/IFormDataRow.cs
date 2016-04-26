using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAutopay.NET
{
    internal interface IFormDataRow
    {
        int ID { get; set; }
        string Name { get; set; }
    }
}
