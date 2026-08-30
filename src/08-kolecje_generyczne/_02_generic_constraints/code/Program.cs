#!/usr/bin/env dotnet-script
// Demonstracja Ograniczeń Typów Generycznych

using System;
using System.Collections.Generic;

namespace GenericConstraints
{
    // ========== OGRANICZENIA INTERFEJSU ==========
    
    public interface IComparable<T>
    {
        int CompareTo(T other);
    }
    
    public class IntegerWrapper : IComparable<IntegerWrapper>
    {
        public int Value { get; set; }
        
        public IntegerWrapper(int value) => Value = value;
        
        public int CompareTo(IntegerWrapper other) => Value.CompareTo(other.Value);
        
        public override string ToString() => Value.ToString();
    }
    
    /// <summary>
    /// Metoda wymaga, aby T implementował IComparable<T>
    /// </summary>
    public static T FindMax<T>(T a, T b) where T : IComparable<T>
    {
        return a.CompareTo(b) > 0 ? a : b;
    }
    
    // ========== OGRANICZENIE KLASY BAZOWEJ ==========
    
    public class Entity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    
    public class User : Entity
    {
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
    }
    
    public class Product : Entity
    {
        public string Title { get; set; } = "";
        public decimal Price { get; set; }
    }
    
    /// <summary>
    /// Repository musi pracować z typami dziedziczącymi z Entity
    /// </summary>
    public class EntityRepository<T> where T : Entity
    {
        private List<T> items = new();
        
        public void Add(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            items.Add(entity);
        }
        
        public T? GetById(int id) => items.Find(e => e.Id == id);
        
        public List<T> GetAll() => new(items);
        
        public void PrintInfo()
        {
            Console.WriteLine($"Repository<{typeof(T).Name}> contains {items.Count} items");
        }
    }
    
    // ========== OGRANICZENIE KONSTRUKTORA new() ==========
    
    public class Configuration
    {
        public string ConnectionString { get; set; } = "default";
        public int Timeout { get; set; } = 30;
    }
    
    public class AppSettings
    {
        public string AppName { get; set; } = "MyApp";
        public bool Debug { get; set; } = false;
    }
    
    /// <summary>
    /// Klasa Factory wymaga konstruktora bezparametrowego
    /// </summary>
    public class Factory<T> where T : new()
    {
        public T Create()
        {
            return new T();
        }
        
        public T CreateAndConfigure(Action<T> configure)
        {
            T instance = new T();
            configure(instance);
            return instance;
        }
    }
    
    // ========== OGRANICZENIE KLASY BAZOWEJ (Reference Type) ==========
    
    public class Cache<T> where T : class
    {
        private Dictionary<string, T> items = new();
        
        public void Add(string key, T value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            
            items[key] = value;
        }
        
        public T? Get(string key) => items.ContainsKey(key) ? items[key] : null;
    }
    
    // ========== OGRANICZENIE STRUKTURY (Value Type) ==========
    
    /// <summary>
    /// Holder tylko dla typów wartościowych
    /// </summary>
    public class Nullable<T> where T : struct
    {
        private T? value;
        private bool hasValue;
        
        public Nullable() { }
        
        public Nullable(T val)
        {
            value = val;
            hasValue = true;
        }
        
        public bool HasValue => hasValue;
        
        public T Value => hasValue ? value!.Value 
            : throw new InvalidOperationException("No value");
        
        public T GetValueOrDefault(T defaultValue) => hasValue ? value!.Value : defaultValue;
        
        public override string ToString() => hasValue ? value!.ToString()! : "null";
    }
    
    // ========== OGRANICZENIE notnull (C# 8+) ==========
    
    public class NotNullCache<T> where T : notnull
    {
        private Dictionary<T, string> cache = new();
        
        public void Set(T key, string value)
        {
            cache[key] = value;
        }
        
        public string? Get(T key) => cache.TryGetValue(key, out var value) ? value : null;
    }
    
    // ========== KOMBINACJA OGRANICZEŃ ==========
    
    public interface ITimestamped
    {
        DateTime CreatedAt { get; set; }
    }
    
