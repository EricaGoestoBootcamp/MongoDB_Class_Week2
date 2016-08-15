using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDB_deletePractice
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
            Console.WriteLine();
            Console.WriteLine("Press Enter");
            Console.ReadLine();
         
        }
        static async Task MainAsync(string[] args)
        {
            var client = new MongoClient();
            var db = client.GetDatabase("test");
            var col = db.GetCollection<Widget>("widgets");
            await db.DropCollectionAsync("widgets");
            var docs = Enumerable.Range(0, 10).Select(i => new Widget { Id = i, X = i });
            await col.InsertManyAsync(docs);

            //here are the deletes. this will delete all X greater than 5.
            var result = await col.DeleteManyAsync(x => x.X > 5);

            await col.Find(new BsonDocument())
                .ForEachAsync(x => Console.WriteLine(x));

        }

        private class Widget
        {
            public int Id { get; set; }

            [BsonElement("x")]
            public int X { get; set; }

            public override string ToString()
            {
                  return string.Format("Id: {0}, X: {1}", Id, X);
            }
        }
    }
}
