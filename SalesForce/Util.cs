﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesForce
{
    public static class Util
    {

        private static CultureInfo cultureInfo = new CultureInfo("en-US");

        public static int ToInt(string value)
        {
            value = value.Replace(",", "");
            int index = value.IndexOf('.');
            if (index > 0)
            {
                value = value.Substring(0, index);
            }

            //Faz a conversão
            if (!String.IsNullOrWhiteSpace(value))
            {
                return int.Parse(value);
            }
            else
            {
                return 0;
            }
        }

        public static DateTime? ToDate(string value)
        {
            if (!String.IsNullOrWhiteSpace(value))
            {
                return DateTime.Parse(value, cultureInfo);
            }
            else
            {
                return null;
            }
        }

        public static decimal ToDecimal(string value)
        {
            //Limpa a string
            value = value.Replace("USD", "").Trim();
            value = value.Replace("%", "").Trim();

            //Faz a conversão
            if (!String.IsNullOrWhiteSpace(value))
            {
                return decimal.Parse(value, cultureInfo);
            }
            else
            {
                return 0;
            }
        }
    }
}
