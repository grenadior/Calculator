 
using System;

namespace Utils
{
    public class QueryUtils
    {
        public static string GetGroupedFabricatedCargoQuery()
        {
            const string query = "CREATE TABLE #FabricatedCargoTbl(" +
                                 "[Номер документа] VARCHAR(20) ," +
                                 "[Дата отправления] VARCHAR(20)," +
                                 "[Номер вагона] VARCHAR(20)," +
                                 "[Номер контейнера] VARCHAR(20), " +
                                 "[Count] INT" +
                                 ")" +
                                 "insert into #FabricatedCargoTbl " +
                                 "select  [Номер документа], [Дата отправления],  [Номер вагона], [Номер контейнера]," +
                                 "COUNT(*)" +
                                 "FROM {0} " +
                                 "{1}" +
                                 "GROUP BY [Номер документа], " +
                                 "[Дата отправления], " +
                                 "[Номер вагона]," +
                                 "[Номер контейнера] ";
            return query;
        }
//  " Count( distinct case when gr.[Номер вагона] <> '00000000000' then gr.[Номер вагона] end) as [Количество вагонов], " +
//----  SUM( Case When fb.Count > 1 Then fb.[Объем перевозок (тн)] ELSE gr.[Объем перевозок (тн)] END) as [Объем перевозок (тн)], " +
        public static string GetMainGroupedQuery()
        {
            const string query = "SELECT t.* from (" +
                                 " SELECT Row_number() OVER (ORDER BY (SELECT 1)) AS  ID, {0}," +
                                 " Count(*) over() as CountRows" +
                                 " FROM {1} gr" +
                                 " LEFT JOIN #FabricatedCargoTbl fb ON " +
                                 " fb.[Номер документа] = gr.[Номер документа] AND" +
                                 " fb.[Дата отправления] = gr.[Дата отправления] AND" +
                                 " fb.[Номер вагона] = gr.[Номер вагона] AND" +
                                 " fb.[Номер контейнера] = gr.[Номер контейнера]" +
                                 /*Where*/ " {2} " +
                                 " GROUP BY {3}" +
                                 //Order By
                                 //" {4} " +
                                 " ) as t";
            return query;
        }

        public static string Prefix = "t.";
        public static string PrefixGr = "gr.";
        public static string DropTempTableQuery =  " drop table #FabricatedCargoTbl";
    }
}
