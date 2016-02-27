namespace SqlChangeDataLog.QueryObjects
{
    public class SelectDescription
    {
        public QueryObject All()
        {
            return new QueryObject(@"
                SELECT 
	                t.name AS TableName,
	                c.name AS ColumnName,
	                ep.[value] AS [Description]
                FROM sys.tables AS t
                inner JOIN sys.columns c ON c.[object_id] = t.[object_id]
                inner JOIN sys.extended_properties AS ep ON t.[object_id] = ep.major_id AND ep.minor_id = c.column_id AND ep.name = 'MS_Description'

                UNION ALL

                SELECT 
	                t.name AS TableName,
	                NULL AS ColumnName,
	                ep.[value] AS [Description]
                FROM sys.tables AS t
                inner JOIN sys.extended_properties AS ep ON t.[object_id] = ep.major_id AND ep.minor_id = 0 AND ep.name = 'MS_Description'
            ");
        }
    }
}
