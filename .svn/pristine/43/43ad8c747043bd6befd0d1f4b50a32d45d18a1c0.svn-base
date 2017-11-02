namespace EPWI.Components.Models
{
  public class LogRepository : Repository
  {
    public void AddFaxLogEntry(FaxLog log)
    {
      Db.FaxLogs.InsertOnSubmit(log);
    }

    public void AddActionLogEntry(ActivityLog logEntry)
    {
      Db.ActivityLogs.InsertOnSubmit(logEntry);
    }
  }
}
