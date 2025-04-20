namespace Finance_Manager_Backend.BusinessLogic.Models;

public class Saving : IEntity
{
    public Saving() { }
    public Saving(string name, decimal goal, User user) 
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name can't be null or empty", nameof(name));

        if(goal < 0)
            throw new ArgumentOutOfRangeException("Goal can't be negative", nameof(goal));

        if (user == null)
            throw new ArgumentNullException("User can't be null", nameof(user));

        Name = name;
        Goal = goal;
        User = user;
        UserId = user.Id;
    }
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Goal { get; set; }
    public decimal CurrentAmount { get; set; } = 0;
    public int UserId { get; set; }
    public User? User { get; set; } // = new();
}
