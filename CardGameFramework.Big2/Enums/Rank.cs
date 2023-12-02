using System.ComponentModel.DataAnnotations;

namespace CardGameFramework.Big2.Enums;

public enum Rank: byte
{
    [Display(Name = "2")]
    Two = 0,
    [Display(Name = "3")]
    Three,
    [Display(Name = "4")]
    Four,
    [Display(Name = "5")]
    Five,
    [Display(Name = "6")]
    Six,
    [Display(Name = "7")]
    Seven,
    [Display(Name = "8")]
    Eight,
    [Display(Name = "9")]
    Nine,
    [Display(Name = "10")]
    Ten,
    [Display(Name = "J")]
    Jack,
    [Display(Name = "Q")]
    Queen,
    [Display(Name = "K")]
    King,
    [Display(Name = "A")]
    Ace,
}