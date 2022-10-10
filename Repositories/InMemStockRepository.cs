namespace GSMS.Repositories
{
    public class InMemStockRepository : IStockRepository
    {
        private IJson json;
        public InMemStockRepository(IJson Json)
        {
            json = Json;
        }

        // Updations
        public bool increaseQuantityOfItem(string itemName, int quantity = 1)
        {
            if (!checkContainsItem(itemName))
            {
                return false;
            }
            var data = json.readFromJson();
            data.stock[itemName] = data.stock[itemName] + quantity;
            json.writeToJson(data);
            return true;
        }

        public bool decreaseQuantityOfItem(string itemName, int quantity = 1)
        {
            if (!checkContainsItem(itemName))
            {
                return false;
            }
            var data = json.readFromJson();
            int updatedQuantity = data.stock[itemName] - quantity;
            if (updatedQuantity <= 0)
            {
                data.stock.Remove(itemName);
            }
            else data.stock[itemName] = updatedQuantity;

            json.writeToJson(data);
            return true;
        }

        public void updateQuantityOfItem(string itemName, int quantity)
        {
            if (!checkContainsItem(itemName))
            {
                addItem(itemName, quantity);
                return;
            }
            var data = json.readFromJson();
            if (quantity <= 0)
            {
                data.stock.Remove(itemName);
            }
            else data.stock[itemName] = quantity;
            json.writeToJson(data);
        }

        public bool addItem(string itemName, int quantity = 1)
        {
            if (checkContainsItem(itemName))
            {
                return false;
            }
            var data = json.readFromJson();
            if (quantity <= 0)
            {
                return false;
            }
            data.stock[itemName] = quantity;
            json.writeToJson(data);
            return true;
        }

        public bool deleteItem(string itemName)
        {
            if (!checkContainsItem(itemName))
            {
                return false;
            }
            var data = json.readFromJson();
            data.stock.Remove(itemName);
            json.writeToJson(data);
            return true;
        }

        // Getters
        public int getQuantityOfAnItem(string itemName)
        {
            if (!checkContainsItem(itemName))
            {
                return -1;
            }

            var data = json.readFromJson();
            return data.stock[itemName];
        }

        public Dictionary<string, int> getQuantities()
        {
            var data = json.readFromJson();
            return data.stock;
        }

        public bool checkContainsItem(string itemName)
        {
            var data = json.readFromJson();
            return data.stock.ContainsKey(itemName);
        }

        public Dictionary<string, int> findItemsWhereQuantityIsGreaterThan(int quantity)
        {
            var data = json.readFromJson();

            Dictionary<string, int> filteredItems = data.stock.Where(item => item.Value > quantity).ToDictionary(item => item.Key, item => item.Value);
            return filteredItems;
        }

        public Dictionary<string, int> findItemsWhereQuantityIsLessThan(int quantity)
        {
            var data = json.readFromJson();

            Dictionary<string, int> filteredItems = data.stock.Where(item => item.Value < quantity).ToDictionary(item => item.Key, item => item.Value);
            return filteredItems;
        }

        public Dictionary<string, int> findItemsWhereQuantityIsEqualTo(int quantity)
        {
            var data = json.readFromJson();

            Dictionary<string, int> filteredItems = data.stock.Where(item => item.Value == quantity).ToDictionary(item => item.Key, item => item.Value);
            return filteredItems;
        }
    }
}