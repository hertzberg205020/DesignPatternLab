namespace WarmUp;

public class Directory: Item
{
    public ICollection<Item> ChildrenItems { get; } = new HashSet<Item>();
    
    public void Add(Item item)
    {
        ChildrenItems.Add(item);
        item.ParentDirectory = this;
    }

}