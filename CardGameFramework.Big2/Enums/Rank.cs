using System.ComponentModel.DataAnnotations;

namespace CardGameFramework.Big2.Enums;

public enum Rank: byte
{
    [Display(Name = "3")]
    Three = 0,
    [Display(Name = "4")]
    Four = 1,
    [Display(Name = "5")]
    Five = 2,
    [Display(Name = "6")]
    Six = 3,
    [Display(Name = "7")]
    Seven = 4,
    [Display(Name = "8")]
    Eight = 5,
    [Display(Name = "9")]
    Nine = 6,
    [Display(Name = "10")]
    Ten = 7,
    [Display(Name = "J")]
    Jack = 8,
    [Display(Name = "Q")]
    Queen = 9,
    [Display(Name = "K")]
    King = 10,
    [Display(Name = "A")]
    Ace = 11,
    [Display(Name = "2")]
    Two = 12
}