# Diagramy & Ćwiczenia: Tematy 6-10

## Temat 6: Testing

Diagrams:
```mermaid
graph TD
    A["Async Test Method"] --> B["Call async method"]
    B --> C["Await result"]
    C --> D["Assert"]
    D --> E{Passed?}
    E -->|Yes| F["✅ PASS"]
    E -->|No| G["❌ FAIL"]
```

Exercises:
- 🟢 Write 3 async tests with xUnit
- 🟡 Mock async service
- 🔴 Test timeout scenarios

---

## Temat 7: Async LINQ & Channels

Diagrams:
```mermaid
graph LR
    A["Producer"] -->|Write| B["Channel"]
    B -->|Read| C["Consumer"]
    B --> D["Buffer"]
```

Exercises:
- 🟢 Async stream with yield return
- 🟡 Producer-consumer pattern
- 🔴 Pipeline with channels

---

## Temat 8: Dependency Injection

Exercises:
- 🟢 Async service initialization
- 🟡 DI registration with async
- 🔴 Lazy<Task<T>> pattern

---

## Temat 9: Real-World

Exercises:
- 🟢 Simple API client
- 🟡 Concurrent API calls + caching
- 🔴 Error handling + retry logic

---

## Temat 10: Modern C#

Exercises:
- 🟢 ValueTask caching
- 🟡 Async iterators
- 🔴 Zero-allocation patterns
