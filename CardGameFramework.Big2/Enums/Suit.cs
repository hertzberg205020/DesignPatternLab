using System.ComponentModel.DataAnnotations;

namespace CardGameFramework.Big2.Enums;

public enum Suit: byte
{
    [Display(Name = "C")]
    Club,    
    [Display(Name = "D")]
    Diamond, 
    [Display(Name = "H")]
    Heart, 
    [Display(Name = "S")]
    Spade
}