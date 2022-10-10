namespace GSMS.Repositories
{
    public interface IStockRepository
    {
        bool addItem(string itemName, int quantity = 1);
        bool checkContainsItem(string itemName);
        bool decreaseQuantityOfItem(string itemName, int quantity = 1);
        bool deleteItem(string itemName);
        Dictionary<string, int> findItemsWhereQuantityIsEqualTo(int quantity);
        Dictionary<string, int> findItemsWhereQuantityIsGreaterThan(int quantity);
        Dictionary<string, int> findItemsWhereQuantityIsLessThan(int quantity);
        Dictionary<string, int> getQuantities();
        int getQuantityOfAnItem(string itemName);
        bool increaseQuantityOfItem(string itemName, int quantity = 1);
        void updateQuantityOfItem(string itemName, int quantity);
    }
}