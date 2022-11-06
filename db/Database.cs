using MySql.Data.MySqlClient;
using System.Data;


public class Database
{


    public static DataTable query(string srQuery, List<cmdParameterType> lstParameters)
    {
        try
        {
            var dtTable = new DataTable();

            using (var connection = new MySqlConnection("Server=localhost; database=enucuz; UID='root'; password=''"))
            {
                using (MySqlCommand command = new MySqlCommand(srQuery, connection))
                {
                    foreach (var vrPerParameter in lstParameters)
                    {
                        command.Parameters.AddWithValue(vrPerParameter.parameterName, vrPerParameter.objParam);
                    }
                    connection.Open();
                    dtTable.Load(command.ExecuteReader());
                }
            }
            return dtTable;
        }
        catch (Exception e)
        {
            // MessageBox.Show(e.ToString());
            return null;
        }

    }

}

public class cmdParameterType
{

    public cmdParameterType(string _parameterName, object _objParam)
    {
        parameterName = _parameterName;
        objParam = _objParam;
    }



    public string parameterName = "";
    public object objParam;
}
