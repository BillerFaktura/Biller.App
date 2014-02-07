using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biller.Data.Samples
{
    public class Units
    {
        public static Utils.Unit GetKilogrammUnit()
        {
            return new Utils.Unit() { Name = "Kilogramm", ShortName = "Kg", DecimalDigits = 3, DecimalSeperator = ",", ThousandSeperator = "" };
        }

        public static Utils.Unit GetPieceUnit()
        {
            return new Utils.Unit() { Name = "Stück", ShortName = "St", DecimalDigits = 0, DecimalSeperator = "", ThousandSeperator = "." };
        }
    }
}
