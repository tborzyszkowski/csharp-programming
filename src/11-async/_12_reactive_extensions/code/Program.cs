using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

Console.OutputEncoding = System.Text.Encoding.UTF8;

PrintHeader();

// Example 1: Basic Observable and Observer
RunExample1();

// Example 2: Observable with Error Handling
RunExample2();

// Example 3: Subject (Producer and Consumer)
RunExample3();

// Example 4: LINQ Operators on Observable
RunExample4();

PrintFooter();

void RunExample1()
{
    Console.WriteLine("\n=== EXAMPLE 1: Basic Observable ===");
    
    // Create an observable that emits 3 values
    var observable = Observable.Range(1, 3);
    
    // Subscribe with callbacks
    observable.Subscribe(
        onNext: value => Console.WriteLine($"  Value: {value}"),
        onError: error => Console.WriteLine($"  Error: {error}"),
        onCompleted: () => Console.WriteLine("  Completed!")
    );
}

void RunExample2()
{
    Console.WriteLine("\n=== EXAMPLE 2: Observable with Error Handling ===");
    
    // Create observable that throws after 2 values
    var observable = Observable.Create<int>(observer =>
    {
        observer.OnNext(1);
        observer.OnNext(2);
        observer.OnError(new Exception("Something went wrong!"));
        return () => { };
    });
    
    Console.WriteLine("  Observer 1:");
    observable.Subscribe(
        onNext: value => Console.WriteLine($"    Value: {value}"),
        onError: error => Console.WriteLine($"    Error caught: {error.Message}"),
        onCompleted: () => Console.WriteLine("    Completed")
    );
    
    Console.WriteLine("  Observer 2:");
    observable.Subscribe(
        onNext: value => Console.WriteLine($"    Value: {value}"),
        onError: error => Console.WriteLine($"    Error caught: {error.Message}")
    );
}

void RunExample3()
{
    Console.WriteLine("\n=== EXAMPLE 3: Subject (Hot Observable) ===");
    
    // Subject acts as both observer and observable
    var subject = new Subject<string>();
    
    Console.WriteLine("  Subscriber 1 subscribed");
    subject.Subscribe(value => Console.WriteLine($"    Sub1: {value}"));
    
    subject.OnNext("First message");
    subject.OnNext("Second message");
    
    Console.WriteLine("  Subscriber 2 subscribed (late)");
    subject.Subscribe(value => Console.WriteLine($"    Sub2: {value}"));
    
    subject.OnNext("Third message");
    subject.OnCompleted();
}

void RunExample4()
{
    Console.WriteLine("\n=== EXAMPLE 4: LINQ Operators on Observable ===");
    
    // Create sequence and apply LINQ operators
    Observable.Range(1, 10)
        .Where(x => x % 2 == 0)           // Filter even numbers
        .Select(x => x * 2)               // Transform: multiply by 2
        .Take(3)                          // Take first 3 results
        .Subscribe(
            onNext: value => Console.WriteLine($"  Result: {value}"),
            onCompleted: () => Console.WriteLine("  Stream completed")
        );
    
    Console.WriteLine("\n  Throttle example (interval):");
    Observable.Interval(TimeSpan.FromMilliseconds(100))
        .Take(5)
        .Subscribe(
            onNext: x => Console.WriteLine($"    Tick {x}"),
            onCompleted: () => Console.WriteLine("    Interval completed")
        );
    
    // Wait for interval to complete
    System.Threading.Thread.Sleep(1000);
}

void PrintHeader()
{
    Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
    Console.WriteLine("║   REACTIVE EXTENSIONS (Rx)                                        ║");
    Console.WriteLine("║   IObservable, Subjects, LINQ for Events                          ║");
    Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
}

void PrintFooter()
{
    Console.WriteLine("\n╔════════════════════════════════════════════════════════════════════╗");
    Console.WriteLine("║   ✓ Example Completed                                              ║");
    Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
}
