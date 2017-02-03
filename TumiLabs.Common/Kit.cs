using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace TumiLabs.Common
{
//    namespace 149RLP
//{
    //using Microsoft.Win32;
    //using System;
    //using System.Collections;
    //using System.Collections.Generic;
    //using System.Data;
    //using System.Data.OleDb;
    //using System.IO;
    //using System.Linq;
    //using System.Net.Security;
    //using System.Runtime.InteropServices;
    //using System.Runtime.Serialization.Formatters.Soap;
    //using System.Security.Cryptography.X509Certificates;
    //using System.Security.Principal;
    //using System.Text;
    //using System.Windows.Forms;
    //using System.Xml;
    //using System.Xml.Serialization;
    //using Vyapin.DocKIT;
    //using Vyapin.Windows.Application.Logging;
    //using Vyapin.Windows.Application.Schedule;
    //using Vyapin.Windows.Application.Text;
    //using Vyapin.Windows.Application.UI;

    public class Kit
    {
        //private static string 1aagfBZ = string.Empty;
        //private DataTable 1er5ef;
        //private DataTable 1RYpR2Z = new DataTable("Version");
        //public const string 1sxvBC = "The {0} cannot contain any of the following characters:\n\t \\ / : * ? \" < > |";
        //private DataSet 1YGwmWZ;
        //private static IProcessStatusUpdatable 2EOpOgZ;
        //private DataSet 3qwv8Z = new DataSet("Tasks");
        //private static int BdKAqZ = -1;
        //private static string CvCXuZ = string.Empty;
        //private string H7OpV = (MZdp3() + @"\Tasks.XML");
        //private static bool haR8QZ = false;
        //private static string nD3JT = string.Empty;
        //private DataTable qVQlLZ;
        //private static string qWLsU = string.Empty;

        //public 1cbamL()
        //{
        //    this.1RYpR2Z.Columns.Add(new DataColumn("Version", typeof(string), "", MappingType.SimpleContent));
        //    this.1er5ef = new DataTable("Custom");
        //    this.1er5ef.Columns.Add(new DataColumn("LoadTaskStatusAtStartup", typeof(bool), "", MappingType.Attribute));
        //    this.1er5ef.Columns.Add(new DataColumn("MaxTaxonomyRetrieveCount", typeof(int), "", MappingType.Attribute));
        //    this.1er5ef.Columns.Add(new DataColumn("UseDistinuguisedNameForSecurityDistributionGroupInSP2010", typeof(bool), "", MappingType.Attribute));
        //    this.3qwv8Z.Tables.Add(this.1RYpR2Z);
        //    this.3qwv8Z.Tables.Add(this.1er5ef);
        //    if (File.Exists(this.H7OpV))
        //    {
        //        this.3qwv8Z.ReadXml(this.H7OpV);
        //    }
        //    else
        //    {
        //        DataRow row = this.1RYpR2Z.NewRow();
        //        row["Version"] = "4.0.0.0";
        //        this.1RYpR2Z.Rows.Add(row);
        //        this.1RYpR2Z.AcceptChanges();
        //        DataRow row2 = this.1er5ef.NewRow();
        //        row2["LoadTaskStatusAtStartup"] = true;
        //        row2["MaxTaxonomyRetrieveCount"] = 100;
        //        row2["UseDistinuguisedNameForSecurityDistributionGroupInSP2010"] = true;
        //        this.1er5ef.Rows.Add(row2);
        //        this.1er5ef.AcceptChanges();
        //        this.21vp3BZ();
        //    }
        //    this.6NTk1Z();
        //}

        //private static string 1aBwytZ(string text1)
        //{
        //    string startupPath = string.Empty;
        //    try
        //    {
        //        nD3JT = string.Empty;
        //        startupPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), @"Vyapin\DocKIT\9x");
        //        string path = Path.Combine(Application.StartupPath, "Vyapin.DocKIT.InstallState");
        //        if (File.Exists(path))
        //        {
        //            Hashtable hashtable = null;
        //            SoapFormatter formatter = null;
        //            Stream serializationStream = null;
        //            bool flag = false;
        //            try
        //            {
        //                try
        //                {
        //                    serializationStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None);
        //                    formatter = new SoapFormatter();
        //                    hashtable = (Hashtable) formatter.Deserialize(serializationStream);
        //                    if ((hashtable != null) && hashtable.ContainsKey("_reserved_nestedSavedStates"))
        //                    {
        //                        IDictionary[] dictionaryArray = (IDictionary[]) hashtable["_reserved_nestedSavedStates"];
        //                        if (dictionaryArray != null)
        //                        {
        //                            foreach (IDictionary dictionary in dictionaryArray)
        //                            {
        //                                if (dictionary.Contains(text1))
        //                                {
        //                                    startupPath = dictionary[text1].ToString();
        //                                    flag = true;
        //                                    break;
        //                                }
        //                            }
        //                        }
        //                    }
        //                    if (!flag)
        //                    {
        //                        nD3JT = string.Format("The INSTALLSTATE file '{0}' does not contain '{1}' information.", path, text1);
        //                    }
        //                }
        //                catch (Exception exception)
        //                {
        //                    nD3JT = exception.Message;
        //                    startupPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), @"Vyapin\DocKIT\9x");
        //                    startupPath = Application.StartupPath;
        //                }
        //                return startupPath;
        //            }
        //            finally
        //            {
        //                if (serializationStream != null)
        //                {
        //                    serializationStream.Flush();
        //                    serializationStream.Close();
        //                    serializationStream = null;
        //                }
        //                formatter = null;
        //                hashtable = null;
        //            }
        //        }
        //        nD3JT = string.Format("The INSTALLSTATE file '{0}' is not found in the installation folder.", path);
        //        startupPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), @"Vyapin\DocKIT\9x");
        //        return Application.StartupPath;
        //    }
        //    catch (Exception exception2)
        //    {
        //        nD3JT = exception2.Message;
        //        startupPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), @"Vyapin\DocKIT\9x");
        //        return Application.StartupPath;
        //    }
        //}

        //public static bool 1bot7T(string text1)
        //{
        //    return (text1.IndexOfAny(2aXhz9Z()) < 0);
        //}

        //public static void 1d8dVy(IProcessStatusUpdatable updatable1)
        //{
        //    2EOpOgZ = updatable1;
        //}

        //public static bool 1d9Q6lZ(string text1, bool flag1)
        //{
        //    bool flag = false;
        //    try
        //    {
        //        if (Directory.Exists(text1))
        //        {
        //            Directory.Delete(text1, true);
        //        }
        //        flag = true;
        //    }
        //    catch (Exception exception)
        //    {
        //        if (flag1)
        //        {
        //            MessageBox.Show("The following error occurred attempting to delete task history folder:\n" + StringFormat.DoubleQuotes(text1) + "\n\n" + exception.Message, "Deleting Task History...", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        //        }
        //        ErrorLog.ActiveErrorLogWriter.WriteToLog("TaskManager", "DeleteTaskHistoryFolder", new object[] { text1, exception.Message });
        //    }
        //    return flag;
        //}

        //public static bool 1E7N2A(string text1, DateTime time1, DataTable table1)
        //{
        //    bool flag = false;
        //    DataRow row = null;
        //    try
        //    {
        //        row = table1.Rows.Find(new object[] { text1, time1 });
        //        if (row != null)
        //        {
        //            row.Delete();
        //            flag = true;
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        ErrorLog.ActiveErrorLogWriter.WriteToLog("TaskManager", "DeleteTaskHistory", new object[] { text1, time1, exception.Message });
        //    }
        //    finally
        //    {
        //        row = null;
        //        table1.AcceptChanges();
        //        UOiDqZ(table1, 2lmpl() + @"\TaskHistory.xml");
        //    }
        //    return flag;
        //}

        //public static bool 1E7N2A(string text1, string text2, DataTable table1)
        //{
        //    Func<DataRow, bool> predicate = null;
        //    string 27KRV8 = text1;
        //    string 2C5NRYZ = text2;
        //    bool flag = false;
        //    IEnumerable<DataRow> source = null;
        //    try
        //    {
        //        if (predicate == null)
        //        {
        //            <>c__DisplayClass2 class2;
        //            predicate = new Func<DataRow, bool>(class2.1CNp2W);
        //        }
        //        source = table1.Rows.Cast<DataRow>().Where<DataRow>(predicate);
        //        if ((source != null) && (source.Count<DataRow>() > 0))
        //        {
        //            DataRow row = source.First<DataRow>();
        //            table1.Rows.Remove(row);
        //            flag = true;
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        ErrorLog.ActiveErrorLogWriter.WriteToLog("TaskManager", "DeleteTaskHistory", new object[] { 27KRV8, 2C5NRYZ, exception.Message });
        //    }
        //    finally
        //    {
        //        source = null;
        //        table1.AcceptChanges();
        //    }
        //    return flag;
        //}

        //public static bool 1E7N2A(string text1, DateTime time1, DataTable table1, DataRow row1)
        //{
        //    bool flag = false;
        //    try
        //    {
        //        table1.Rows.Remove(row1);
        //        flag = true;
        //    }
        //    catch (Exception exception)
        //    {
        //        ErrorLog.ActiveErrorLogWriter.WriteToLog("TaskManager", "DeleteTaskHistory", new object[] { text1, time1, exception.Message });
        //    }
        //    finally
        //    {
        //        table1.AcceptChanges();
        //    }
        //    return flag;
        //}

        //public static string 1MX9FqZ()
        //{
        //    try
        //    {
        //        nD3JT = string.Empty;
        //        if (File.Exists(tOFRmZ()))
        //        {
        //            DMO10Z.gsv3p("");
        //            if (DMO10Z.1cxWraZ.CommonUserDataPath.EndsWith(@"\Dockit9x", StringComparison.InvariantCultureIgnoreCase))
        //            {
        //                return DMO10Z.1cxWraZ.CommonUserDataPath;
        //            }
        //            return Path.Combine(DMO10Z.1cxWraZ.CommonUserDataPath, "Dockit9x");
        //        }
        //        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), "Dockit9x");
        //    }
        //    catch (Exception exception)
        //    {
        //        nD3JT = exception.Message;
        //        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), "Dockit9x");
        //    }
        //}

        //public static ScheduleTaskSettings 1RCBD9Z(string text1)
        //{
        //    ScheduleTaskSettings settings = null;
        //    if (File.Exists(text1))
        //    {
        //        using (FileStream stream = new FileStream(text1, FileMode.Open, FileAccess.Read))
        //        {
        //            XmlSerializer serializer = null;
        //            try
        //            {
        //                settings = new ScheduleTaskSettings {
        //                    ScheduleTaskDescription = "DocKIT Task Wizard"
        //                };
        //                ScheduleTaskSettings.AssociateProductScheduleHandle(1fujpkZ(), 1RAWDeZ());
        //                ScheduleTaskSettings.ScheduleTaskRunnerAbsolutePath = Application.StartupPath + @"\DocKITTaskRunner.exe";
        //                serializer = new XmlSerializer(typeof(ScheduleTaskSettings));
        //                settings = (ScheduleTaskSettings) serializer.Deserialize(stream);
        //                stream.Close();
        //            }
        //            catch (Exception exception)
        //            {
        //                ErrorLog.ActiveErrorLogWriter.WriteToLog("TaskManager", "DeserializeScheduleTaskSettings", exception.Message);
        //            }
        //        }
        //    }
        //    return settings;
        //}

        //public void 21vp3BZ()
        //{
        //    this.3qwv8Z.AcceptChanges();
        //    if (!Directory.Exists(MZdp3()))
        //    {
        //        Directory.CreateDirectory(MZdp3());
        //    }
        //    XmlTextWriter writer = null;
        //    try
        //    {
        //        writer = new XmlTextWriter(this.H7OpV, Encoding.Unicode);
        //        this.3qwv8Z.WriteXml(writer);
        //    }
        //    catch (Exception exception)
        //    {
        //        ErrorLog.ActiveErrorLogWriter.WriteToLog("TaskManager", "Save", exception.Message);
        //    }
        //    finally
        //    {
        //        if (writer != null)
        //        {
        //            writer.Flush();
        //            writer.Close();
        //            writer = null;
        //        }
        //    }
        //}

        //public static void 25M9daZ(string text1)
        //{
        //    if (2EOpOgZ != null)
        //    {
        //        2EOpOgZ.OnProcessStatusUpdate(text1);
        //    }
        //}

        //[DllImport("shell32.dll", EntryPoint="SHGetFolderPath")]
        //private static extern int 2AuzlrZ(IntPtr, int, IntPtr, uint, [Out] StringBuilder);
        //private void 6NTk1Z()
        //{
        //    string str = string.Empty;
        //    try
        //    {
        //        this.1YGwmWZ = new DataSet("TaskHistory");
        //        this.qVQlLZ = new DataTable("TaskHistory");
        //        this.qVQlLZ.Columns.Add(new DataColumn("TaskName", typeof(string), "", MappingType.Attribute));
        //        this.qVQlLZ.Columns.Add(new DataColumn("TaskType", typeof(string), "", MappingType.Attribute));
        //        this.qVQlLZ.Columns.Add(new DataColumn("StartTime", typeof(DateTime), "", MappingType.Attribute));
        //        this.qVQlLZ.Columns.Add(new DataColumn("EndTime", typeof(DateTime), "", MappingType.Attribute));
        //        this.qVQlLZ.Columns.Add(new DataColumn("Remarks", typeof(string), "", MappingType.Attribute));
        //        this.qVQlLZ.Columns.Add(new DataColumn("DestinationDomainUserName", typeof(string), "", MappingType.Attribute));
        //        this.qVQlLZ.Columns.Add(new DataColumn("Unlock", typeof(bool), "", MappingType.Attribute));
        //        this.qVQlLZ.Columns.Add(new DataColumn("Unread", typeof(bool), "", MappingType.Attribute));
        //        this.qVQlLZ.Columns.Add(new DataColumn("ActivityLogPath", typeof(string), "", MappingType.Attribute));
        //        this.qVQlLZ.PrimaryKey = new DataColumn[] { this.qVQlLZ.Columns["TaskName"], this.qVQlLZ.Columns["StartTime"] };
        //        this.1YGwmWZ.Tables.Add(this.qVQlLZ);
        //        if (Directory.Exists(2lmpl()))
        //        {
        //            foreach (string str2 in Directory.GetDirectories(2lmpl()))
        //            {
        //                try
        //                {
        //                    str = str2.Substring(str2.LastIndexOf(@"\") + 1);
        //                    if (RQ2tB(str))
        //                    {
        //                        using (TaskHistoryBase base2 = new TaskHistoryBase(str, "Unknown"))
        //                        {
        //                            if (base2 != null)
        //                            {
        //                                using (base2.3kPAV())
        //                                {
        //                                    using (DataTable table = base2.Mr5LDZ())
        //                                    {
        //                                        using (DataTableReader reader = table.CreateDataReader())
        //                                        {
        //                                            this.qVQlLZ.Load(reader, LoadOption.OverwriteChanges);
        //                                            this.qVQlLZ.AcceptChanges();
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                            continue;
        //                        }
        //                    }
        //                    ErrorLog.ActiveErrorLogWriter.WriteToLog("TaskManagerBase", "PrepareTaskHistory", new object[] { "Processing all task history files", str2, "There is no associated task for this history and it may consider to move or remove from here" });
        //                }
        //                catch (Exception exception)
        //                {
        //                    ErrorLog.ActiveErrorLogWriter.WriteToLog("TaskManagerBase", "PrepareTaskHistory", new object[] { "Processing all task history files", str2, exception.Message });
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception exception2)
        //    {
        //        ErrorLog.ActiveErrorLogWriter.WriteToLog("TaskManagerBase", "PrepareTaskHistory", exception2.Message);
        //    }
        //}

        //public bool ayan2(string text1)
        //{
        //    bool flag = false;
        //    try
        //    {
        //        if (RQ2tB(text1))
        //        {
        //            Directory.Delete(Path.Combine(MZdp3(), text1), true);
        //            flag = true;
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        ErrorLog.ActiveErrorLogWriter.WriteToLog("TaskManager", "DeleteTask", new object[] { text1, exception.Message });
        //    }
        //    if (flag)
        //    {
        //        string path = "";
        //        try
        //        {
        //            path = Path.Combine(2lmpl(), text1);
        //            if (Directory.Exists(path))
        //            {
        //                Directory.Delete(path, true);
        //            }
        //        }
        //        catch (Exception exception2)
        //        {
        //            flag = false;
        //            MessageBox.Show("The following error occurred attempting to delete task history folder:\n\n" + StringFormat.DoubleQuotes(path) + "\n\n" + exception2.Message, "Deleting Task History...", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        //            ErrorLog.ActiveErrorLogWriter.WriteToLog("TaskManagerBase", "DeleteTask (Task history folder)", new object[] { exception2.Message, "Task Name: " + text1, path });
        //        }
        //        try
        //        {
        //            ScheduleTaskSettings.DeleteTask(text1);
        //        }
        //        catch (Exception exception3)
        //        {
        //            flag = false;
        //            MessageBox.Show("The following error occurred attempting to delete schedule task:\n\n" + exception3.Message, "Deleting Schedule Task...", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        //            ErrorLog.ActiveErrorLogWriter.WriteToLog("TaskManagerBase", "DeleteTask (Schedule Task)", new object[] { exception3.Message, "Task Name: " + text1, path });
        //        }
        //    }
        //    return flag;
        //}

        //public static bool ESmDdZ(object, X509Certificate, X509Chain, SslPolicyErrors)
        //{
        //    return true;
        //}

        //public static void IecagZ()
        //{
        //    1aagfBZ = string.Empty;
        //    1aagfBZ = 1MX9FqZ();
        //}

        //public static bool J9x9u(string text1, ref string textRef2, ref string textRef1)
        //{
        //    bool flag = true;
        //    try
        //    {
        //        text1 = text1.Trim();
        //        textRef1 = Environment.UserDomainName;
        //        textRef2 = Environment.UserName;
        //        if ((text1.Contains(@"\") && !text1.StartsWith(@"\")) && !text1.EndsWith(@"\"))
        //        {
        //            textRef1 = text1.Substring(0, text1.IndexOf(@"\"));
        //            textRef2 = text1.Substring(text1.IndexOf(@"\") + 1);
        //            return flag;
        //        }
        //        textRef1 = string.Empty;
        //        textRef2 = text1;
        //    }
        //    catch (Exception exception)
        //    {
        //        flag = false;
        //        ErrorLog.ActiveErrorLogWriter.WriteToLog("TaskManager", "GetUserAndDomainName", exception.Message);
        //    }
        //    return flag;
        //}

        //public static bool jJDAnZ(string text1, string text2, out long numRef1)
        //{
        //    OleDbConnection connection = null;
        //    OleDbDataAdapter adapter = null;
        //    OleDbCommand command = null;
        //    bool flag = false;
        //    string message = "";
        //    numRef1 = 0L;
        //    if (!File.Exists(text1))
        //    {
        //        message = "Unable to locate / access the metadata file";
        //        flag = true;
        //    }
        //    else
        //    {
        //        StringBuilder builder = new StringBuilder(FWS2Q.PNbtMZ(text1, ref text2));
        //        try
        //        {
        //            connection = new OleDbConnection(builder.ToString());
        //            connection.Open();
        //            numRef1 = 0L;
        //            command = new OleDbCommand {
        //                Connection = connection,
        //                CommandText = string.Format("SELECT count(*) from [{0}] where SourceIndex = -1", text2)
        //            };
        //            numRef1 = Convert.ToInt64(command.ExecuteScalar());
        //            command.Dispose();
        //            command = null;
        //        }
        //        catch (Exception exception)
        //        {
        //            flag = true;
        //            message = message + "\n" + exception.Message;
        //        }
        //        finally
        //        {
        //            if (connection != null)
        //            {
        //                connection.Close();
        //                connection.Dispose();
        //                connection = null;
        //            }
        //            if (adapter != null)
        //            {
        //                adapter.Dispose();
        //                adapter = null;
        //            }
        //        }
        //    }
        //    if (flag)
        //    {
        //        throw new Exception(message);
        //    }
        //    return true;
        //}

        //public static bool NBr6mZ(string text1, out long numRef1)
        //{
        //    bool flag = false;
        //    string message = string.Empty;
        //    string name = string.Empty;
        //    string str3 = string.Empty;
        //    string str4 = string.Empty;
        //    string str5 = string.Empty;
        //    numRef1 = 0L;
        //    try
        //    {
        //        name = "FileErrors";
        //        str3 = "FolderErrors";
        //        str4 = "FileUnprocessed";
        //        str5 = "FolderUnprocessed";
        //        if (!File.Exists(text1))
        //        {
        //            message = "Unable to locate / access the " + text1 + " file.";
        //            flag = true;
        //        }
        //        else
        //        {
        //            using (XmlReader reader = XmlReader.Create(text1))
        //            {
        //                while (reader.Read())
        //                {
        //                    if ((((reader.NodeType == XmlNodeType.Element) && string.Equals(reader.Name, "Item", StringComparison.InvariantCultureIgnoreCase)) && reader.HasAttributes) && (((int.Parse(reader.GetAttribute(name)) != 0) || (int.Parse(reader.GetAttribute(str3)) != 0)) || ((int.Parse(reader.GetAttribute(str4)) != 0) || (int.Parse(reader.GetAttribute(str5)) != 0))))
        //                    {
        //                        numRef1 += 1L;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        message = "Unable to process the " + text1 + " file schema structure. Task is created in different Dockit version.";
        //        flag = true;
        //        ErrorLog.ActiveErrorLogWriter.WriteToLog("TaskManager", "IsActivityErrorsAndUnProcessedItems", exception.Message);
        //    }
        //    if (flag)
        //    {
        //        throw new Exception(message);
        //    }
        //    return true;
        //}

        //public static bool QU4q(string text1, out long numRef1)
        //{
        //    numRef1 = 0L;
        //    try
        //    {
        //        if (!File.Exists(text1))
        //        {
        //            ErrorLog.ActiveErrorLogWriter.WriteToLog("TaskManager", "IsActivityGotErrors", new object[] { "Parsing problem", string.Format("Unable to locate / access {0}", text1) });
        //        }
        //        else if (text1 != null)
        //        {
        //            OleDbConnection connection = null;
        //            StringBuilder builder = null;
        //            string str = "untitled";
        //            long num = 0L;
        //            builder = new StringBuilder(FWS2Q.PNbtMZ(text1, ref str));
        //            connection = new OleDbConnection(builder.ToString());
        //            connection.Open();
        //            num = Convert.ToInt64(new OleDbCommand { Connection = connection, CommandText = "SELECT count(ActivityDate) from [activityerrors.txt]" }.ExecuteScalar());
        //            numRef1 = num;
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        ErrorLog.ActiveErrorLogWriter.WriteToLog("TaskManager", "IsActivityGotErrors", exception.Message);
        //        return false;
        //    }
        //    return true;
        //}

        //public static bool RQ2tB(string text1)
        //{
        //    return File.Exists(4NFdz.1uqrKEZ(text1));
        //}

        //public static void UOiDqZ(DataTable table1, string text1)
        //{
        //    XmlTextWriter writer = null;
        //    try
        //    {
        //        writer = new XmlTextWriter(text1, Encoding.Unicode);
        //        table1.WriteXml(writer);
        //    }
        //    catch (Exception exception)
        //    {
        //        ErrorLog.ActiveErrorLogWriter.WriteToLog("TaskManager", "WriteUnicodeXmlFile", exception.Message);
        //    }
        //    finally
        //    {
        //        if (writer != null)
        //        {
        //            writer.Flush();
        //            writer.Close();
        //            writer = null;
        //        }
        //    }
        //}

        //public static bool Ut3lJZ(DriveInfo info1, out string textRef1)
        //{
        //    RegistryKey key = null;
        //    bool flag = false;
        //    textRef1 = string.Empty;
        //    if (info1 != null)
        //    {
        //        try
        //        {
        //            if (info1.DriveType == DriveType.Network)
        //            {
        //                string name = @"Network\" + info1.Name.Substring(0, 1);
        //                key = Registry.CurrentUser.OpenSubKey(name);
        //                if (key != null)
        //                {
        //                    textRef1 = key.GetValue("RemotePath").ToString();
        //                    flag = true;
        //                }
        //                return flag;
        //            }
        //            textRef1 = info1.RootDirectory.FullName;
        //            return flag;
        //        }
        //        catch (Exception exception)
        //        {
        //            textRef1 = string.Empty;
        //            ErrorLog.ActiveErrorLogWriter.WriteToLog("TaskManagerBase", "GetUncRemotePath", exception.Message);
        //        }
        //        finally
        //        {
        //            if (key != null)
        //            {
        //                key.Close();
        //                key = null;
        //            }
        //        }
        //    }
        //    return flag;
        //}

        //public static string z4s4E(string text2, string text1)
        //{
        //    try
        //    {
        //        nD3JT = string.Empty;
        //        if (File.Exists(Path.Combine(text1, "DocKITConfiguration.xml")))
        //        {
        //            DMO10Z.gsv3p(text1);
        //            if (DMO10Z.1cxWraZ.CommonUserDataPath.EndsWith(@"\" + text2, StringComparison.InvariantCultureIgnoreCase))
        //            {
        //                return DMO10Z.1cxWraZ.CommonUserDataPath;
        //            }
        //            return Path.Combine(DMO10Z.1cxWraZ.CommonUserDataPath, text2);
        //        }
        //        StringBuilder builder = new StringBuilder();
        //        int num = 0x2e;
        //        2AuzlrZ(IntPtr.Zero, num, IntPtr.Zero, 0, builder);
        //        return Path.Combine(builder.ToString(), text2);
        //    }
        //    catch (Exception exception)
        //    {
        //        nD3JT = exception.Message;
        //        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Dockit9x");
        //    }
        //}

        //public static string 12laFN
        //{
        //    get
        //    {
        //        return Path.Combine(G0OCGZ(), "Config");
        //    }
        //}

        //public static string 15k7AjZ
        //{
        //    get
        //    {
        //        return Path.Combine(1FgqiKZ(), "DocKITConfiguration.xml");
        //    }
        //}

        //public static string 193J2yZ
        //{
        //    get
        //    {
        //        return Path.Combine(G0OCGZ(), "Log");
        //    }
        //}

        //public bool 19GttN
        //{
        //    get
        //    {
        //        DataRow row = null;
        //        try
        //        {
        //            if ((this.1er5ef != null) && (this.1er5ef.Rows.Count == 1))
        //            {
        //                row = this.1er5ef.Rows[0];
        //                return (bool) row["LoadTaskStatusAtStartup"];
        //            }
        //        }
        //        catch (Exception exception)
        //        {
        //            ErrorLog.ActiveErrorLogWriter.WriteToLog("TaskManager", "LoadTaskStatusAtStartup", exception.Message);
        //        }
        //        return true;
        //    }
        //    set
        //    {
        //        DataRow row = null;
        //        try
        //        {
        //            if (this.1er5ef != null)
        //            {
        //                if (this.1er5ef.Rows.Count == 1)
        //                {
        //                    row = this.1er5ef.Rows[0];
        //                }
        //                else
        //                {
        //                    row = this.1er5ef.NewRow();
        //                    this.1er5ef.Rows.Add(row);
        //                }
        //                row["LoadTaskStatusAtStartup"] = value;
        //                this.1er5ef.AcceptChanges();
        //                this.21vp3BZ();
        //            }
        //        }
        //        catch (Exception exception)
        //        {
        //            ErrorLog.ActiveErrorLogWriter.WriteToLog("TaskManager", "LoadTaskStatusAtStartup", exception.Message);
        //        }
        //    }
        //}

        //public static string 19lgGFZ
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(CvCXuZ))
        //        {
        //            CvCXuZ = 1aBwytZ("ProgramFiles");
        //        }
        //        return CvCXuZ;
        //    }
        //}

        //public int 1Bnm4S
        //{
        //    get
        //    {
        //        DataRow row = null;
        //        try
        //        {
        //            if ((this.1er5ef != null) && (this.1er5ef.Rows.Count == 1))
        //            {
        //                row = this.1er5ef.Rows[0];
        //                if (!DBNull.Value.Equals(row["MaxTaxonomyRetrieveCount"]))
        //                {
        //                    return (int) row["MaxTaxonomyRetrieveCount"];
        //                }
        //            }
        //        }
        //        catch (Exception exception)
        //        {
        //            ErrorLog.ActiveErrorLogWriter.WriteToLog("TaskManager", "MaxTaxonomyRetrieveCount", exception.Message);
        //        }
        //        return 100;
        //    }
        //    set
        //    {
        //        DataRow row = null;
        //        try
        //        {
        //            if (this.1er5ef != null)
        //            {
        //                if (this.1er5ef.Rows.Count == 1)
        //                {
        //                    row = this.1er5ef.Rows[0];
        //                }
        //                else
        //                {
        //                    row = this.1er5ef.NewRow();
        //                    this.1er5ef.Rows.Add(row);
        //                }
        //                row["MaxTaxonomyRetrieveCount"] = value;
        //                this.1er5ef.AcceptChanges();
        //                this.21vp3BZ();
        //            }
        //        }
        //        catch (Exception exception)
        //        {
        //            ErrorLog.ActiveErrorLogWriter.WriteToLog("TaskManager", "MaxTaxonomyRetrieveCount", exception.Message);
        //        }
        //    }
        //}

        //public static string 1cNCTu
        //{
        //    get
        //    {
        //        return "DocKITScheduleMgrLIB_1286";
        //    }
        //}

        //public static string 1jJmsd
        //{
        //    get
        //    {
        //        return Path.Combine(G0OCGZ(), "TaskHistory");
        //    }
        //}

        //public bool 1mSDTFZ
        //{
        //    get
        //    {
        //        DataRow row = null;
        //        try
        //        {
        //            if ((this.1er5ef != null) && (this.1er5ef.Rows.Count == 1))
        //            {
        //                row = this.1er5ef.Rows[0];
        //                if (!DBNull.Value.Equals(row["UseDistinuguisedNameForSecurityDistributionGroupInSP2010"]))
        //                {
        //                    return (bool) row["UseDistinuguisedNameForSecurityDistributionGroupInSP2010"];
        //                }
        //            }
        //        }
        //        catch (Exception exception)
        //        {
        //            ErrorLog.ActiveErrorLogWriter.WriteToLog("TaskManager", "UseDistinuguisedNameForSecurityDistributionGroupInSP2010", exception.Message);
        //        }
        //        return true;
        //    }
        //    set
        //    {
        //        DataRow row = null;
        //        try
        //        {
        //            if (this.1er5ef != null)
        //            {
        //                if (this.1er5ef.Rows.Count == 1)
        //                {
        //                    row = this.1er5ef.Rows[0];
        //                }
        //                else
        //                {
        //                    row = this.1er5ef.NewRow();
        //                    this.1er5ef.Rows.Add(row);
        //                }
        //                row["UseDistinuguisedNameForSecurityDistributionGroupInSP2010"] = value;
        //                this.1er5ef.AcceptChanges();
        //                this.21vp3BZ();
        //            }
        //        }
        //        catch (Exception exception)
        //        {
        //            ErrorLog.ActiveErrorLogWriter.WriteToLog("TaskManager", "LoadTaskStatusAtStartup", exception.Message);
        //        }
        //    }
        //}

        //public static string 1QOHT0
        //{
        //    get
        //    {
        //        return "DocKITScheduleMgr.JobScheduler";
        //    }
        //}

        //public static string 1Wk4yY
        //{
        //    get
        //    {
        //        return Path.Combine(G0OCGZ(), "Tasks");
        //    }
        //}

        //public DataTable 1xXdwtZ
        //{
        //    get
        //    {
        //        return this.qVQlLZ;
        //    }
        //}

        //public static string 24ZVWj
        //{
        //    get
        //    {
        //        return Path.Combine(1FgqiKZ(), "ProductStatisticsManager.dat");
        //    }
        //}

        //public static bool 2AZpiTZ
        //{
        //    get
        //    {
        //        return haR8QZ;
        //    }
        //    set
        //    {
        //        haR8QZ = value;
        //    }
        //}

        //public static string 2bSoBJ
        //{
        //    get
        //    {
        //        return nD3JT;
        //    }
        //}

        //public static string 2daM4pZ
        //{
        //    get
        //    {
        //        return Path.Combine(2dIulhZ(), "ColumnMappingTemplates");
        //    }
        //}

        //public static string 2dKECd
        //{
        //    get
        //    {
        //        return Path.Combine(G0OCGZ(), "Temp");
        //    }
        //}

        //public static string 2xJYlZ
        //{
        //    get
        //    {
        //        return Path.Combine(2dIulhZ(), "ProductSettings.xml");
        //    }
        //}

        //public static string 3zB7NZ
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(qWLsU))
        //        {
        //            qWLsU = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), @"Vyapin\DocKIT\9x");
        //        }
        //        return qWLsU;
        //    }
        //}

        //public DataSet 90qrNZ
        //{
        //    get
        //    {
        //        return this.3qwv8Z;
        //    }
        //}

        //public static string APjQJ
        //{
        //    get
        //    {
        //        string str = string.Empty;
        //        try
        //        {
        //            WindowsIdentity current = WindowsIdentity.GetCurrent();
        //            if (current != null)
        //            {
        //                str = current.User.Value;
        //            }
        //        }
        //        catch (Exception exception)
        //        {
        //            str = string.Empty;
        //            ErrorLog.ActiveErrorLogWriter.WriteToLog("TaskManagerBase", "CurrentUserSid", new object[] { Environment.UserDomainName, Environment.UserName, exception.Message });
        //        }
        //        if (string.IsNullOrWhiteSpace(str))
        //        {
        //            return "all";
        //        }
        //        return str;
        //    }
        //}

        //public static string FzaerZ
        //{
        //    get
        //    {
        //        return Path.Combine(G0OCGZ(), "Categories.xml");
        //    }
        //}

        //public static char[] Jd4T3Z
        //{
        //    get
        //    {
        //        string str = "\\/:*?\"<>|";
        //        return str.ToCharArray();
        //    }
        //}

        //public static string jyA6wZ
        //{
        //    get
        //    {
        //        return Path.Combine(G0OCGZ(), "Backup");
        //    }
        //}

        //public static string rVQ6Z
        //{
        //    get
        //    {
        //        return MZdp3();
        //    }
        //}

        //public static int ScDiW
        //{
        //    get
        //    {
        //        return BdKAqZ;
        //    }
        //    set
        //    {
        //        BdKAqZ = value;
        //    }
        //}

        //public static string tkDDV
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(1aagfBZ))
        //        {
        //            1aagfBZ = 1MX9FqZ();
        //        }
        //        return 1aagfBZ;
        //    }
        //}
    }
//}

}
