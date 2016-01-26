using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using NUnit.Framework;
using SqlChangeDataLog.Triggers;

namespace SqlChangeDataLog.Tests
{
    public class TriggerTextBuilderTests
    {
        protected Trigger CreateTrigger(string operation)
        {
            return new Trigger()
            {
                Columns = new[] { "Id", "Name" },
                Operation = operation,
                LogTableName = "ChangeLog",
                TableName = "TestTable",
                ExtendedLogic = @"IF (@@RowCount > 1) RETURN"
            };
        }

        protected string BuildText(string operation, bool compositeId = false)
        {

            var keys = new List<string> {"Id"};
            if (compositeId)
            {
                keys.Add("Name");
            }

            return new TriggerTextBuilder(CreateTrigger(operation), keys ,"Some_ChangeLog").BuildTriggerText();
        }
        
        protected bool ExistsBrackets(string text)
        {
            return new Regex("[{}]").IsMatch(text);
        }

        [Test]
        public void Check_SqlTemplate_Complete_For_Insert()
        {
            var text = BuildText("INSERT");
            Console.Write(text);
            Assert.True(!ExistsBrackets(text));
        }
        [Test]
        public void Check_SqlTemplate_Complete_For_Update()
        {
            var text = BuildText("update");
            Console.Write(text);
            Assert.True(!ExistsBrackets(text));

        }
        [Test]
        public void Check_SqlTemplate_Complete_For_Delete()
        {
            var text = BuildText("DeLeTe");
            Console.Write(text);
            Assert.True(!ExistsBrackets(text));
        }

        [Test]
        public void Check_SqlTemplate_Complete_For_CompositeId_Insert()
        {
            var text = BuildText("INSERT",true);
            Console.Write(text);
            Assert.True(!ExistsBrackets(text));
        }
        [Test]
        public void Check_SqlTemplate_Complete_For_CompositeId_Update()
        {
            var text = BuildText("update",true);
            Console.Write(text);
            Assert.True(!ExistsBrackets(text));

        }
        [Test]
        public void Check_SqlTemplate_Complete_For_CompositeId_Delete()
        {
            var text = BuildText("DeLeTe",true);
            Console.Write(text);
            Assert.True(!ExistsBrackets(text));
        }

    }
}
