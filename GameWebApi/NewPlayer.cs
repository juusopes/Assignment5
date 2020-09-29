using System;
using System.ComponentModel.DataAnnotations;

public class NewPlayer
{
    [StringLength(5)]
    public string Name { get; set; }
}