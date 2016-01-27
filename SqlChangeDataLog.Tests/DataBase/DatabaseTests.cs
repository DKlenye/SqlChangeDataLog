using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using Dapper;
using NUnit.Framework;
using NUnit.Framework.Internal.Commands;
using SqlChangeDataLog.Commands;
using SqlChangeDataLog.Dtos;
using SqlChangeDataLog.Extensions;
using SqlChangeDataLog.Queries;
using SqlChangeDataLog.QueryObjects;
using SqlChangeDataLog.Tests.Dto;
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

        protected TableDto CompositeIdDto
        {
            get { return new SelectTableDto(Connection, "CompositeIdEntity").Query(); }
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
            tableDto.Insert = CreateEntityTrigger("Insert");
            new SaveTable(Connection, tableDto).Execute();
            
            Assert.AreEqual(TriggerCount, 1);
            tableDto.Update = CreateEntityTrigger("Update");
            tableDto.Delete = CreateEntityTrigger("Delete");
            new SaveTable(Connection, tableDto).Execute();
            Assert.AreEqual(TriggerCount, 3);
        }

        [Test]
        public void Add_Remove_Log_Triggers_Test()
        {
            var tableDto = Dto;
            tableDto.Insert = CreateEntityTrigger("Insert");
            tableDto.Update = CreateEntityTrigger("Update");
            tableDto.Delete = CreateEntityTrigger("Delete");
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
            tableDto.Insert = CreateEntityTrigger("Insert");
            new SaveTable(Connection, tableDto).Execute();

            var entity = InsertEntity("TestName");
            
            Assert.AreEqual(entity.Id, 1);
            Assert.AreEqual(LogRecordsCount, 1);
        }

        [Test]
        public void UpdateTest()
        {
            var tableDto = Dto;
            tableDto.Update = CreateEntityTrigger("Update");
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
            tableDto.Insert = CreateEntityTrigger("Delete");
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
            tableDto.Insert = CreateEntityTrigger("Insert");
            new SaveTable(Connection, tableDto).Execute();

            InsertMultipleEntity("TestName");
            Assert.AreEqual(LogRecordsCount, 2);
        }

        [Test]
        public void UpdateMultipleTest()
        {
            var tableDto = Dto;
            tableDto.Update = CreateEntityTrigger("Update");
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
            query.Delete = CreateEntityTrigger("Delete");
            new SaveTable(Connection, query).Execute();

            Connection.Execute(new DeleteEntity().All());
            Assert.AreEqual(LogRecordsCount, 2);
        }


        [Test]
        public void Extended_Logic_If_RowCount_More_Then_One()
        {
            var tableDto = Dto;
            tableDto.Update = CreateExtendedLogicRowCountTrigger("Update");
            new SaveTable(Connection, tableDto).Execute();

            var entity1 = InsertEntity("TestName1");
            InsertEntity("TestName2");

            Connection.Execute(new UpdateEntity().NameForAllEntities("NewName"));
            Assert.AreEqual(LogRecordsCount, 0);
            
            entity1.Name = "ChangeName";
            Connection.Execute(new UpdateEntity().Query(entity1));
            Assert.AreEqual(LogRecordsCount, 1);
        }

        [Test]
        public void CompositeIdEntityShouldContainsMultipleKeys()
        {
            Assert.AreEqual(CompositeIdDto.KeyColumns.Count(), 2);
        }

        [Test]
        public void InsertCompositeIdTest()
        {
            var tableDto = CompositeIdDto;
            tableDto.Insert = CreateCompositeIdEntityTrigger("Insert");
            new SaveTable(Connection, tableDto).Execute();

            Connection.Execute(new InsertCompositeIdEntity().Query(new CompositeIdEntityDto(1, "key1")));
            Assert.AreEqual(LogRecordsCount, 1);

            var log = Connection.Query<ChangeLogDto>(new SelectChangeLog().All(LogTableName)).First();
            Assert.AreEqual(log.idString.Split(',').Count(), 2);
        }
        [Test]
        public void UpdateCompositeIdTest()
        {
            var tableDto = CompositeIdDto;
            tableDto.Update = CreateCompositeIdEntityTrigger("Update");
            new SaveTable(Connection, tableDto).Execute();

            var entity = new CompositeIdEntityDto(1, "key1");
            Connection.Execute(new InsertCompositeIdEntity().Query(entity));
            Assert.AreEqual(LogRecordsCount, 0);

            entity.Key1 = 11;
            entity.Key2 = "ChangeName";

            Connection.Execute(new UpdateCompositeIdEntity().Key2ForAllEntities("newKey"));
            Assert.AreEqual(LogRecordsCount, 1);

            var log = Connection.Query<ChangeLogDto>(new SelectChangeLog().All(LogTableName)).First();
            Assert.AreEqual(log.idString.Split(',').Count(), 2);
        }

        [Test]
        public void DeleteCompositeIdTest()
        {
            var tableDto = CompositeIdDto;
            tableDto.Insert = CreateCompositeIdEntityTrigger("Delete");
            new SaveTable(Connection, tableDto).Execute();

            var entity = new CompositeIdEntityDto(1, "key1");
            Connection.Execute(new InsertCompositeIdEntity().Query(entity));
            Assert.AreEqual(LogRecordsCount, 0);

            Connection.Execute(new DeleteCompositeIdEntity().Query(entity));
            Assert.AreEqual(LogRecordsCount, 1);

            var log = Connection.Query<ChangeLogDto>(new SelectChangeLog().All(LogTableName)).First();
            Assert.AreEqual(log.idString.Split(',').Count(), 2);
        }

        [Test]
        public void InsertCompsiteIdMultipleTest()
        {
            var tableDto = CompositeIdDto;
            tableDto.Insert = CreateCompositeIdEntityTrigger("Insert");
            new SaveTable(Connection, tableDto).Execute();

            InsertMultipleCompositeIdEntity();
            Assert.AreEqual(LogRecordsCount, 2);
        }

        [Test]
        public void UpdateCompositeIdMultipleTest()
        {
            var tableDto = CompositeIdDto;
            tableDto.Update = CreateCompositeIdEntityTrigger("Update");
            new SaveTable(Connection, tableDto).Execute();

            InsertMultipleCompositeIdEntity();

            Connection.Execute(new UpdateCompositeIdEntity().Key2ForAllEntities("add"));
            Assert.AreEqual(LogRecordsCount, 2);

            var regex = new Regex("<U.+?/>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            var log = Connection.Query<ChangeLogDto>(new SelectChangeLog().All(LogTableName)).First();
            Assert.AreEqual(regex.Matches(log.description).Count, 2);
        }

        [Test]
        public void DeleteCompositeIdMultipleTest()
        {
            var tableDto = CompositeIdDto;
            tableDto.Update = CreateCompositeIdEntityTrigger("Delete");
            new SaveTable(Connection, tableDto).Execute();

            InsertMultipleCompositeIdEntity();
            Connection.Execute(new DeleteCompositeIdEntity().All());
            
            Connection.Execute(new DeleteEntity().All());
            Assert.AreEqual(LogRecordsCount, 2);
        }


    }
}
