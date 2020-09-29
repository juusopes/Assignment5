using System;
using System.ComponentModel.DataAnnotations;

public class Item
{
    [Range(0, 99)]
    public int level { get; set; }
    [CheckTheDate]
    public DateTime creationDate;
    public ItemType type { get; set; }
    public Guid itemId { get; set; }

}

public enum ItemType
{
    SWORD, POTION, SHIELD
}