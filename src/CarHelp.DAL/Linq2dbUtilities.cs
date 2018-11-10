using CarHelp.DAL.Entities;
using LinqToDB;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarHelp.DAL
{
    internal static class Linq2dbUtilities
    {
        [Sql.Expression("ST_DWithin({0}::geography, {1}::geography, {2})", ServerSideOnly = true)]
        public static bool PgisWithinDistance(this Geometry geom1, Geometry geom2, double distance)
        {
            throw new InvalidOperationException();
        }

        [Sql.Function("ST_X", ServerSideOnly = true)]
        public static double PgisLongitude(this Geometry location)
        {
            throw new InvalidOperationException();
        }

        [Sql.Function("ST_Y", ServerSideOnly = true)]
        public static double PgisLatitude(this Geometry location)
        {
            throw new InvalidOperationException();
        }

        [Sql.Function("ST_DistanceSphere", ServerSideOnly = true)]
        public static double PgisDistance(this Geometry geometry1, Geometry geometry2)
        {
            throw new InvalidOperationException();
        }

        [Sql.Expression("{0} BETWEEN {1} AND {2}", PreferServerSide = true)]
        public static bool Between<T>(this T x, T low, T high) where T : IComparable<T>
        {
            return x.CompareTo(low) >= 0 && x.CompareTo(high) <= 0;
        }
    }
}
