using System.ComponentModel.DataAnnotations;

namespace CollisionDetection.Models;

public enum SpriteType
{
    [Display(Name = "W")]
    Water,
    [Display(Name = "F")]
    Fire,
    [Display(Name = "H")]
    Hero,
}