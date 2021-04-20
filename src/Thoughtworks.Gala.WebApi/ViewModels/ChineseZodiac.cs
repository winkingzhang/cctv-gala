using System.Runtime.Serialization;

namespace Thoughtworks.Gala.WebApi.ViewModels
{
    public enum ChineseZodiac : uint
    {
        [EnumMember(Value = "Rat")]
        Rat = 1,
        [EnumMember(Value = "Ox")]
        Ox = 2,
        [EnumMember(Value = "Tiger")]
        Tiger = 3,
        [EnumMember(Value = "Rabbit")]
        Rabbit = 4,
        [EnumMember(Value = "Dragon")]
        Dragon = 5,
        [EnumMember(Value = "Snake")]
        Snake = 6,
        [EnumMember(Value = "Horse")]
        Horse = 7,
        [EnumMember(Value = "Goat")]
        Goat = 8,
        [EnumMember(Value = "Monkey")]
        Monkey = 9,
        [EnumMember(Value = "Rooster")]
        Rooster = 10,
        [EnumMember(Value = "Dog")]
        Dog = 11,
        [EnumMember(Value = "Pig")]
        Pig = 12
    }
}
