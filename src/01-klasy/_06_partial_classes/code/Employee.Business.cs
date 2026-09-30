namespace PartialClasses;

// Część 2 z 3: logika biznesowa (ma dostęp do prywatnych pól z pozostałych części).
public partial class Employee
{
    public void GiveRaise(decimal amount)
    {
        if (amount > 0)
            salary += amount;
    }

    public bool IsHighEarner() => salary >= 5000;
}