    public class AuditedEntity : Entity, ITimestamped
    {
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; } = "";
    }
    
    /// <summary>
    /// Łączy wiele ograniczeń:
    /// - T musi być typem referencyjnym
    /// - T musi dziedziczyć z Entity
    /// - T musi implementować ITimestamped
    /// - T musi mieć bezparametrowy konstruktor
    /// </summary>
    public class AuditedRepository<T> 
        where T : class, Entity, ITimestamped, new()
    {
        public T Create(int id, string createdBy)
        {
            var entity = new T
            {
                Id = id,
                CreatedAt = DateTime.Now
            };
            
            if (entity is AuditedEntity audited)
                audited.CreatedBy = createdBy;
            
            return entity;
        }
    }
    
    // ========== WARTOŚĆ DOMYŚLNA ==========
    
    public class DefaultValueExample<T>
    {
        /// <summary>
        /// Zwraca default dla dowolnego typu
        /// </summary>
        public T GetDefault()
        {
            return default!;  // ! oznacza "nullable forgiving"
        }
        
        /// <summary>
        /// Sprawdza czy wartość równa się domyślnej
        /// </summary>
        public bool IsDefault<U>(U value) where U : IEquatable<U>
        {
            return value.Equals(default(U));
        }
    }
    
    // ========== TYPE ERASURE - SPRAWDZENIE ==========
    
    public class TypeInformationExample
    {
        public static void ShowTypeInfo()
        {
            List<int> ints = new();
            List<string> strings = new();
            
            var intType = ints.GetType();
            var stringType = strings.GetType();
            
            Console.WriteLine($"int List type: {intType}");
            Console.WriteLine($"string List type: {stringType}");
            Console.WriteLine($"Are they same? {intType == stringType}");  // false
            
            Console.WriteLine($"Is generic? {intType.IsGenericType}");
            var genericArgs = intType.GetGenericArguments();
            Console.WriteLine($"Generic argument: {genericArgs[0]}");
        }
    }
    
    // ========== HELPER CLASSES ==========
    
    public static void Main()
    {
        Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║     Ograniczenia Typów Generycznych - Demonstracja        ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");
        
        DemonstrateInterfaceConstraint();
        DemonstrateClassConstraint();
        DemonstrateConstructorConstraint();
        DemonstrateReferenceTypeConstraint();
        DemonstrateValueTypeConstraint();
        DemonstrateNotNullConstraint();
        DemonstrateCombinedConstraints();
        DemonstrateDefaultValue();
        DemonstrateTypeInformation();
    }
    
    private static void DemonstrateInterfaceConstraint()
    {
        Console.WriteLine("\n📌 OGRANICZENIE INTERFEJSU (IComparable<T>)\n");
        
        var a = new IntegerWrapper(10);
        var b = new IntegerWrapper(20);
        
        var max = FindMax(a, b);
        Console.WriteLine($"Max of {a} and {b}: {max}");
    }
    
    private static void DemonstrateClassConstraint()
    {
        Console.WriteLine("\n📌 OGRANICZENIE KLASY BAZOWEJ (Entity)\n");
        
        var userRepo = new EntityRepository<User>();
        userRepo.Add(new User { Id = 1, Name = "Alice", Email = "alice@example.com" });
        userRepo.Add(new User { Id = 2, Name = "Bob", Email = "bob@example.com" });
        userRepo.PrintInfo();
        
        var productRepo = new EntityRepository<Product>();
        productRepo.Add(new Product { Id = 1, Title = "Laptop", Price = 999.99m });
        productRepo.PrintInfo();
    }
    
    private static void DemonstrateConstructorConstraint()
    {
        Console.WriteLine("\n📌 OGRANICZENIE KONSTRUKTORA (new())\n");
        
        var configFactory = new Factory<Configuration>();
        var config = configFactory.Create();
        Console.WriteLine($"Config created: ConnectionString={config.ConnectionString}, Timeout={config.Timeout}");
        
        var appFactory = new Factory<AppSettings>();
        var settings = appFactory.CreateAndConfigure(s => 
        {
            s.AppName = "SuperApp";
            s.Debug = true;
        });
        Console.WriteLine($"Settings created: AppName={settings.AppName}, Debug={settings.Debug}");
    }
    
    private static void DemonstrateReferenceTypeConstraint()
    {
        Console.WriteLine("\n📌 OGRANICZENIE KLASY (Reference Type)\n");
        
        var userCache = new Cache<User>();
        userCache.Add("user1", new User { Id = 1, Name = "Alice" });
        var cached = userCache.Get("user1");
        Console.WriteLine($"Cached user: {cached?.Name}");
    }
    
    private static void DemonstrateValueTypeConstraint()
    {
        Console.WriteLine("\n📌 OGRANICZENIE STRUKTURY (Value Type)\n");
        
        var intNullable = new Nullable<int>(42);
        Console.WriteLine($"Nullable<int> with value: {intNullable}");
        Console.WriteLine($"Has value: {intNullable.HasValue}");
        Console.WriteLine($"Get or default: {intNullable.GetValueOrDefault(0)}");
        
        var emptyNullable = new Nullable<int>();
        Console.WriteLine($"Empty Nullable<int>: {emptyNullable}");
        Console.WriteLine($"Has value: {emptyNullable.HasValue}");
    }
    
    private static void DemonstrateNotNullConstraint()
    {
        Console.WriteLine("\n📌 OGRANICZENIE notnull\n");
        
        var cache = new NotNullCache<string>();
        cache.Set("key1", "value1");
        cache.Set("key2", "value2");
        
        Console.WriteLine($"Cached value: {cache.Get("key1")}");
    }
    
    private static void DemonstrateCombinedConstraints()
    {
        Console.WriteLine("\n📌 KOMBINACJA OGRANICZEŃ\n");
        
        var repo = new AuditedRepository<AuditedEntity>();
        var entity = repo.Create(1, "System");
        
        Console.WriteLine($"Created entity: Id={entity.Id}, CreatedAt={entity.CreatedAt}, CreatedBy={entity.CreatedBy}");
    }
    
    private static void DemonstrateDefaultValue()
    {
        Console.WriteLine("\n📌 WARTOŚĆ DOMYŚLNA\n");
        
        var example = new DefaultValueExample<int>();
        Console.WriteLine($"Default int: {example.GetDefault()}");
        
        var stringExample = new DefaultValueExample<string>();
        var defaultStr = stringExample.GetDefault();
        Console.WriteLine($"Default string: {(defaultStr == null ? "null" : defaultStr)}");
    }
    
    private static void DemonstrateTypeInformation()
    {
        Console.WriteLine("\n📌 INFORMACJA O TYPACH (Bez Type Erasure)\n");
        
        TypeInformationExample.ShowTypeInfo();
    }
}
