using System.Data;
using System.Linq;
using NUnit.Framework;
using SqlChangeDataLog.Commands;
using SqlChangeDataLog.Dtos;
using SqlChangeDataLog.Extensions;
using SqlChangeDataLog.Queries;
using SqlChangeDataLog.QueryObjects;

namespace SqlChangeDataLog.Tests.DataBase
{
    public class DataBaseTests : TestFixtureBase
    {
        [Test]
        public void ConnectionTest()
        {
                Assert.AreEqual(Connection.State, ConnectionState.Open);
        }

        [Test]
        public void TableDtoTest()
        {
            var dto = new SelectTableDto(Connection, "Entity").Query();
            Assert.IsNull(dto.Insert);
            Assert.IsNull(dto.Update);
            Assert.IsNull(dto.Delete);

            dto.Insert = CreateTrigger("Insert");
            new SaveTable(Connection,dto).Execute();
            var logTriggers1 = Connection.Query<TriggerDto>(new SelectLogTrigger().ByTableName("Entity"));
            Assert.AreEqual(logTriggers1.Count(), 1);

            dto.Update = CreateTrigger("Update");
            dto.Delete = CreateTrigger("Delete");
            new SaveTable(Connection, dto).Execute();
            var logTriggers3 = Connection.Query<TriggerDto>(new SelectLogTrigger().ByTableName("Entity"));
            Assert.AreEqual(logTriggers3.Count(), 3);
            
            dto.Insert = null;
            dto.Update = null;
            dto.Delete = null;
            new SaveTable(Connection, dto).Execute();
            var logTriggers0 = Connection.Query<TriggerDto>(new SelectLogTrigger().ByTableName("Entity"));
            Assert.AreEqual(logTriggers0.Count(), 0);
        }





    }
}
