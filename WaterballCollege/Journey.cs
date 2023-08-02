namespace WaterballCollege;

public  class Journey
{
    public required string Name { get; init; }
    public required string Description { get; init; }

    public required decimal Price { get; set; }


    public Journey()
    {
        
    }

    public Journey(string name, string description, decimal price)
    {
        Name = name;
        Description = description;
        Price = price;
    }
}
