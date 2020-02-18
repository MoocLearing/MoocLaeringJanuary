using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using Mooc.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mooc.Data.Mongo
{
    public class MongoDBHelper
    {
        private static string mongoConnString = ConvertHelper.GetConfigValue("MongoDBServer");
        private static string mongoDBName = ConvertHelper.GetConfigValue("MongoDBName");
        private static IMongoDatabase mongoDataBase = null;

        private static readonly object lockHelper = new object();

        public static IMongoDatabase GetDatabaseName()
        {
            if (mongoDataBase == null)
            {
                lock (lockHelper)
                {
                    if (mongoDataBase == null)
                    {
                        var client = new MongoClient(mongoConnString);
                        mongoDataBase = client.GetDatabase(mongoDBName);
                    }
                }
            }
            return mongoDataBase;
        }

        public static GridFSBucket GetGridFSBucket()
        {
            var client = GetDatabaseName();
            var bucket = new GridFSBucket(client, new GridFSBucketOptions
            {
                BucketName = "my_bucket",         //设置根节点名
                ChunkSizeBytes = 1024 * 1024,   //设置块的大小为1M
                WriteConcern = WriteConcern.WMajority,     //写入确认级别为majority
                ReadPreference = ReadPreference.Secondary  //优先从从节点读取
            });
            return bucket;
        }

        public static string Upload(string filename, byte[] byties)
        {
            try
            {
                if (string.IsNullOrEmpty(filename)||byties==null||byties.Length==0) return null;

                GridFSBucket bucket = GetGridFSBucket();
                var objectId = bucket.UploadFromBytes(filename, byties);
                return objectId.ToString();
            }
            catch (Exception e)
            {

                throw (e);
            }

        }

        public static byte[] down(string filename)
        {
            try
            {
                if (string.IsNullOrEmpty(filename)) return null;

                GridFSBucket bucket = GetGridFSBucket();
                var filter = Builders<GridFSFileInfo>.Filter.Eq(x => x.Filename, filename);
                var file = bucket.Find(filter).FirstOrDefault();
                if (file == null)
                    return null;

                var bytes = bucket.DownloadAsBytes(file.Id);
                return bytes;
            }
            catch (Exception e)
            {

                throw (e);
            }

        }

    }


}
