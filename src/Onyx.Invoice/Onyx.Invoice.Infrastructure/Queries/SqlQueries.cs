using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onyx.Invoice.Infrastructure.Queries
{
    public static class SqlQueries
    {
        public const string TravelAgentsWithNoObservations = @"
            SELECT DISTINCT t.TravelAgent
            FROM TravelAgent t
            LEFT JOIN Observation o ON t.TravelAgent = o.TravelAgent
            WHERE o.TravelAgent IS NULL";

        public const string TravelAgentsWithMoreThanTwoObservations = @"
            SELECT o.TravelAgent, COUNT(o.Id) AS ObservationCount
            FROM Observation o
            GROUP BY o.TravelAgent
            HAVING COUNT(o.Id) > 2";
    }
}
