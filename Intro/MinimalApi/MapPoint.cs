using System.Reflection;

namespace MinimalApi;

public class MapPoint
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    // Allows you to deal with special needs, IE a old api that sends a common seperated lat and long, but can't send it as the body, this will enable the minimal api to understand "map-point?latAndLong=5.448,56464.584" as a proper value for a endpoint
    public static bool TryParse(string? value, out MapPoint? point)
    {
        try
        {
            var splitValue = value?.Split(',').Select(double.Parse).ToArray();
            point = new MapPoint()
            {
                Latitude = splitValue![0],
                Longitude = splitValue[1]
            };
            return true;
        }
        catch (Exception)
        {
            point = null;
            return false;
        }
    }
}

public class MapPoint2
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    // Allows the same as above but if the body was only allowed to be a simple type.
    public static async ValueTask<MapPoint2?> BindAsync(HttpContext context, ParameterInfo parameterInfo)
    {
        var rawRequestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
        try
        {
            var splitValue = rawRequestBody?.Split(',').Select(double.Parse).ToArray();
            return new MapPoint2()
            {
                Latitude = splitValue![0],
                Longitude = splitValue[1]
            };
        }
        catch (Exception)
        {
            return null;
        }
    }
}
