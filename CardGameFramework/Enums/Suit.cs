using System.ComponentModel.DataAnnotations;

namespace CardGameFramework.Enums;

public enum Suit: byte
{
    [Display(Name = "♣")]
    Clubs,    // 梅花
    [Display(Name = "♦")]
    Diamonds, // 方塊
    [Display(Name = "♥")]
    Hearts,   // 紅心
    [Display(Name = "♠")]
    Spades    // 黑桃
}