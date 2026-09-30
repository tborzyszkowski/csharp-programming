namespace PartialClasses;

// Część 3 z 3: walidacja i prezentacja.
public partial class Employee
{
    public bool IsValid() => !string.IsNullOrEmpty(name) && salary >= 0;

    public override string ToString() => $"{name} (ID:{id}) - {salary:C}";
}
