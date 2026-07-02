using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIC_And_SICXE
{
  
        public class HTERecords
        {
          
            public List<HTERecord> ObjectCode { get; set; } = new List<HTERecord>();

            public int GetLenght()
            {
               if(ObjectCode.Count == 0)
                {
                    return 0;
                }
                
                int start = int.Parse(ObjectCode.FirstOrDefault().Address, System.Globalization.NumberStyles.HexNumber);
                int last =  int.Parse(ObjectCode.LastOrDefault().Address, System.Globalization.NumberStyles.HexNumber);
                return last - start;
            }
            public int GetFirst()
            {
                if (ObjectCode.FirstOrDefault() == null)
                {
                      return 0;
                }
                   return Convert.ToInt32( ObjectCode.FirstOrDefault().Address,16);
            }
            public override string ToString()
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append($"T^{GetFirst().ToString("X6")}^{GetLenght().ToString("X2")}");

                for (int i = 0; i < ObjectCode.Count -1; i++)
                {
                    stringBuilder.Append("^" + ObjectCode[i].ObjectCode.ToString());
                }
                
                return stringBuilder.ToString();
            }

        }
    
}
