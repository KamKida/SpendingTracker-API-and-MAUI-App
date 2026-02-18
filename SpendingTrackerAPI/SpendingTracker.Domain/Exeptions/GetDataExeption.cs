using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpendingTracker.Domain.Exeptions
{
    public class GetDataExeption : Exception
    {
        public GetDataExeption(string message) : base(message)
        {

        }
    }
}
