using System;
using System.Linq;
using NUnit.Framework;
using SqlChangeDataLog.Commands;
using SqlChangeDataLog.Extensions;
using SqlChangeDataLog.Queries;
using SqlChangeDataLog.QueryObjects;
using SqlChangeDataLog.Tests.Queries;
using SqlChangeDataLog.Triggers;

namespace SqlChangeDataLog.Tests.DataBase
{
    public class Bugs : TestFixtureBase
    {
        private const string Bug1TableName = "FondTransfer";

        [SetUp]
        public void SetUpBugs()
        {
            Connection.Execute(new CreateBug1Table().Query());
        }

        [TearDown]
        public void TearDownBugs()
        {
            Connection.Execute(new DropTable().Query(Bug1TableName));
        }

        /*
         Bug 1: Неверный шаблон выборки колонок (присутствие в имени 'for'). После добавления триггера количество колонок после парсинга текста не совпадает с реальным количеством
         Fix: Не верный шаблон регулярного выражения для выборки колонок. Ошибка присутствует там где есть колонки в имени которой находится for
         */
        [Test]
        public void Bug1_AfterSaveTriggerColumnsInDtoNotAll()
        {
            var tableDto = new SelectTableDto(Connection, Bug1TableName).Query();
            tableDto.Insert = new Trigger()
            {
                Columns = new[] { "IdFondTransfer", "IdFondMoveFrom", "IdFondFrom", "IdFondMoveTo", "IdFondTo" },
                Operation = "Insert",
                LogTableName = LogTableName,
                TableName = Bug1TableName
            };

            new SaveTable(Connection, tableDto).Execute();

            var dto = new SelectTableDto(Connection, Bug1TableName).Query();
            Assert.AreEqual(dto.Insert.Columns.Count(),5);
        }

        [Test]
        public void Bug2_AfterSaveTriggerColumnsInDtoNotAll()
        {
            var tableDto = new SelectTableDto(Connection, Bug1TableName).Query();
            tableDto.Insert = new Trigger()
            {
                Columns = new[] { "IdFondTransfer", "IdFondMoveFrom", "IdFondFrom", "IdFondMoveTo", "IdFondTo" },
                Operation = "Insert",
                LogTableName = LogTableName,
                TableName = Bug1TableName,
                ExtendedLogic = @"
                    IF((Select COUNT(*) FROM shipment_Log Where userAccount = USER AND MessageText LIKE 'Вход в программу(v 3.3.36.492)')=0)	 
	                BEGIN	
	                RAISERROR('Данная версия программы является устаревшей.',16,1);
	                ROLLBACK TRANSACTION
	                RETURN 
	                END;
                --IF ((SELECT GETDATE())>'20160701')	 
                --	BEGIN	
                --	RAISERROR('В связи с проведением деноминации, данная версия программы является устаревшей.  Функции оформления будут не доступны до --9.00',16,1);
                --	ROLLBACK TRANSACTION
                --	RETURN 
                --	END;
                "
                                
            };

            new SaveTable(Connection, tableDto).Execute();
            var dto = new SelectTableDto(Connection, Bug1TableName).Query();
            Assert.AreEqual(dto.Insert.Columns.Count(), 5);
        }

    }
}
