# Diagramy - Mokowanie Zależności z Moq

## 1. System Under Test z Mockowanymi Zależnościami

```mermaid
graph TD
    Test["OrderServiceTests"] -->|"tworzy"| SUT["OrderService (System Under Test)"]
    Test -->|"tworzy i konfiguruje"| MockRepo["Mock&lt;IOrderRepository&gt;"]
    Test -->|"tworzy i konfiguruje"| MockDiscount["Mock&lt;IDiscountService&gt;"]

    SUT -->|"wstrzyknięte przez konstruktor"| MockRepo
    SUT -->|"wstrzyknięte przez konstruktor"| MockDiscount

    MockRepo -.->|"Setup(...).Returns(...)"| FakeOrder["Order { Amount = 200 }"]
    MockDiscount -.->|"Setup(...).Returns(...)"| FakeDiscount["10 (%)"]
```

## 2. Przepływ Setup → Act → Verify

```mermaid
sequenceDiagram
    participant Test as Test xUnit
    participant Mock as Mock&lt;IEmailSender&gt;
    participant SUT as OrderNotifier

    Test->>Mock: new Mock<IEmailSender>()
    Test->>SUT: new OrderNotifier(mock.Object)
    Test->>SUT: NotifyCustomer("jan@example.com", 150m)
    SUT->>Mock: Send("jan@example.com", "...")
    Test->>Mock: Verify(Send(...), Times.Once)
    Mock-->>Test: ✅ Zweryfikowano wywołanie
```

## 3. Stub vs Mock (Terminologia)

```mermaid
graph LR
    subgraph Stub["Stub (Setup + Returns)"]
        S1["Zwraca ustalone dane"]
        S2["Nie interesuje nas czy była wywołana"]
    end

    subgraph MockRole["Mock (Verify)"]
        M1["Sprawdza CZY i JAK metoda była wywołana"]
        M2["Times.Once, Times.Never, Times.Exactly(n)"]
    end

    Moq["Biblioteka Moq"] --> Stub
    Moq --> MockRole
```
