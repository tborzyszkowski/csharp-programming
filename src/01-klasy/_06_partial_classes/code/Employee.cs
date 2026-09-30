namespace PartialClasses;

// Część 1 z 3: dane i konstruktor.
// Każda część musi mieć słowo kluczowe partial, tę samą nazwę, przestrzeń nazw i moduł (assembly).
public partial class Employee
{
    private string name;
    private int id;
    private decimal salary;

    public string Name => name;
    public int Id => id;
    public decimal Salary => salary;

    public Employee(string name, int id, decimal salary)
    {
        this.name = name;
        this.id = id;
        this.salary = salary;
    }
}
