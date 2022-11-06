using System.Data;
using System.Reflection;

class SQLQueries : Database
{
    public static DataTable select(ScrapeQuery queries)
    {
        List<cmdParameterType> parameters = new List<cmdParameterType>();
        string query = "SELECT * FROM items where 1=1 ";

        query += " and query=@query";
        parameters.Add(new cmdParameterType("@query", queries.query));

        if (queries.website.Count > 0)
        {

            query += " and website in (";
            foreach (string website in queries.website)
            {
                query += "@" + website + ",";
                parameters.Add(new cmdParameterType("@" + website, website));
            }
            query = query.Remove(query.Length - 1) + ")";
        }




        return Database.query(query, parameters);
    }

    public static void insert(List<ScrapeModel> screpeItems, string itemQuery)
    {
        List<cmdParameterType> parameters = new List<cmdParameterType>();
        string query = "insert into items (";
        foreach (PropertyInfo propertyInfo in new ScrapeModel().GetType().GetProperties())
        {
            var propertyName = propertyInfo.Name;
            query += propertyName + ",";

        }
        query += "query) values ";
        for (int i = 0; i < screpeItems.Count; i++)
        {
            ScrapeModel item = screpeItems[i];
            query += "(";
            foreach (PropertyInfo propertyInfo in item.GetType().GetProperties())
            {
                var propertyName = propertyInfo.Name;
                var propertyValue = propertyInfo.GetValue(item);
                query += "@" + propertyName + i + ",";
                parameters.Add(new cmdParameterType("@" + propertyName + i, propertyValue));
            }
            query += "@query" + "),";
        }
        parameters.Add(new cmdParameterType("@query", itemQuery));
        query = query.Remove(query.Length - 1);
        Database.query(query, parameters);
    }
}