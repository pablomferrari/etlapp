using System.Collections.Generic;
using System.Linq;

namespace ETLAppInternal.Models.Materials
{
    public static class MaterialHelper
    {
        public static IEnumerable<Room> GetRooms()
        {

            var numbers = Enumerable.Range(1, 100).Select(x => new Room {Name = x.ToString()}).ToList();
            var extGarage = new Room { Name = "Exterior garage" };
            var extHouse = new Room { Name = "Exterior house" };
            var garage = new Room {Name = "Throughout garage"};
            var house = new Room { Name = "Throughout house" };
            numbers.Insert(0, garage);
            numbers.Insert(0, house);
            numbers.Insert(0, extGarage);
            numbers.Insert(0, extHouse);
            return numbers;
        }

        public static string[] Sizes()
        {
            var list = new List<string>
            {
                "9x9",
                "12x12",
                "18x18",
                "24x24",
                "2x4",
                "2x2",
                "1x1",
                "10x2",
                "2",
                "4",
                "6",
                "8",
                "10",
                "12",
                "2 - 4",
                "5 - 8",
                "8 - 12",
                "12 and up"

            };
            return list.ToArray();
        }

        public static string[] Colors()
        {
            var list = new List<string>
            {
                "Biege",
                "Black",
                "Blue",
                "Bluish Green",
                "Brown",
                "Dark Blue",
                "Dark Brown",
                "Dark Green",
                "Dark Grey",
                "Dark Orange",
                "Dark Red",
                "Dark Tan",
                "Dark Yellow",
                "Green",
                "Grey",
                "Light Blue",
                "Light Brown",
                "Light Green",
                "Light Grey",
                "Light Orange",
                "Light Red",
                "Light Tan",
                "Light Yellow",
                "Multi-Color",
                "Off-White",
                "Orange",
                "Pink",
                "Purple",
                "Red",
                "Silver",
                "Silver",
                "Tan",
                "White",
                "Wood Grain",
                "Yellow",

            };
            return list.ToArray();
        }
    }
}
