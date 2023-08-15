namespace WarmUp;

public class Classroom
{
    private readonly School _school;
    
    
    public Classroom(School school)
    {
        _school = school;
        _school.AddClassroom(this);
    }
}