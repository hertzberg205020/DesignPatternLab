using System.ComponentModel.DataAnnotations;

namespace CardGameFramework.Big2.Enums;

public enum Suit: byte
{
    [Display(Name = "♣")]
    Club,    // 梅花
    [Display(Name = "♦")]
    Diamond, // 方塊
    [Display(Name = "♥")]
    Heart,   // 紅心
    [Display(Name = "♠")]
    Spade    // 黑桃
}