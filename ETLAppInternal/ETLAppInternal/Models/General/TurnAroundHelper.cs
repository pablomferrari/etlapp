namespace ETLAppInternal.Models.General
{
    public static class TurnAroundHelper
    {
        public static string[] TurnAroundOptions = {"Rush", "Same Day", "Next Day", "48 Hours", "72 Hours", "Standard" };

        public static int GetOptionId(string option)
        {
            switch (option)
            {
                case "Rush": return -1;
                case "Same Day": return 0;
                case "Next Day": return 1;
                case "48 Hours": return 2;
                case "72 Hours": return 3;
                default: return 5;
            }
        }
    }
}
