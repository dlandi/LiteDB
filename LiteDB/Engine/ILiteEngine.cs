﻿using System;
using System.Collections.Generic;

namespace LiteDB.Engine
{
    public interface ILiteEngine : IDisposable
    {
        int Analyze(string[] collections);
        int Checkpoint();
        long Shrink();
        int Vaccum();

        bool BeginTrans();
        bool Commit();
        bool Rollback();

        IBsonDataReader Query(string collection, QueryDefinition query);

        int Insert(string collection, IEnumerable<BsonDocument> docs, BsonAutoId autoId);
        int Update(string collection, IEnumerable<BsonDocument> docs);
        int UpdateMany(string collection, BsonExpression extend, BsonExpression predicate);
        int Upsert(string collection, IEnumerable<BsonDocument> docs, BsonAutoId autoId);
        int Delete(string collection, IEnumerable<BsonValue> ids);
        int DeleteMany(string collection, BsonExpression predicate);

        bool CreateCollection(string name);
        bool DropCollection(string name);
        bool RenameCollection(string name, string newName);

        bool EnsureIndex(string collection, string name, BsonExpression expression, bool unique);
        bool DropIndex(string collection, string name);

        BsonValue DbParam(string parameterName);
        bool DbParam(string parameterName, BsonValue value);
    }
}