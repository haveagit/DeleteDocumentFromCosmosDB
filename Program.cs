using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;

namespace DeleteDocumentFromCosmosDB
{
    class DocumentDbManager
    {
        // DocumentDbManagerに定数定義を追加
        private const string EndpointUrl = "自分のDBのエンドポイント";
        private const string PrimaryKey = "自分のDBのキー";
        private const string DatabaseId = "outDatabase";    //自身のDB名に変更
        private const string CollectionId = "MyCollection"; //自身のコレクションIDに変更

        private DocumentClient client = null;

        public DocumentDbManager()
        {
            this.client =
              new DocumentClient(
                new Uri(DocumentDbManager.EndpointUrl),
                DocumentDbManager.PrimaryKey);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Main よばれたよ");

            var manager = new DocumentDbManager();


            List<Record> records = manager.FindAllRecords();

            // 検索結果出力
            Console.WriteLine(string.Format("{0}件", records.Count));
            foreach (var info in records)
            {
                Console.WriteLine(
                  string.Format("id = {0} delete. ", info.Id));

                manager.DeleteById(info.Id).Wait();
            }

        }

        // DatabaseId、CollectionIdで全件取得するメソッド
        public List<Record> FindAllRecords()
        {
            var query =
              this.client.CreateDocumentQuery<Record>(
                UriFactory.CreateDocumentCollectionUri(
                  DocumentDbManager.DatabaseId, DocumentDbManager.CollectionId));
            var result = query.ToList();

            return result;
        }

        // id指定で1件削除するメソッド
        public async Task<Document> DeleteById(string id)
        {
            var document = await this.client.DeleteDocumentAsync(
              UriFactory.CreateDocumentUri(
                DocumentDbManager.DatabaseId,
                DocumentDbManager.CollectionId,
                id));

            return document;
        }
    }

    class Record
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
    }
}
