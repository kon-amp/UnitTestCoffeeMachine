using WiredBrainCoffee.DataProcessor.Data;
using WiredBrainCoffee.DataProcessor.Model;

namespace WiredBrainCoffee.DataProcessor.Processing; 
public class MachineDataProcessorTests : IDisposable {
    private readonly FakeCoffeeCountStore _coffeeCountStore;
    private readonly MachineDataProcessor _machineDataProcessor;

    // Constructor runs before each unit test
    public MachineDataProcessorTests()
    {
        _coffeeCountStore = new FakeCoffeeCountStore();
        _machineDataProcessor = new MachineDataProcessor(_coffeeCountStore);
    }

    [Fact]
    public void ShouldSaveCountPerCoffeeType() {
        // Arrange
        var items = new[] {
            new MachineDataItem("Cappuccino", new DateTime(2022,10,27,8,0,0)),
            new MachineDataItem("Cappuccino", new DateTime(2022,10,27,9,0,0)),
            new MachineDataItem("Espresso", new DateTime(2022,10,27,10,0,0))
        };

        // Act
        _machineDataProcessor.ProcessItems(items);

        // Assert
        Assert.Equal(2, _coffeeCountStore.SaveItems.Count);

        var item = _coffeeCountStore.SaveItems[0];
        Assert.Equal("Cappuccino", item.CoffeeType);
        Assert.Equal(2, item.Count);

        item = _coffeeCountStore.SaveItems[1];
        Assert.Equal("Espresso", item.CoffeeType);
        Assert.Equal(1, item.Count);
    }


    [Fact]
    public void ShouldClearPreviousCoffeeCount() {
        // Arrange
        var items = new[] {
            new MachineDataItem("Cappuccino", new DateTime(2022,10,27,8,0,0))
        };

        // Act
        _machineDataProcessor.ProcessItems(items);
        _machineDataProcessor.ProcessItems(items);

        // Assert
        Assert.Equal(2, _coffeeCountStore.SaveItems.Count);
        foreach(var item in _coffeeCountStore.SaveItems) {
            Assert.Equal("Cappuccino", item.CoffeeType);
            Assert.Equal(1, item.Count);
        }
    }

    [Fact]
    public void ShouldIgnoreItemsThatAreNotNewer() {
        // Arrange
        var items = new[] {
            new MachineDataItem("Cappuccino", new DateTime(2022,10,27,8,0,0)),
            new MachineDataItem("Cappuccino", new DateTime(2022,10,27,7,0,0)),  // should be ignore
            new MachineDataItem("Cappuccino", new DateTime(2022,10,27,7,10,0)), // should be ignore
            new MachineDataItem("Cappuccino", new DateTime(2022,10,27,9,0,0)),
            new MachineDataItem("Espresso", new DateTime(2022,10,27,10,0,0)),   // should be ignore
            new MachineDataItem("Espresso", new DateTime(2022,10,27,10,0,0))    // should be ignore
        };

        // Act
        _machineDataProcessor.ProcessItems(items);

        // Assert
        Assert.Equal(2, _coffeeCountStore.SaveItems.Count);

        var item = _coffeeCountStore.SaveItems[0];
        Assert.Equal("Cappuccino", item.CoffeeType);
        Assert.Equal(2, item.Count);

        item = _coffeeCountStore.SaveItems[1];
        Assert.Equal("Espresso", item.CoffeeType);
        Assert.Equal(1, item.Count);
    }

    public void Dispose() {
        // This runs after every test
    }
}

public class FakeCoffeeCountStore : ICoffeeCountStore {

    public List<CoffeeCountItem> SaveItems { get; } = [];
    public void Save(CoffeeCountItem item) {
        SaveItems.Add(item);
    }
}
