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


        [Test]
        public void TableDtoTest()
        {
            var dto = new SelectTableDto(Connection, "Entity").Query();
            Assert.IsNull(dto.Insert);
            Assert.IsNull(dto.Update);
            Assert.IsNull(dto.Delete);

            dto.Insert = CreateTrigger("Insert");
            new SaveTable(Connection, dto).Execute();
            Assert.AreEqual(TriggerCount, 1);

            dto.Update = CreateTrigger("Update");
            dto.Delete = CreateTrigger("Delete");
            new SaveTable(Connection, dto).Execute();
            Assert.AreEqual(TriggerCount, 3);

            dto.Insert = null;
            dto.Update = null;
            dto.Delete = null;
            new SaveTable(Connection, dto).Execute();
            Assert.AreEqual(TriggerCount, 0);
        }

        [Test]
        public void InsertTest()
        {
            var dto = new SelectTableDto(Connection, "Entity").Query();
            dto.Insert = CreateTrigger("Insert");
            new SaveTable(Connection, dto).Execute();

            var entity = InsertEntity("TestName");
            
            Assert.AreEqual(entity.Id, 1);
            Assert.AreEqual(LogRecordsCount, 1);
        }

        [Test]
        public void UpdateTest()
        {
            var dto = new SelectTableDto(Connection, "Entity").Query();
            dto.Insert = CreateTrigger("Update");
            new SaveTable(Connection, dto).Execute();

            var entity = InsertEntity("TestName");
            Assert.AreEqual(LogRecordsCount, 0);

            entity.Name = "ChangeName";
            Connection.Execute(new UpdateEntity().Query(entity));
            Assert.AreEqual(LogRecordsCount, 1);
        }


        [Test]
        public void DeleteTest()
        {
            var dto = new SelectTableDto(Connection, "Entity").Query();
            dto.Insert = CreateTrigger("Delete");
            new SaveTable(Connection, dto).Execute();

            var entity = InsertEntity("TestName");
            Assert.AreEqual(LogRecordsCount, 0);

            Connection.Execute(new DeleteEntity().ById(entity.Id));
            Assert.AreEqual(LogRecordsCount, 1);
        }

        [Test]
        public void UpdateMultipleTest()
        {
            var dto = new SelectTableDto(Connection, "Entity").Query();
            dto.Insert = CreateTrigger("Update");
            new SaveTable(Connection, dto).Execute();

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

            var dto = new SelectTableDto(Connection, "Entity").Query();
            dto.Insert = CreateTrigger("Delete");
            new SaveTable(Connection, dto).Execute();

            Connection.Execute(new DeleteEntity().All());
            Assert.AreEqual(LogRecordsCount, 2);
        }
        
    }
}
