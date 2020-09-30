using System.ComponentModel.DataAnnotations;

public class NewItem
{
    [EnumDataType(typeof(ItemType))]
    public ItemType type { get; set; }
    [Range(0, 99)]
    public int level { get; set; }
}
