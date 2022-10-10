using System.Collections.Generic;
namespace GSMS {

    public class JsonData {
            public Dictionary<string, int> stock;
            public Dictionary<string, Dictionary<string, int>> recipes;

            public JsonData(){
                stock = new Dictionary<string, int>();
                recipes = new Dictionary<string, Dictionary<string, int>>();
            }
    }

}