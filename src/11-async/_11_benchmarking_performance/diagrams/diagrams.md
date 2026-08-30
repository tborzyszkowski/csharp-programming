# Diagramy: Benchmarking Performance

## Diagram 1: Sync vs Async Timeline

```mermaid
timeline
    title Sync vs Async Execution Timeline (10 operations)
    
    Sync Approach: Thread blocked 100ms : Thread blocked 100ms : Thread blocked 100ms : ...
    : Total: 1000ms
    
    Async Approach: Concurrent 100ms : Concurrent 100ms : Concurrent 100ms : ...
    : Total: ~100ms
```

## Diagram 2: Throughput Comparison

```mermaid
graph LR
    A["Requests per Second"]
    
    A --> B["Sync Approach<br/>10 req/s"]
    A --> C["Async Approach<br/>100 req/s<br/>(10x better)"]
    
    style B fill:#ff6b6b
    style C fill:#51cf66
```

## Diagram 3: Memory Allocation Pattern

```mermaid
graph TD
    A["Operation Execution"]
    
    A --> B["Sync: Few allocations<br/>Simple control flow"]
    A --> C["Async: More allocations<br/>Task objects, state machine"]
    
    B --> D["Lower memory overhead"]
    C --> E["Higher memory usage<br/>Better resource utilization"]
    
    style D fill:#51cf66
    style E fill:#ffd93d
```

## Diagram 4: Benchmark Results Interpretation

```mermaid
graph LR
    A["Benchmark Results"]
    
    A --> B["Mean ± StdDev<br/>Consistency"]
    A --> C["Min/Max<br/>Range"]
    A --> D["Ratio<br/>Improvement"]
    A --> E["Memory<br/>Allocations"]
    
    style A fill:#e8f5e9
    style B fill:#c8e6c9
    style C fill:#c8e6c9
    style D fill:#c8e6c9
    style E fill:#c8e6c9
```

## Diagram 5: Benchmark Workflow

```mermaid
sequenceDiagram
    participant Dev as Developer
    participant Code as Codebase
    participant Bench as Benchmark
    participant Results as Results
    
    Dev->>Code: Identify bottleneck
    Dev->>Bench: Write baseline benchmark
    Bench->>Results: Run & collect metrics
    Results->>Dev: Save results (baseline)
    
    Dev->>Code: Implement optimization
    Dev->>Bench: Run benchmark again
    Bench->>Results: Collect new metrics
    Results->>Results: Compare vs baseline
    
    alt If improved
        Results->>Dev: ✓ Commit changes
    else If regressed
        Results->>Dev: ✗ Iterate/revert
    end
```

## Diagram 6: Stopwatch Usage Pattern

```mermaid
stateDiagram-v2
    [*] --> Create: new Stopwatch()
    Create --> Start: .Start()
    Start --> Running: Measuring...
    Running --> Stop: .Stop()
    Stop --> Result: Get ElapsedMilliseconds
    Result --> [*]
    
    note right of Running
        Thread running code
        to be measured
    end note
```

## Diagram 7: I/O Performance Improvement

```mermaid
graph LR
    A["10 File Operations"]
    
    A --> B["Sync<br/>Sequential<br/>1000ms"]
    A --> C["Async<br/>Concurrent<br/>100ms"]
    
    C --> D["10x<br/>Improvement"]
    
    style B fill:#ff6b6b
    style C fill:#51cf66
    style D fill:#ffd700
```

## Diagram 8: Resource Utilization

```mermaid
graph TD
    A["Async Operations"]
    
    A --> B["Thread Pool"]
    A --> C["I/O Completion Ports"]
    A --> D["Non-blocking Waits"]
    
    B --> E["Efficient thread usage<br/>Few threads for many tasks"]
    C --> F["OS handles I/O<br/>Application doesn't block"]
    D --> G["Thread available<br/>for other work"]
    
    style E fill:#51cf66
    style F fill:#51cf66
    style G fill:#51cf66
```
