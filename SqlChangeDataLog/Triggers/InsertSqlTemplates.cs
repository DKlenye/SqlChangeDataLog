﻿namespace SqlChangeDataLog.Triggers
{
    public class InsertSqlTemplates:SqlTemplates
    {
        public override string SelectXml()
        {
                return @"SELECT {Columns}
            FROM INSERTED AS I WHERE I.[{PrimaryKey}] = C.[{PrimaryKey}] FOR XML AUTO";
        }

        public override string ChangeType()
        {
            return "I";
        }

        public override string IdFrom()
        {
            return "INSERTED";
        }
    }
}
