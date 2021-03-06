﻿using System.Data;
using System.Data.SqlClient;
using System.Linq;
using NUnit.Framework;
using SqlChangeDataLog.QueryObjects;
using SqlChangeDataLog.Tests.Dto;
using SqlChangeDataLog.Tests.Queries;
using SqlChangeDataLog.Extensions;
using SqlChangeDataLog.Triggers;

namespace SqlChangeDataLog.Tests.DataBase
{
    public class TestFixtureBase
    {
        private const string TestSqlString = "data source=.; initial catalog=test; integrated security=SSPI;";
        protected const string LogTableName = "ChangeLog";
        protected IDbConnection Connection;

        private IDbConnection Connect()
        {
            var connection = new SqlConnection(TestSqlString);
            connection.Open();
            return connection;
        }

        [SetUp]
        public void SetUp()
        {
            Connection = Connect();
            Connection.Execute(new CreateEntityTable().Query());
            Connection.Execute(new CreateCompositeIdEntityTable().Query());
            Connection.Execute(new CreateChangeLogTable().Create(LogTableName));
        }

        [TearDown]
        public void TearDown()
        {
            Connection.Execute(new DropEntityTable().Query());
            Connection.Execute(new DropCompositeIdEntityTable().Query());
            Connection.Execute(new DropTable().Query(LogTableName));
            Connection.Dispose();
        }

        protected EntityDto InsertEntity(string name)
        {
                var dto = new EntityDto(name);
                dto.Id = Connection.Query<int>(new InsertEntity().Query(dto)).Single();
                return dto;
        }

        protected EntityDto[] InsertMultipleEntity(string name)
        {
            var dto1 = new EntityDto(name);
            var dto2 = new EntityDto(name);

            using (var connection = Connect())
            {
                using (var transaction = connection.BeginTransaction())
                {
                    dto1.Id = connection.Query<int>(new InsertEntity().Query(dto1), transaction).Single();
                    dto2.Id = connection.Query<int>(new InsertEntity().Query(dto2), transaction).Single();
                    transaction.Commit();
                }
            }

            return new[] {dto1, dto2};
        }

        protected CompositeIdEntityDto[] InsertMultipleCompositeIdEntity()
        {
            var dto1 = new CompositeIdEntityDto(1, "key1");
            var dto2 = new CompositeIdEntityDto(1, "key2");

            using (var connection = Connect())
            {
                using (var transaction = connection.BeginTransaction())
                {
                    connection.Query<int>(new InsertCompositeIdEntity().Query(dto1), transaction);
                    connection.Query<int>(new InsertCompositeIdEntity().Query(dto2), transaction);
                    transaction.Commit();
                }
            }

            return new[] { dto1, dto2 };

        }
        
        protected Trigger CreateEntityTrigger(string Operation)
        {
            return new Trigger()
            {
                Columns = new[] { "Id", "Name" },
                Operation = Operation,
                LogTableName = LogTableName,
                TableName = "Entity"
            };
        }

        protected Trigger CreateCompositeIdEntityTrigger(string Operation)
        {
            return new Trigger()
            {
                Columns = new[] { "Key1", "Key2" },
                Operation = Operation,
                LogTableName = LogTableName,
                TableName = "CompositeIdEntity"
            };
        }

        protected Trigger CreateExtendedLogicRowCountTrigger(string Operation)
        {
            return new Trigger()
            {
                Columns = new[] { "Id", "Name" },
                Operation = Operation,
                LogTableName = LogTableName,
                TableName = "Entity",
                ExtendedLogic = @"IF (@@RowCount > 1) RETURN"
            };
        }

    }
}
