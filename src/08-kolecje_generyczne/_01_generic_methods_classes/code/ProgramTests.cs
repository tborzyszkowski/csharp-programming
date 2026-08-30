using Xunit;
using GenericMethods;

namespace GenericMethods.Tests
{
    public class GenericMethodsTests
    {
        [Fact]
        public void GetFirst_WithIntArray_ReturnsFirstElement()
        {
            // Arrange
            int[] array = { 1, 2, 3, 4, 5 };
            
            // Act
            int result = GenericMethods.GetFirst(array);
            
            // Assert
            Assert.Equal(1, result);
        }
        
        [Fact]
        public void GetFirst_WithStringArray_ReturnsFirstElement()
        {
            // Arrange
            string[] array = { "Alice", "Bob", "Charlie" };
            
            // Act
            string result = GenericMethods.GetFirst(array);
            
            // Assert
            Assert.Equal("Alice", result);
        }
        
        [Fact]
        public void GetFirst_WithEmptyArray_ReturnsDefault()
        {
            // Arrange
            int[] array = { };
            
            // Act
            int result = GenericMethods.GetFirst(array);
            
            // Assert
            Assert.Equal(default(int), result);
        }
        
        [Fact]
        public void Swap_SwapsValues()
        {
            // Arrange
            int a = 10;
            int b = 20;
            
            // Act
            GenericMethods.Swap(ref a, ref b);
            
            // Assert
            Assert.Equal(20, a);
            Assert.Equal(10, b);
        }
        
        [Fact]
        public void FindIndex_WithExistingElement_ReturnsIndex()
        {
            // Arrange
            int[] array = { 10, 20, 30, 40, 50 };
            
            // Act
            int index = GenericMethods.FindIndex(array, 30);
            
            // Assert
            Assert.Equal(2, index);
        }
        
        [Fact]
        public void FindIndex_WithNonExistingElement_ReturnsMinusOne()
        {
            // Arrange
            int[] array = { 10, 20, 30, 40, 50 };
            
            // Act
            int index = GenericMethods.FindIndex(array, 99);
            
            // Assert
            Assert.Equal(-1, index);
        }
    }
    
    public class StackTests
    {
        [Fact]
        public void Push_AddElement_IncrementsCount()
        {
            // Arrange
            Stack<int> stack = new();
            
            // Act
            stack.Push(1);
            stack.Push(2);
            stack.Push(3);
            
            // Assert
            Assert.Equal(3, stack.Count);
        }
        
        [Fact]
        public void Pop_RemovesElement_ReturnsElement()
        {
            // Arrange
            Stack<int> stack = new();
            stack.Push(1);
            stack.Push(2);
            stack.Push(3);
            
            // Act
            int result = stack.Pop();
            
            // Assert
            Assert.Equal(3, result);
            Assert.Equal(2, stack.Count);
        }
        
        [Fact]
        public void Pop_EmptyStack_ThrowsException()
        {
            // Arrange
            Stack<int> stack = new();
            
            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => stack.Pop());
        }
        
        [Fact]
        public void Peek_ReturnsTopElement_DoesNotRemove()
        {
            // Arrange
            Stack<int> stack = new();
            stack.Push(1);
            stack.Push(2);
            
            // Act
            int result = stack.Peek();
            
            // Assert
            Assert.Equal(2, result);
            Assert.Equal(2, stack.Count);
        }
        
        [Fact]
        public void IsEmpty_WithElements_ReturnsFalse()
        {
            // Arrange
            Stack<int> stack = new();
            stack.Push(1);
            
            // Act & Assert
            Assert.False(stack.IsEmpty);
        }
        
        [Fact]
        public void IsEmpty_WithNoElements_ReturnsTrue()
        {
            // Arrange
            Stack<int> stack = new();
            
            // Act & Assert
            Assert.True(stack.IsEmpty);
        }
    }
    
    public class RepositoryTests
    {
        [Fact]
        public void Add_AddsElement_IncreasesCount()
        {
            // Arrange
            Repository<Dog> repo = new();
            var dog = new Dog("Buddy", 5);
            
            // Act
            repo.Add(dog);
            
            // Assert
            Assert.Equal(1, repo.Count);
        }
        
        [Fact]
        public void Add_NullElement_ThrowsException()
        {
            // Arrange
            Repository<Dog> repo = new();
            
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => repo.Add(null!));
        }
        
        [Fact]
        public void Get_WithMatchingPredicate_ReturnsElement()
        {
            // Arrange
            Repository<Dog> repo = new();
            var dog1 = new Dog("Buddy", 5);
            var dog2 = new Dog("Max", 3);
            repo.Add(dog1);
            repo.Add(dog2);
            
            // Act
            var result = repo.Get(d => d.Name == "Max");
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal("Max", result.Name);
        }
    }
    
    public class ContainerTests
    {
        [Fact]
        public void Map_TransformsValue()
        {
            // Arrange
            var container = new Container<int>(5);
            
            // Act
            var result = container.Map(x => x * 2);
            
            // Assert
            Assert.Equal(10, result.Value);
        }
        
        [Fact]
        public void Map_CanChain()
        {
            // Arrange
            var container = new Container<int>(5);
            
            // Act
            var result = container
                .Map(x => x * 2)
                .Map(x => x + 10);
            
            // Assert
            Assert.Equal(20, result.Value);
        }
        
        [Fact]
        public void Filter_WithTruePredicate_ReturnsContainer()
        {
            // Arrange
            var container = new Container<int>(50);
            
            // Act
            var result = container.Filter(x => x > 40);
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal(50, result.Value);
        }
        
        [Fact]
        public void Filter_WithFalsePredicate_ReturnsNull()
        {
            // Arrange
            var container = new Container<int>(30);
            
            // Act
            var result = container.Filter(x => x > 40);
            
            // Assert
            Assert.Null(result);
        }
    }
}
