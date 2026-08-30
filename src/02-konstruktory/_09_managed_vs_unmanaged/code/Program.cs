using System;
using Xunit;

namespace ManagedUnmanaged;

public class FileResource : IDisposable
{
    private string filepath;
    private bool disposed = false;
    
    public FileResource(string path)
    {
        filepath = path;
        Console.WriteLine($"[CTOR] Opening file: {path}");
    }
    
    public void Read()
    {
        if (disposed)
            throw new ObjectDisposedException(nameof(FileResource));
        
        Console.WriteLine($"[READ] Reading from {filepath}");
    }
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);  // Nie uruchamiaj finalizer
    }
    
    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                Console.WriteLine($"[DISPOSE] Closing managed resources");
            }
            
            Console.WriteLine($"[DISPOSE] Closing unmanaged resources: {filepath}");
            disposed = true;
        }
    }
    
    ~FileResource()
    {
        Dispose(false);  // Backup finalizer
    }
}

public class Program
{
    public static void Main()
    {
        Console.WriteLine("=== TEMAT 9: MANAGED vs UNMANAGED ===\n");
        
        Console.WriteLine("1. USING STATEMENT (C# 8+):");
        using var res1 = new FileResource("file1.txt");
        res1.Read();
        // Tutaj Dispose() się uruchomi
        Console.WriteLine();
        
        Console.WriteLine("2. USING STATEMENT (Tradycyjnie):");
        using (var res2 = new FileResource("file2.txt"))
        {
            res2.Read();
        }  // Tutaj Dispose() się uruchomi
        Console.WriteLine();
        
        Console.WriteLine("3. TRYING TO USE DISPOSED RESOURCE:");
        var res3 = new FileResource("file3.txt");
        res3.Dispose();
        try
        {
            res3.Read();  // Exception!
        }
        catch (ObjectDisposedException ex)
        {
            Console.WriteLine($"[ERROR] {ex.Message}");
        }
    }
}

public class ManagedUnmanagedTests
{
    [Fact]
    public void FileResource_ThrowsAfterDispose()
    {
        var res = new FileResource("test.txt");
        res.Dispose();
        
        Assert.Throws<ObjectDisposedException>(() => res.Read());
    }
    
    [Fact]
    public void FileResource_CanReadBeforeDispose()
    {
        var res = new FileResource("test.txt");
        res.Read();  // OK
        res.Dispose();
    }
}
