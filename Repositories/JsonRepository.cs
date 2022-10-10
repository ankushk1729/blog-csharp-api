using Newtonsoft.Json;

namespace GSMS.Repositories {

    public interface IJson {
        JsonData readFromJson();
        void writeToJson(JsonData data);
    }
    public class Json : IJson
    {
        public JsonData readFromJson(){
            var jsonData = File.ReadAllText(@"C:\Users\AnKumar\Downloads\projects\grocery-stock-mngt-system\stock.json");

            var resultData = JsonConvert.DeserializeObject<JsonData>(jsonData);
            return resultData!;

        }
        public void writeToJson(JsonData data){
            var serializedData = JsonConvert.SerializeObject(data);
            File.WriteAllText(@"C:\Users\AnKumar\Downloads\projects\grocery-stock-mngt-system\stock.json", serializedData);
        }
    }
}