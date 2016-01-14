using System.Data;
using System.Linq;
using NUnit.Framework;
using SqlChangeDataLog.Commands;
using SqlChangeDataLog.Dtos;
using SqlChangeDataLog.Extensions;
using SqlChangeDataLog.Queries;
using SqlChangeDataLog.QueryObjects;
using SqlChangeDataLog.Tests.Queries;

namespace SqlChangeDataLog.Tests.DataBase
{
    public class DataBaseTests : TestFixtureBase
    {
        [Test]
        public void ConnectionTest()
        {
            Assert.AreEqual(Connection.State, ConnectionState.Open);
        }
        
        protected int TriggerCount
        {
            get { return Connection.Query<TriggerDto>(new SelectLogTrigger().ByTableName("Entity")).Count(); }
        }

        protected int LogRecordsCount
        {
            get { return Connection.Query<ChangeLogDto>(new SelectChangeLog().All(LogTableName)).Count(); }
        }

        protected TableDto Dto
        {
            get { return new SelectTableDto(Connection, "Entity").Query(); }
        }


        [Test]
        public void Not_Logging_Table_Must_Not_Contain_Log_Triggers()
        {
            var tableDto = Dto;
            Assert.IsNull(tableDto.Insert);
            Assert.IsNull(tableDto.Update);
            Assert.IsNull(tableDto.Delete);
        }


        [Test]
        public void Logging_Table_Must_Contain_Log_Triggers()
        {
            var tableDto = Dto;
            tableDto.Insert = CreateTrigger("Insert");
            new SaveTable(Connection, tableDto).Execute();
            
            Assert.AreEqual(TriggerCount, 1);
            tableDto.Update = CreateTrigger("Update");
            tableDto.Delete = CreateTrigger("Delete");
            new SaveTable(Connection, tableDto).Execute();
            Assert.AreEqual(TriggerCount, 3);
        }

        [Test]
        public void Add_Remove_Log_Triggers_Test()
        {
            var tableDto = Dto;
            tableDto.Insert = CreateTrigger("Insert");
            tableDto.Update = CreateTrigger("Update");
            tableDto.Delete = CreateTrigger("Delete");
            new SaveTable(Connection, tableDto).Execute();
            Assert.AreEqual(TriggerCount, 3);
            
            tableDto.Insert = null;
            tableDto.Update = null;
            tableDto.Delete = null;
            new SaveTable(Connection, tableDto).Execute();
            Assert.AreEqual(TriggerCount, 0);
        }


        [Test]
        public void InsertTest()
        {
            var tableDto = Dto;
            tableDto.Insert = CreateTrigger("Insert");
            new SaveTable(Connection, tableDto).Execute();

            var entity = InsertEntity("TestName");
            
            Assert.AreEqual(entity.Id, 1);
            Assert.AreEqual(LogRecordsCount, 1);
        }

        [Test]
        public void UpdateTest()
        {
            var tableDto = Dto;
            tableDto.Update = CreateTrigger("Update");
            new SaveTable(Connection, tableDto).Execute();

            var entity = InsertEntity("TestName");
            Assert.AreEqual(LogRecordsCount, 0);

            entity.Name = "ChangeName";
            Connection.Execute(new UpdateEntity().Query(entity));
            Assert.AreEqual(LogRecordsCount, 1);
        }


        [Test]
        public void DeleteTest()
        {
            var tableDto = Dto;
            tableDto.Insert = CreateTrigger("Delete");
            new SaveTable(Connection, tableDto).Execute();

            var entity = InsertEntity("TestName");
            Assert.AreEqual(LogRecordsCount, 0);

            Connection.Execute(new DeleteEntity().ById(entity.Id));
            Assert.AreEqual(LogRecordsCount, 1);
        }

        [Test]
        public void InsertMultipleTest()
        {
            var tableDto = Dto;
            tableDto.Insert = CreateTrigger("Insert");
            new SaveTable(Connection, tableDto).Execute();

            InsertMultipleEntity("TestName");
            Assert.AreEqual(LogRecordsCount, 2);
        }

        [Test]
        public void UpdateMultipleTest()
        {
            var tableDto = Dto;
            tableDto.Update = CreateTrigger("Update");
            new SaveTable(Connection, tableDto).Execute();

            InsertEntity("TestName1");
            InsertEntity("TestName2");

            Connection.Execute(new UpdateEntity().NameForAllEntities("NewName"));
            Assert.AreEqual(LogRecordsCount, 2);
        }

        [Test]
        public void DeleteMultipleTest()
        {
            InsertEntity("TestName1");
            InsertEntity("TestName2");

            var query = Dto;
            query.Delete = CreateTrigger("Delete");
            new SaveTable(Connection, query).Execute();

            Connection.Execute(new DeleteEntity().All());
            Assert.AreEqual(LogRecordsCount, 2);
        }
        
    }
}
