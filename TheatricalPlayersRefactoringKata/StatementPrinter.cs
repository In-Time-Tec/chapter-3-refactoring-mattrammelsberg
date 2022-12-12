using System;
using System.Collections.Generic;
using System.Globalization;

namespace TheatricalPlayersRefactoringKata
{
    public class StatementPrinter
    {
        public string Print(Invoice invoice, Dictionary<string, Play> plays)
        {
            var totalAmount = 0;
            var volumeCredits = 0;
            var result = string.Format("Statement for {0}\n", invoice.Customer);
            CultureInfo cultureInfo = new CultureInfo("en-US");

            foreach(var perf in invoice.Performances) 
            {
                var play = plays[perf.PlayID];
                var thisAmount = calculateAmountForPerf(perf, play.Type);
                volumeCredits += calculateVolCreditsForPerf(perf.Audience, play.Type);

                // print line for this order
                result += String.Format(cultureInfo, "  {0}: {1:C} ({2} seats)\n", play.Name, Convert.ToDecimal(thisAmount / 100), perf.Audience);
                totalAmount += thisAmount;
            }
            result += String.Format(cultureInfo, "Amount owed is {0:C}\n", Convert.ToDecimal(totalAmount / 100)) + String.Format("You earned {0} credits\n", volumeCredits);
            return result;
        }

        public int setAmountForTragedy(int perfAudience) {
            var thisAmount = 40000;
            if (perfAudience > 30) {
                thisAmount += 1000 * (perfAudience - 30);
            }
            return thisAmount;
        }

        public int setAmountForComedy(int perfAudience) {
            var thisAmount = 30000 + 300 * perfAudience;
            if (perfAudience > 20) {
                thisAmount += 10000 + 500 * (perfAudience - 20);
            }
            return thisAmount;
        }

        public int addExtraCreditsForEveryTenComedyAttendees(int perfAudience) {
            return (int)Math.Floor((decimal)perfAudience / 5);
        }

        public int calculateAmountForPerf(Performance perf, String playType) {
            var thisAmount = 0;
            switch (playType) 
            {
                case "tragedy":
                    thisAmount = setAmountForTragedy(perf.Audience);
                    break;
                case "comedy":
                    thisAmount = setAmountForComedy(perf.Audience);
                    break;
                default:
                    throw new Exception("unknown type: " + playType);
            }
            return thisAmount;
        }

        public int calculateVolCreditsForPerf(int audience, String playType) {
            var credits = Math.Max(audience - 30, 0);
            if(playType == "comedy") {
                credits+= addExtraCreditsForEveryTenComedyAttendees(audience);
            }
            return credits;
        }
    }
}
