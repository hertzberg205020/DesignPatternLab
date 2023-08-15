namespace WarmUp;

public class Person
{
    public InLove? InLove { get; set; }

    public Person()
    {
    }

    public void Love(Person person)
    {
        if (InLove != null)
        {
            throw new ArgumentException("只能與一人相愛");
        }
        InLove = new InLove(this, person);
        person.InLove = InLove;
    }

    public void RemoveInLove()
    {
        InLove = null;
    }
    
}

public class InLove
{
    public Person LPerson { get; set; }

    public Person RPerson { get; set; }

    public Marriage? Marriage { get; set; }

    public InLove(Person lPerson, Person rPerson)
    {
        LPerson = lPerson;
        RPerson = rPerson;
    }

    public void Marry()
    {
        Marriage = new Marriage(DateTime.Now, this);
    }
    
    /// <summary>
    /// 移除2人的戀愛關係
    /// </summary>
    public void BreakUp()
    {
        LPerson.RemoveInLove();
        RPerson.RemoveInLove();
    }
    
    public void RemoveMarriage()
    {
        Marriage = null;
    }
}

public class Marriage
{
    private readonly DateTime _anniversaryDay;
    private readonly InLove _inLove;

    public Marriage(DateTime anniversaryDay, InLove inLove)
    {
        _anniversaryDay = anniversaryDay;
        _inLove = inLove;
    }

    public void Divorce()
    {
        _inLove.BreakUp();
        _inLove.RemoveMarriage();
    }
}