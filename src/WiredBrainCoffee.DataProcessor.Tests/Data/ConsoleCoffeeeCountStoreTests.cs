using WiredBrainCoffee.DataProcessor.Model;

namespace WiredBrainCoffee.DataProcessor.Data; 
public class ConsoleCoffeeeCountStoreTests {

    [Fact]
    public void ShouldWriteOutputToConsole() {
        // Arrange
        var item = new CoffeeCountItem("Cappuccino", 5);
        var stringWriter = new StringWriter();
        var consoleCoffeeeCountStore = new ConsoleCoffeeCountStore(stringWriter);

        // Act
        consoleCoffeeeCountStore.Save(item);

        // Assert
        var result = stringWriter.ToString();
        Assert.Equal($"{item.CoffeeType}:{item.Count}{Environment.NewLine}", result);
    }
}
