using System.Collections.Generic;
using System.Linq;
using ETLAppInternal.Models.Jobs;
using ETLAppInternal.Models.Materials;

namespace ETLAppInternal.Models.General
{
    public static class PlmInstructionsHelper
    {
        public static string[] PlmInstructionOptions =
        {
            "Detroit Demo",
            "Stop at 1st Positive",
            "400 Point Count all <5%",
            "400 Point Count all <10%",
            "400 Point Count all trace",
            "1000 Point Count all <5%",
            "1000 Point Count all <10%",
            "1000 Point Count all trace",
            "Point Count Plaster (Trace to <3%)",
            "Point Count Plaster (<10%)",
            "Gravimetric Analysis all <3%",
            "Gravimetric Analysis all <5%",
            "Gravimetric Analysis all <10%",
            "Composite all drywall"
        };

        public static IEnumerable<PlmInstructions> GetInstructions()
        {
            return PlmInstructionOptions.Select(x => new PlmInstructions {Name = x, Selected = false});
        }

        public static string GetOption(string option)
        {
            switch (option)
            {
                case "Detroit Demo": return "Stop at first positive, point count plaster <5% and other materials that are greater than 0 and less than 1";
                case "Composite all drywall": return "Composite all drywall / joint compound / mud if any individual layers is positive";
                default: return option;
            }
        }
    }
}
