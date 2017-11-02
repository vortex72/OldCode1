using System.Linq;

namespace EPWI.Components.Models
{
    public class LineRepository : Repository
    {
        public IQueryable<Line> GetAllLines(bool includeExclusions)
        {
            var lines = from line in Db.Lines where line.Displayable select line;

            if (!includeExclusions)
            {
                lines = from line in lines
                        where !(from exclusion in Db.DataDownloadExclusions select exclusion.LineCode).Contains(line.LINE)
                        select line;
            }

            return lines;
        }

    }
}
