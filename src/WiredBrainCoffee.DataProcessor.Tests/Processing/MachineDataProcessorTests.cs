using WiredBrainCoffee.DataProcessor.Data;
using WiredBrainCoffee.DataProcessor.Model;

namespace WiredBrainCoffee.DataProcessor.Processing; 
public class MachineDataProcessorTests {

    [Fact]
    public void ShouldSaveCountPerCoffeeType() {
        // Arrange
        var coffeeCountStore = new FakeCoffeeCountStore();
        var machineDataProcessor = new MachineDataProcessor(coffeeCountStore);
        var items = new[] {
            new MachineDataItem("Cappuccino", new DateTime(2022,10,27,8,0,0)),
            new MachineDataItem("Cappuccino", new DateTime(2022,10,27,9,0,0)),
            new MachineDataItem("Espresso", new DateTime(2022,10,27,10,0,0))
        };

        // Act
        machineDataProcessor.ProcessItems(items);

        // Assert
        Assert.Equal(2, coffeeCountStore.SaveItems.Count);

        var item = coffeeCountStore.SaveItems[0];
        Assert.Equal("Cappuccino", item.CoffeeType);
        Assert.Equal(2, item.Count);

        item = coffeeCountStore.SaveItems[1];
        Assert.Equal("Espresso", item.CoffeeType);
        Assert.Equal(1, item.Count);
    }
}

public class FakeCoffeeCountStore : ICoffeeCountStore {

    public List<CoffeeCountItem> SaveItems { get; } = [];
    public void Save(CoffeeCountItem item) {
        SaveItems.Add(item);
    }
}
